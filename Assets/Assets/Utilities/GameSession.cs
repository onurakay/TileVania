using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public event Action OnLivesChanged;

    [SerializeField] private int playerLives = 3;
    public int coinBalance = 0;

    public int PlayerLives => playerLives;

    [SerializeField] private TextMeshProUGUI coinAmountText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Add this method
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshUIElements();  // Reinitialize UI elements when a new scene is loaded
    }

    private void RefreshUIElements()
    {
        coinAmountText = GameObject.Find("Coin Amount Text")?.GetComponent<TextMeshProUGUI>();
        UpdateCoinText();
    }

    public void HandlePlayerDeath()
    {
        if (playerLives > 1)
        {
            StartCoroutine(ResetLevelWithDelay());
        }
        else
        {
            Debug.LogError("No more lives!");
            ResetGameSession();
        }
    }

    private IEnumerator ResetLevelWithDelay()
    {
        yield return new WaitForSeconds(3f);
        ResetLevel();
    }

    private void ResetLevel()
    {
        playerLives--;
        OnLivesChanged?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        coinBalance += amount;
        Debug.Log(coinBalance);
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinAmountText != null)
        {
            coinAmountText.text = $"x{coinBalance}";
        }
    }
}

