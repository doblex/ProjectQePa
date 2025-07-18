using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(PlanetPlaceHolder))]
public class GravityPullToggle : MonoBehaviour
{
    [Header("Gravity settings")]
    [Tooltip("Da trattare come massa del pianeta")]
    [SerializeField] private float pullIntensity; 
    [SerializeField] private float pullRadius;
    [SerializeField] private bool pullActive;
    [Tooltip("Costante di Newton, aumentare se non sufficiente")]
    [SerializeField] private float G = 6.674e-11f;

    private CapsuleCollider pullCollider;
    private SelectionComponent selectionComponent;
    private HashSet<Collider> affectedColliders = new HashSet<Collider>();

    public float PullRadius { get => pullRadius;}

    public void SetPullActive(bool _pullActive) { this.pullActive = _pullActive; }

    private void Awake()
    {
        // Add selection event
        selectionComponent = GetComponentInChildren<SelectionComponent>();
        selectionComponent.OnSelectionChanged += SetPullActive;
    }

    private void Start()
    {
        // apply radius to trigger coolider
        pullCollider = GetComponent<CapsuleCollider>();
        pullCollider.radius = pullRadius;
    }

    private void Update()
    {
        // toggle gravity pull
        pullCollider.enabled = pullActive;
        if (!pullActive) UnsubscribeAll();
    }

    /// <summary>
    /// subscribes a GravitySubject to the gravitational pull of the body.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GravitySubject>() != null && !affectedColliders.Contains(other))
        {
            affectedColliders.Add(other);
            GravitySubject gs = other.GetComponent<GravitySubject>();
            gs.onGravityPulls += ComputeAttractionForce;
        }
        Debug.Log("Trigger entered!");
    }

    /// <summary>
    /// Disables movement once the subscribed GravitySubject reaches the center of the body.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<GravitySubject>() != null)
        {
            GravitySubject gs = other.GetComponent<GravitySubject>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // Stop acting if attracted body is already in the center (ignore Y component) and there is no other influence
            if (gs.onGravityPulls.GetInvocationList().Length == 1)
            {
                Debug.Log("Single Planet pull");
                if (Mathf.Abs(other.transform.position.x - transform.position.x) < 0.1f && 
                    Mathf.Abs(other.transform.position.z - transform.position.z) < 0.1f )
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    //selectionComponent.CallSelectionDisable();
                }
            }
        }
    }

    /// <summary>
    /// unsubscribes a GravitySubject from the gravitational pull of the body.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GravitySubject>() != null && affectedColliders.Contains(other))
        {
            affectedColliders.Remove(other);
            GravitySubject gs = other.GetComponent<GravitySubject>();
            gs.onGravityPulls -= ComputeAttractionForce;
        }
        Debug.Log("Trigger exited!");
    }

    /// <summary>
    /// Manually unsubscribes all GravitySubjects from the gravitational pull of the body when turned off.
    /// </summary>
    private void UnsubscribeAll()
    {
        foreach(Collider other in affectedColliders)
        {
            GravitySubject gs = other.GetComponent<GravitySubject>();
            gs.onGravityPulls -= ComputeAttractionForce;
            Debug.Log("Trigger exited!");
        }
        affectedColliders.Clear();
    }

    /// <summary>
    /// Computes the attraction force between the body and an object of given position and mass using Newton's Law
    /// </summary>
    /// <param name="position">The position of the body being attracted.</param>
    /// <param name="mass">The mass of the body being attracted.</param>
    /// <returns>A vector3 representing the attraction force.</returns>
    public Vector3 ComputeAttractionForce(Vector3 position, float mass)
    {
        float distance = Vector3.Distance(position, transform.position);
        Vector3 direction = Vector3.ProjectOnPlane((transform.position - position), Vector3.up).normalized;

        Vector3 resultingForce = direction * (G * pullIntensity * mass / (distance * distance));
        return resultingForce;
    }

}
