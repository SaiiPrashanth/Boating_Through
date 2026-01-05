using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Text References")]
    public TMP_Text scoreText;
    public TMP_Text restartTimerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Reset UI
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
        
        if (restartTimerText != null)
        {
            restartTimerText.gameObject.SetActive(false);
        }
    }

    public void SetScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = score.ToString();
        }
    }

    public void HideScore()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
    }
    
    public void UpdateRestartText(int seconds)
    {
        if (restartTimerText != null)
        {
            restartTimerText.gameObject.SetActive(true);
            restartTimerText.text = "Restarting in " + seconds + "...";
        }
    }
}