using UnityEngine;

public class Asteroid : MonoBehaviourWithAudio
{
    private GravitySubject capturedGs;
    private Rigidbody capturedRb;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Snail"))
        {
            // If no gravitational pull is active, capture the snail and move it with the asteroid
            onPlayAudio?.Invoke(audioChannels);
            capturedGs = collision.collider.GetComponent<GravitySubject>();
            if(capturedGs.onGravityPulls == null || capturedGs.onGravityPulls.GetInvocationList().Length <= 0)
            {
                capturedRb = collision.collider.GetComponent<Rigidbody>();
                capturedRb.linearVelocity = Vector3.zero;
                capturedRb.transform.parent = this.transform;
                Debug.Log("Captured: " + collision.collider.name);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Snail"))
        {
            // Check for gravitational activity 
            if(capturedGs != null && capturedGs.onGravityPulls != null)
            {
                if(capturedGs.onGravityPulls.GetInvocationList().Length > 0)
                {
                    capturedRb.transform.parent = null;
                    Debug.Log("Detected gravity on: " + collision.collider.name);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Snail"))
        {
            // Reset values
            capturedGs = null;
            capturedRb = null;
            Debug.Log("Detached: " + collision.collider.name);
        }
    }
}
