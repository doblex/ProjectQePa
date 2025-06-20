using UnityEngine;

public class StarGravitySubject : GravitySubject
{
    // Star specific behavior

    protected override void FixedUpdate()
    {
        // Completely stop if no gravity is present
        if(onGravityPulls == null || onGravityPulls.GetInvocationList().Length == 0)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            base.FixedUpdate();
        }
    }
}
