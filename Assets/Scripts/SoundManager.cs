using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int SOUND_VOLUMN_MAX = 10;

    private static int soundVolumn = 5;
   
    public static SoundManager Instance {get; private set;}

    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip fuelPickupSound;
    [SerializeField] private AudioClip successLandingSound;
    [SerializeField] private AudioClip crashLandingSound;

    private Camera mainCamera;

    // Event
    public event EventHandler OnSoundVolumeChanged;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickUp;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickUp;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.LandedEventArgs e)
    {
        if (e.landingType == Lander.LANDING_TYPE.SUCCESSFUL_LANDING)
        {
            PlaySoundOnPoint(successLandingSound);
        }
        else
        {
            PlaySoundOnPoint(crashLandingSound);
        }
    }

    private void Lander_OnFuelPickUp(object sender, EventArgs e)
    {
        PlaySoundOnPoint(fuelPickupSound);
    }

    private void Lander_OnCoinPickUp(object sender, EventArgs e)
    {
        PlaySoundOnPoint(coinPickupSound);
    }

    private void PlaySoundOnPoint(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, mainCamera.transform.position, GetSoundVolumnNormalized());
    }

    #region PUBLIC_METHODS
    public void ChangedSoundVolumne()
    {
        soundVolumn = (soundVolumn + 1) % SOUND_VOLUMN_MAX;
        Debug.Log("Sound Volumn " + soundVolumn);
        OnSoundVolumeChanged?.Invoke(this,EventArgs.Empty);
    }

    public int GetSoundVolumn() => soundVolumn;

    public float GetSoundVolumnNormalized()
    {
        Debug.Log("GetSoundVolumnNormalized" + ((float) soundVolumn) / SOUND_VOLUMN_MAX);
        return ((float) soundVolumn) / SOUND_VOLUMN_MAX;
    }
    #endregion // PUBLIC_METHODS
}
