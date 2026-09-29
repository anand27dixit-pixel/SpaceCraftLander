using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource landerThrusterAudioSource;
    private Lander lander;
    // Start is called before the first frame update
    void Start()
    {
        lander = GetComponent<Lander>();

        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnLeftForce += Lander_OnLeftForce;

        // On Sound Volumne Change
        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumnChanged;

        landerThrusterAudioSource.Pause();
    }

    private void SoundManager_OnSoundVolumnChanged(object sender, EventArgs e)
    {
        landerThrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumnNormalized();
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        if(!landerThrusterAudioSource.isPlaying)
        landerThrusterAudioSource.Play();
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
         if(!landerThrusterAudioSource.isPlaying)
        landerThrusterAudioSource.Play();
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        if(!landerThrusterAudioSource.isPlaying)
        landerThrusterAudioSource.Play();
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        landerThrusterAudioSource.Pause();
    }
}
