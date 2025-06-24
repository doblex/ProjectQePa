using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class MonoBehaviourWithAudio : MonoBehaviour
{
    protected AudioSource[] audioChannels;
    public delegate void AudioSourceDelegate(AudioSource[] channels);
    public AudioSourceDelegate onPlayAudio;
    public AudioSourceDelegate onStopAudio;

    protected void Start()
    {
        audioChannels = GetComponents<AudioSource>();
    }
}
