using UnityEngine;
using utilities.Controllers;

public class DamageAndReflectOnCollision : MonoBehaviourWithAudio
{
    // Damages the snail and reflects its direction when a colision happens

    [SerializeField] private int damage = 1;
    [SerializeField] private float reflectStrenght = 1;

    private void OnCollisionEnter(Collision collision)
    {
        // reflect and deal damage to the snail
        if (collision.collider.CompareTag("Snail"))
        {
            HealthController hc = collision.collider.GetComponent<HealthController>();
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            hc.DoDamage(damage);
            rb.linearVelocity = reflectStrenght * Vector3.Reflect(rb.linearVelocity, collision.GetContact(0).normal);
            OnPlayAudio?.Invoke(audioChannels);
        }
    }
}
