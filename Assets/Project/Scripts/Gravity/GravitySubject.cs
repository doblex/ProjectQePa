using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySubject : MonoBehaviour
{
    public delegate Vector3 OnGravityPull(Vector3 position, float mass);

    public OnGravityPull onGravityPulls = null;

    public SkinnedMeshRenderer skinnedMesh = null;

    protected Rigidbody rb;
    protected float mass;

    bool isOutside = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
    }

    private void Update()
    {
        AnimateSnail();
    }

    protected virtual void FixedUpdate()
    {
        rb.AddForce(ComputeTotalForce());
    }

    private void AnimateSnail()
    {
        if (skinnedMesh != null)
        {
            bool isStationary = rb.linearVelocity.magnitude < 0.01f;
            if (isStationary != isOutside) // state changed
            {
                StopCoroutine("BlendShapeAnimate");
                StartCoroutine(BlendShapeAnimate(!isOutside));
                isOutside = isStationary;
            }
        }
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

    IEnumerator BlendShapeAnimate(bool backwards = false)
    {
        float value = backwards ? 100f : 0f;
        float maxvalue = backwards ? 0f : 100f;

        if (backwards)
        {
            while (value > maxvalue)
            {
                skinnedMesh.SetBlendShapeWeight(0, value);
                value -= 1 * 5f;
                yield return null;
            }
        }
        else
        {
            while (value < maxvalue)
            {
                skinnedMesh.SetBlendShapeWeight(0, value);
                value += 1 * 5f;
                yield return null;
            }
        }

        
    }
}
