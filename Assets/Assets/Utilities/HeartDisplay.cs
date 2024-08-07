using UnityEngine;

public class HeartDisplay : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartsContainer;

    private void Start()
    {
        UpdateHeartDisplay();
    }

    public void UpdateHeartDisplay()
    {
        if (heartsContainer == null || heartPrefab == null)
        {
            Debug.LogError("Hearts container or heart prefab not set!");
            return;
        }

        // Clear existing hearts
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        // Get the number of lives from the GameSession
        GameSession gameSession = GameSession.Instance;
        if (gameSession == null)
        {
            Debug.LogError("GameSession instance is not found!");
            return;
        }

        int currentLives = gameSession.PlayerLives;

        // Instantiate heart images based on current lives
        for (int i = 0; i < currentLives; i++)
        {
            Instantiate(heartPrefab, heartsContainer);
        }
    }

    private void OnEnable()
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.OnLivesChanged += UpdateHeartDisplay;
        }
    }

    private void OnDisable()
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.OnLivesChanged -= UpdateHeartDisplay;
        }
    }
}
