using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private Vector3 startingPos;
    public float amountOfParallax;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        Vector3 position = GameObject.FindWithTag("Snail").transform.position  /*Camera.main.transform.position*/;
        float distanceX = position.x * amountOfParallax;
        float distanceZ = position.z * amountOfParallax;

        transform.position = new Vector3(startingPos.x + distanceX, transform.position.y, startingPos.z + distanceZ);
    }
}
