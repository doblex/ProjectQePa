using UnityEngine;

public class AudioObserver : MonoBehaviour
{
    [SerializeField] MonoBehaviourWithAudio observed;
    [SerializeField] string audioFile;
    [SerializeField] AudioMode audioMode;

    private AudioClip clip;

    void Start()
    {
        clip = (AudioClip)Resources.Load(audioFile);
        switch (audioMode)
        {
            case AudioMode.Once:
                observed.onPlayAudio += PlayOnce;
                break;
            case AudioMode.Loop:
                observed.onPlayAudio += PlayLoop;
                observed.onStopAudio += StopLoop;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    public void PlayOnce(AudioSource aS)
    {
        aS.loop = false;
        aS.PlayOneShot(clip);
    }

    public void PlayLoop(AudioSource aS)
    {
        aS.loop = true;
        aS.clip = clip;
        aS.Play();
    }

    public void StopLoop(AudioSource aS)
    {
        aS.Stop();
    }
}
