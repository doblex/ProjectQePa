using UnityEngine;

[ExecuteInEditMode]
public class SatellitePlaceholder : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<Satellite>().Radius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
}
