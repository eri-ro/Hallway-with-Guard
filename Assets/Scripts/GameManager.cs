using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TMP_Text gameMessageText;
    private float messageTimer;
    private bool messageActive;

    private bool gamePaused;
    public GameObject pauseImage;


    public bool hasArtifact;
    public bool gameOver;

    void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        gameMessageText.text = "";
        gamePaused = false;
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
        ShowMessage("Crown Box collected! Escape the way you came in.");
    }

    public void WinGame()
    {
        gameOver = true;
        ShowMessage("You escaped with the Crown Box!", 2f);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("WinScreen");
    }

    public void LoseGame()
    {
        gameOver = true;
        ShowMessage("You've been CAUGHT!");

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            pauseImage.SetActive(true);
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            pauseImage.SetActive(false);
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void ShowMessage(string message, float duration = 2f)
    {
        gameMessageText.text = message;
        messageTimer = duration;
        messageActive = true;
    }
}