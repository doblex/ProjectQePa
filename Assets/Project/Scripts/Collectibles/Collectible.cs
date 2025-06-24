using UnityEngine;
using utilities.Controllers;

public class Collectible : MonoBehaviourWithAudio
{
    [SerializeField] bool isHealing;
    [SerializeField, ShowIf("isHealing", true)] private int healingAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snail"))
        {
            if (isHealing)
            {
                HealthController hc = other.GetComponent<HealthController>();
                hc.RestoreHealth(healingAmount);
            }
            ScoreManager.Instance.RegisterCollected();
            gameObject.SetActive(false);
        }
    }
}
