using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TMP_Text gameMessageText;
    private float messageTimer;
    private bool messageActive;

    public bool hasArtifact;
    public bool gameOver;

    void Awake()
    {
        Instance = this;    
    }

    void Start()
    {
        gameMessageText.text = "";
    }

    void Update()
    {
        if (messageActive)
        {
            messageTimer -= Time.deltaTime;

            if (messageTimer <= 0f)
            {
                gameMessageText.text = "";
                messageActive = false;
            }
        }
    }

    public void OnArtifactCollected()
    {
        hasArtifact = true;
        ShowMessage("Crown collected! Escape the way you came in.");
    }

    public void WinGame()
    {
        gameOver = true;
        ShowMessage("You escaped with the crown!", 2f);

        // Restart game
        Invoke(nameof(RestartScene), 2f);
    }

    public void LoseGame()
    {
        gameOver = true;
        ShowMessage("You've been CAUGHT!");

        // Restart game
        Invoke(nameof(RestartScene), 2f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        gameMessageText.text = message;
        messageTimer = duration;
        messageActive = true;
    }
}