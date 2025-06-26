using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private int collectedAmount = 0;
    public int GetCollected {  get { return collectedAmount; } }

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
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
