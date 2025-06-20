using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class MonoBehaviourWithAudio : MonoBehaviour
{
    protected AudioSource aS;
    public delegate void AudioSourceDelegate(AudioSource source);
    public AudioSourceDelegate onPlayAudio;
    public AudioSourceDelegate onStopAudio;

    protected void Start()
    {
        aS = GetComponent<AudioSource>();
    }
}
