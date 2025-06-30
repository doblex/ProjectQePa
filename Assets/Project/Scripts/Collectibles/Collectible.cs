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
            OnPlayAudio?.Invoke(audioChannels);
            ScoreManager.Instance.RegisterCollected();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
