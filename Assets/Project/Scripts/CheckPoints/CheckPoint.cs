using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public delegate void CheckPointReachedEventHandler(int id, Transform newCameraPosition,bool changeCameraSize, float cameraSize = 0, bool isSpawned = false);

    public event CheckPointReachedEventHandler CheckPointReached;

    [ReadOnly][SerializeField] CheckPointType type;
    [ReadOnly][SerializeField] private int id;

    [SerializeField] bool changeCameraSize = false;
    [ShowIf("changeCameraSize", true)][SerializeField] float cameraSize = 5f;

    [SerializeField] List<SelectionComponent> componentToDeactivate; 
    [SerializeField] Transform spawnpoint;
    [SerializeField] Transform CamPosition;

    [SerializeField] GameObject flag;

    public int Id { get => id; set => id = value; }
    public CheckPointType Type { get => type ; set => type = value; }
    public Transform Spawnpoint { get => spawnpoint; }
    public GameObject Flag { get => flag; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snail"))
        {
            ReachCheckPoint(other.gameObject);
        }
    }

    public void SpawnOnCheckPoint(GameObject snail)
    {
        ReachCheckPoint(snail);
        //snail.GetComponent<CameraBorderCheck>().Active = true;
    }

    private void ReachCheckPoint(GameObject snail, bool isSpawned = false)
    {
        snail.transform.position = spawnpoint.position;
        GetComponent<Collider>().enabled = false;

        if (flag != null)
        { 
            flag.SetActive(true);
        }

        DeactivateComponets();

        //Reset the velocity of the snail
        snail.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        CheckPointReached?.Invoke(id, CamPosition, changeCameraSize, cameraSize, isSpawned);
    }

    private void DeactivateComponets()
    {
        foreach (SelectionComponent component in componentToDeactivate)
        {
            component.CallSelectionDisable(false);
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
