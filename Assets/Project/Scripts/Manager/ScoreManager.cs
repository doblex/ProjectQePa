using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private int collectedAmount = 0;
    public int GetCollected {  get { return collectedAmount; } }

    [SerializeField, ReadOnly] private int collectibleTotal;

    public int GetTotalCollectibles { get { return collectibleTotal; } }

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        collectibleTotal = FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
    }

    public void RegisterCollected()
    {
        collectedAmount++;
    }

    public void SetScore(int score)
    {
        collectedAmount = score;
    }
    public void ResetScore()
    {
        collectedAmount = 0;
    }
}
