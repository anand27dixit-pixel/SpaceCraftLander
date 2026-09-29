using System;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_SCALE = 0.7f;
    private const float MAX_FUEL_AMOUNT = 10f;
    // Singleton
    public static Lander Instance { get; private set; }
    private Rigidbody2D landerRigidBody2D;
    private STATE currentState;
    [SerializeField] private float fuelAmount = 0f;

    // Events
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler OnFuelPickup;

    public event EventHandler<LandedEventArgs> OnLanded;
    public class LandedEventArgs : EventArgs
    {
        public int score;
        public LANDING_TYPE landingType;
        public float landingSpeed;
        public int landingScoreMultipler;
        public float landingAngle;

    }

    public event EventHandler<StateEventArgs> OnStateChanged;
    public class StateEventArgs : EventArgs
    {
        public STATE stateChanged;
    }

    public enum LANDING_TYPE
    {
        SUCCESSFUL_LANDING,
        FAST_LANDING,
        LANDING_ON_STEVE_ANGLE,
        CRASHED,
        FIRED
    }

    // Key Functionality 
    public event EventHandler KeyPickUp_CanKeyAccess;
    public event EventHandler KeyDrop_CanKeyAccess;

    public GameObject landerAttacher;


    private void Awake()
    {
        Instance = this;
        currentState = STATE.WAITING_TO_START;
        landerRigidBody2D = GetComponent<Rigidbody2D>();
        landerRigidBody2D.gravityScale = 0f;
        fuelAmount = MAX_FUEL_AMOUNT;
        Debug.Log("lander RigidBody " + landerRigidBody2D);
        landerAttacher = null;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        // Before Starting each Frame Particle should Stops
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (currentState)
        {
            default:
            case STATE.WAITING_TO_START:
                if (GameInput.Instance.IsUpKeyPressed() ||
          GameInput.Instance.IsLeftKeyPressed() ||
          GameInput.Instance.IsRightKeyPressed() ||
          GameInput.Instance.GetMovementFromJoystick() != Vector2.zero)
                {
                    ConsumeFuel();
                    landerRigidBody2D.gravityScale = GRAVITY_SCALE;
                    SetState(STATE.NORMAL);
                }
                break;
            case STATE.NORMAL:
                if (fuelAmount <= 0f)
                {
                    Debug.Log("Crashed Emptied Fuel ");
                    fuelAmount = 0;
                    SetState(STATE.GAMEOVER);
                    return;
                }

                if (GameInput.Instance.IsUpKeyPressed() || GameInput.Instance.IsLeftKeyPressed() ||
   GameInput.Instance.IsRightKeyPressed() || GameInput.Instance.GetMovementFromJoystick() != Vector2.zero)
                {
                    ConsumeFuel();
                }
                float movementDeadZone = 0.4f;
                
                if (GameInput.Instance.IsUpKeyPressed() || GameInput.Instance.GetMovementFromJoystick().y > movementDeadZone)
                {
                    float force = 700f;
                    landerRigidBody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }


                if (GameInput.Instance.IsLeftKeyPressed() || GameInput.Instance.GetMovementFromJoystick().x < -movementDeadZone)
                {
                    float turnSpeed = 100f;
                    landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }


                if (GameInput.Instance.IsRightKeyPressed() || GameInput.Instance.GetMovementFromJoystick().x > movementDeadZone)
                {
                    float turnSpeed = -100f;
                    landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }

                break;
            case STATE.GAMEOVER:
                break;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

        if (!collision2D.transform.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Not Landed on LANDING PAD");
            OnLanded?.Invoke(this, new LandedEventArgs
            {
                score = 0,
                landingType = LANDING_TYPE.CRASHED,
                landingSpeed = 0f,
                landingScoreMultipler = 0,
                landingAngle = 0
            });
            SetState(STATE.GAMEOVER);
            return;
        }

        float safeLandingVelocity = 4f;
        float relativeLandingVelocity = collision2D.relativeVelocity.magnitude;
        // Relative Velocity
        if (relativeLandingVelocity > safeLandingVelocity)
        {
            Debug.Log("Relative Velocity " + collision2D.relativeVelocity.magnitude);
            OnLanded?.Invoke(this, new LandedEventArgs
            {
                score = 0,
                landingType = LANDING_TYPE.FAST_LANDING,
                landingSpeed = relativeLandingVelocity,
                landingScoreMultipler = 0,
                landingAngle = 0
            });

            SetState(STATE.GAMEOVER);
            return;
        }

        float safeLandingAngle = 0.9f;
        // Angle Detection.
        float dotProduct = Vector2.Dot(Vector2.up, transform.up);
        Debug.Log("DOT PRODUCT " + dotProduct);
        if (dotProduct < safeLandingAngle)
        {
            Debug.Log("Angle Detection " + dotProduct + "Sleet angle");
            OnLanded?.Invoke(this, new LandedEventArgs
            {
                score = 0,
                landingType = LANDING_TYPE.LANDING_ON_STEVE_ANGLE,
                landingSpeed = relativeLandingVelocity,
                landingScoreMultipler = 0,
                landingAngle = dotProduct
            });
            SetState(STATE.GAMEOVER);
            return;
        }

        Debug.Log("Safe landing on Both Velocity & Angle");

        // Score Calculation
        float maxSafeLandingAngleScore = 100;
        float scoreDotVectorMultipler = 10;

        float landingAngleScore = maxSafeLandingAngleScore - Mathf.Abs(dotProduct - 1f) * scoreDotVectorMultipler * maxSafeLandingAngleScore;

        float maxSafeLandingScore = 100;

        float safeLandingScore = (safeLandingVelocity - relativeLandingVelocity) * maxSafeLandingScore;

        Debug.Log("LandingAngleScore " + landingAngleScore);
        Debug.Log("safeLandingScore " + safeLandingScore);

        int score = Mathf.RoundToInt(safeLandingScore + landingAngleScore) * landingPad.GetScoreMultipler();

        Debug.Log("Score : " + score);

        OnLanded?.Invoke(this, new LandedEventArgs
        {
            score = score,
            landingType = LANDING_TYPE.SUCCESSFUL_LANDING,
            landingSpeed = relativeLandingVelocity,
            landingScoreMultipler = landingPad.GetScoreMultipler(),
            landingAngle = dotProduct
        });

        SetState(STATE.GAMEOVER);
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        // FuelPickup
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuel))
        {
            fuelAmount += fuel.GetFuelMultipler();
            // Fuel Amount should not greater than max fuel amount
            if (fuelAmount > MAX_FUEL_AMOUNT)
                fuelAmount = MAX_FUEL_AMOUNT;
            fuel.DestroySelf();
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
        }

        // CoinPickup
        if (collider2D.gameObject.TryGetComponent(out CoinPickUp coin))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coin.DestroySelf();
        }

        // Fired By Bullet
        if (collider2D.gameObject.TryGetComponent(out Bullet bulletFired))
        {
            OnLanded.Invoke(this, new LandedEventArgs()
            {
                score = 0,
                landingType = LANDING_TYPE.FIRED,
                landingAngle = 0,
                landingScoreMultipler = 0,
                landingSpeed = 0
            });
            bulletFired.DestroySelfImmdiate();
        }

        // Key Picked up
        if (collider2D.gameObject.TryGetComponent(out KeyPickUp keyPickUp))
        {
            Debug.Log("KeyUp GameObject " + keyPickUp.gameObject.name);
            KeyPickUp_CanKeyAccess.Invoke(this, EventArgs.Empty);
        }

          // Key Picked up
        if (collider2D.gameObject.TryGetComponent(out KeyDrop keyDrop))
        {
            Debug.Log("KeyUp GameObject " + keyDrop.gameObject.name);
            KeyDrop_CanKeyAccess.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetState(STATE stateToChange)
    {
        currentState = stateToChange;
        OnStateChanged?.Invoke(this, new StateEventArgs
        {
            stateChanged = stateToChange
        });
    }

    private void ConsumeFuel()
    {
        fuelAmount -= 1f * Time.deltaTime;
    }

    #region PUBLIC_METHODS
    public float GetSpeedX() => landerRigidBody2D.velocity.x;
    public float GetSpeedY() => landerRigidBody2D.velocity.y;

    public float GetFuelNormalziedAmount() => fuelAmount / MAX_FUEL_AMOUNT;

    #endregion // PUBLIC_METHODS

}
