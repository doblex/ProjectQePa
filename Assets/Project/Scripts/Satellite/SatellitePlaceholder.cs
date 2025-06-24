using UnityEngine;

[ExecuteInEditMode]
public class SatellitePlaceholder : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<Satellite>().Radius);
    }
}
