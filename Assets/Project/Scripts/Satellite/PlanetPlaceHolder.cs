using UnityEngine;

[ExecuteInEditMode]
public class PlanetPlaceHolder : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<GravityPullToggle>().PullRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
}
