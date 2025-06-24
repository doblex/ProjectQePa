using UnityEngine;

public class AudioObserver : MonoBehaviour
{
    [SerializeField] MonoBehaviourWithAudio observed;
    [SerializeField] string audioFile;
    [SerializeField] AudioMode audioMode;
    [SerializeField] int audioChannelIndex = 0;
    [SerializeField, Range(0, 1)] float soundVolume = 1;

    private AudioClip clip;

    void Start()
    {
        clip = (AudioClip)Resources.Load(audioFile);
        switch (audioMode)
        {
            case AudioMode.Once:
                observed.OnPlayAudio += PlayOnce;
                break;
            case AudioMode.Loop:
                observed.OnPlayAudio += PlayLoop;
                observed.OnStopAudio += StopLoop;
                break;
            case AudioMode.Unselect:
                observed.OnUnselectAudio += PlayOnceOnUnselect;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    public void PlayOnce(AudioSource[] channels)
    {
        channels[audioChannelIndex].volume = soundVolume;
        channels[audioChannelIndex].loop = false;
        channels[audioChannelIndex].PlayOneShot(clip);
    }

    public void PlayLoop(AudioSource[] channels)
    {
        channels[audioChannelIndex].volume = soundVolume;
        channels[audioChannelIndex].loop = true;
        channels[audioChannelIndex].clip = clip;
        channels[audioChannelIndex].Play();
    }

    public void StopLoop(AudioSource[] channels)
    {
        channels[audioChannelIndex].Stop();
    }

    public void PlayOnceOnUnselect(AudioSource[] channels)
    {
        channels[audioChannelIndex].volume = soundVolume;
        channels[audioChannelIndex].loop = false;
        channels[audioChannelIndex].PlayOneShot(clip);
    }
}
