using UnityEngine;

public class ObjectMotionManager : MonoBehaviour
{
    public static ObjectMotionManager Instance;

    [Header("Object Properties")]
    [SerializeField] Material selectedMaterial;
    [SerializeField] float movementSpeed = 5f;

    [Header("Spline Visualizer Properties")]
    [Range(2, 100), SerializeField] int resolution = 32;
    [Range(0, 1), SerializeField] float width = 0.1f;

    public int Resolution { get => resolution;}
    public float Width { get => width;}
    public float MovementSpeed { get => movementSpeed;}

    private void Awake()
    {
        SetInstance();
    }

    private void SetInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
