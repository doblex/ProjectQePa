using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public delegate void CheckPointReachedEventHandler(int id, Transform newCameraPosition,bool changeCameraSize, float cameraSize = 0);

    public event CheckPointReachedEventHandler CheckPointReached;

    [ReadOnly][SerializeField] CheckPointType type;
    [ReadOnly][SerializeField] private int id;

    [SerializeField] bool changeCameraSize = false;
    [ShowIf("changeCameraSize", true)][SerializeField] float cameraSize = 5f;

    [SerializeField] Transform spawnpoint;
    [SerializeField] Transform CamPosition;

    public int Id { get => id; set => id = value; }
    public CheckPointType Type { get => type ; set => type = value; }
    public Transform Spawnpoint { get => spawnpoint; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snail"))
        {
            other.transform.position = spawnpoint.position;
            GetComponent<Collider>().enabled = false;
            CheckPointReached?.Invoke(id, CamPosition, changeCameraSize, cameraSize);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(spawnpoint.position, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CamPosition.position, 0.1f);
    }
}
