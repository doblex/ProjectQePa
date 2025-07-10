using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using utilities.Controllers;

public class HUDController : DocController
{
    [SerializeField] float resetFillTime = 1f;

    VisualElement healthContainer;

    Button pause;
    Button reset;
    VisualElement resetFiller;

    bool isFilling = false;

    private Coroutine resetCoroutine;

    protected override void SetComponents()
    {
        healthContainer = Root.Q<VisualElement>("HealtContainer");

        pause = Root.Q<Button>("Pause");
        pause.clicked += Pause_clicked;

        reset = Root.Q<Button>("Reset");

        reset.RegisterCallback<PointerDownEvent>(evt =>
        {
            if (resetCoroutine != null) return;

            resetCoroutine = StartCoroutine(ResetFill());
        }, TrickleDown.TrickleDown);

        reset.RegisterCallback<PointerUpEvent>(evt =>
        {
            CancelReset();
        });

        reset.RegisterCallback<PointerCancelEvent>(evt =>
        {
            CancelReset();
        });


        resetFiller = Root.Q<VisualElement>("ResetFiller");
        resetFiller.style.width = new StyleLength(new Length(0, LengthUnit.Percent)); 
        // Reset button
    }

    private void Pause_clicked()
    {
        UIController.Instance.ShowPause(true);
    }

    public void SubcribeToHealth(HealthController controller)
    {
        controller.onDamage += OnPlayerDamaged;
    }

    public void UnsubcribeToHealth(HealthController controller)
    {
        controller.onDamage -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged(float MaxHp, float currentHp)
    {
        healthContainer.Clear();

        VisualTreeAsset template;
        VisualElement healthUnit;

        for (int i = 0; i < MaxHp; i++)
        {
            if (i < currentHp)
            {
                template = UIController.Instance.HealthUnitTempleate;
                healthUnit = template.CloneTree();
            }
            else
            {
                template = UIController.Instance.EmptyhealthUnitTempleate;
                healthUnit = template.CloneTree();
            }

            healthContainer.Add(healthUnit);
        }
    }

    private IEnumerator ResetFill()
    {
        isFilling = true;
        float elapsedTime = 0f;

        while (elapsedTime < resetFillTime)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(elapsedTime / resetFillTime) * 100f;
            resetFiller.style.width = new StyleLength(new Length(fillAmount, LengthUnit.Percent));
            yield return null;
        }

        resetFiller.style.width = new StyleLength(new Length(100f, LengthUnit.Percent));
        TriggerResetAction();
        CancelReset();
    }

    private void CancelReset()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }

        resetFiller.style.width = new StyleLength(new Length(0, LengthUnit.Percent));
        isFilling = false;
    }

    private void TriggerResetAction()
    {
        LevelManager.Instance.ResetToCheckPoint(false); // Your logic here
    }
}