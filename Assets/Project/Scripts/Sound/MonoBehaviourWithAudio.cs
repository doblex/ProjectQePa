using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class MonoBehaviourWithAudio : MonoBehaviour
{
    protected AudioSource[] audioChannels;
    public delegate void AudioSourceDelegate(AudioSource[] channels);
    public AudioSourceDelegate OnPlayAudio;
    public AudioSourceDelegate OnStopAudio;
    public AudioSourceDelegate OnUnselectAudio;

    protected void Start()
    {
        audioChannels = GetComponents<AudioSource>();
    }
}
