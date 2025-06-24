using UnityEngine;
using utilities.Controllers;

public class Collectible : MonoBehaviourWithAudio
{
    [SerializeField] bool isHealing;
    [SerializeField, ShowIf("isHealing", true)] private int healingAmount;

    private MeshRenderer mesh;
    private Collider colliderRef;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        colliderRef = GetComponent<Collider>();
    }

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
            mesh.enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
