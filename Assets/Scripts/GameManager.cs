using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool hasArtifact;
    public bool gameOver;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnArtifactCollected()
    {
        hasArtifact = true;
        Debug.Log("Artifact collected! Return to Room 1.");
    }

    public void WinGame()
    {
        gameOver = true;
        Debug.Log("You escaped with the artifact!");

        // Restart game
        Invoke(nameof(RestartScene), 2f);
    }

    public void LoseGame()
    {
        gameOver = true;
        Debug.Log("Caught!");

        // Restart game
        Invoke(nameof(RestartScene), 2f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}