using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySubject : MonoBehaviour
{
    public delegate Vector3 OnGravityPull(Vector3 position, float mass);

    public OnGravityPull onGravityPulls = null;

    protected Rigidbody rb;
    protected float mass;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
    }

    protected virtual void FixedUpdate()
    {
        rb.AddForce(ComputeTotalForce());
    }

    /// <summary>
    /// Computes the total force on the affected body.
    /// </summary>
    protected Vector3 ComputeTotalForce()
    {
        Vector3 totalForce = Vector3.zero;
        if (onGravityPulls != null)
        {
            Delegate[] pulls = onGravityPulls.GetInvocationList();
            Debug.Log("Gravity pulls number: " + pulls.Length.ToString());

            foreach (OnGravityPull pull in pulls)
            {
                totalForce += pull.Invoke(transform.position, mass);
            }
        }
        return totalForce;
    }
}
