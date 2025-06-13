using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySubject : MonoBehaviour
{
    public delegate Vector3 OnGravityPull(Vector3 position, float mass);

    public OnGravityPull onGravityPulls;

    private Rigidbody rb;
    private float mass;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
    }

    private void FixedUpdate()
    {
        rb.AddForce(ComputeTotalForce());
    }

    private Vector3 ComputeTotalForce()
    {
        Vector3 totalForce = Vector3.zero;
        if (onGravityPulls != null)
        {
            Delegate[] pulls = onGravityPulls.GetInvocationList();

            foreach (OnGravityPull pull in pulls)
            {
                totalForce += pull.Invoke(transform.position, mass);
            }
        }
        return totalForce;
    }
}
