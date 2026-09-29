using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float BULLET_FORCE= 25f;
    private Rigidbody2D bulletRigidBody2D;
    // Start is called before the first frame update
    void Awake()
    {
        bulletRigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void ShootBulletInDirection(Vector2 direction)
    {
        bulletRigidBody2D.velocity = direction * BULLET_FORCE;
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject,5f);
    }

    public void DestroySelfImmdiate()
    {
        Destroy(gameObject);
    }
}
