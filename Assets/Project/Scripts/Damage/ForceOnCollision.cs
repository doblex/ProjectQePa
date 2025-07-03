using UnityEngine;

public class ForceOnCollision : MonoBehaviourWithAudio
{
    // Reflects the snail when a colision happens
    [SerializeField] private float strength = 1;
    private void OnCollisionEnter(Collision collision)
    {
        // reflect the snail
        if (collision.collider.CompareTag("Snail"))
        {
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            Vector3 normal = collision.GetContact(0).normal;

            rb.AddForce(-normal * strength, ForceMode.Impulse);

            OnPlayAudio?.Invoke(audioChannels);
        }
    }
}
