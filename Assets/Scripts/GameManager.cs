using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public float slowMotionFactor = 10f;
    public float slowMotionDuration = 1f;
    public float autoRestartDelay = 3f;

    [Header("State Flags")]
    public bool isGameOver;
    public bool areTilesFrozen;
    public bool hasGameEnded;

    private int _currentScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize default state
        isGameOver = true;
        areTilesFrozen = true;
        hasGameEnded = false;

        // Ensure we have a high score saved
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }

    private void Update()
    {
        // Start game on first click if we are in the waiting state
        if (Input.GetMouseButtonDown(0))
        {
            if (isGameOver && areTilesFrozen && !hasGameEnded)
            {
                BeginGame();
            }
        }
    }

    public void BeginGame()
    {
        isGameOver = false;
        areTilesFrozen = false;
        _currentScore = 0;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetScoreDisplay(_currentScore);
        }
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetScoreDisplay(_currentScore);
        }
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideScore();
        }

        SaveHighScore();
        StartCoroutine(DoSlowMotionEffect());
    }

    private void SaveHighScore()
    {
        int currentHigh = PlayerPrefs.GetInt("HighScore", 0);
        if (_currentScore > currentHigh)
        {
            PlayerPrefs.SetInt("HighScore", _currentScore);
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator DoSlowMotionEffect()
    {
        // Slow down time
        Time.timeScale = 1f / slowMotionFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowMotionFactor;

        // Wait for a bit in real-time
        yield return new WaitForSeconds(slowMotionDuration / slowMotionFactor);

        // Reset time
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowMotionFactor;

        // Freeze everything and start restart timer
        areTilesFrozen = true;
        hasGameEnded = true;
        
        StartCoroutine(RunRestartTimer());
    }
    
    private IEnumerator RunRestartTimer()
    {
        float timer = autoRestartDelay;
        
        while (timer > 0)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateRestartText(Mathf.CeilToInt(timer));
            }
            
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        
        RestartLevel();
    }
}