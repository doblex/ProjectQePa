using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class VideoComicController : DocController
{
    [Header("Video")]
    [SerializeField] VideoPlayer videoPlayer;

    Button skip;

    Action onEndVideo;

    protected override void SetComponents()
    {
        skip = Root.Q<Button>("Skip");

        skip.clicked += Skip_clicked;
    }

    public void Show(VideoClip videoClip, Action onEndVideo)
    {
        ShowDoc(true);

        if (videoPlayer == null)
        {
            Debug.LogWarning("VideoPlayer is not assigned in UIController.");
            return;
        }

        if (videoClip == null)
        {
            Debug.LogWarning("VideoClip is not assigned in VideoComicController.");
            return;
        }
        videoPlayer.targetCamera = Camera.main;
        this.onEndVideo = onEndVideo;
        videoPlayer.clip = videoClip;
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Hide()
    {
        ShowDoc(false);
        videoPlayer.Stop();
        videoPlayer.loopPointReached -= OnVideoEnd;
        onEndVideo?.Invoke();
        onEndVideo = null;
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        Hide();
    }

    private void Skip_clicked()
    {
        Hide();
    }
}
