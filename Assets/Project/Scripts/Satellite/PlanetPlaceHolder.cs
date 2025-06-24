using UnityEngine;

[ExecuteInEditMode]
public class PlanetPlaceHolder : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<GravityPullToggle>().PullRadius);
    }
}
