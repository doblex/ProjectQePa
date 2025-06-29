using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingController : DocControllerWithFade
{
    VisualElement loadingMessage;

    bool isShowing = false;
    [Header("Wave Animation Settings")]
    [SerializeField] float waveSpeed = 4f;
    [SerializeField] float waveHeight = 10f;
    [SerializeField] float waveFrequency = 0.5f;

    private List<Label> childLabels = new List<Label>();

    private void Start()
    {
        childLabels.Clear();
        foreach (var child in loadingMessage.Children())
        {
            if (child is Label label)
            {
                childLabels.Add(label);
            }
        }
    }

    protected override void SetComponents()
    {
        loadingMessage = Root.Q<VisualElement>("LoadingMessage");
    }

    public override void ShowDoc(bool show)
    {
        base.ShowDoc(show);
        isShowing = show;
    }

    private void Update()
    {
        if (isShowing == false) return;

        for (int i = 0; i < childLabels.Count; i++)
        {
            var label = childLabels[i];
            float offset = Mathf.Sin(Time.time * waveSpeed + i * waveFrequency) * waveHeight;

            label.style.top = offset;
        }
    }
}
