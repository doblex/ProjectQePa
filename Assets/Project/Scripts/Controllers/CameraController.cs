using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 0.125f;

    Transform target;

    bool canMove = false;

    public void PanCamera(Transform _target)
    {
        target = _target;

        canMove = true;
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (canMove && target != null)
        {
            Vector3 desiredPosition = target.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void SetCameraPosition(Vector3 pos)
    { 
        transform.position = pos;
    }
}
