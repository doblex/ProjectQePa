using UnityEngine;
using utilities.Controllers;

public class DamageOnTriggerEnter : MonoBehaviour
{
    // Damages the snail once if entered

    [SerializeField] private int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snail"))
        {
            HealthController hc = other.GetComponent<HealthController>();
            hc.DoDamage(damage);
        }
    }
}
