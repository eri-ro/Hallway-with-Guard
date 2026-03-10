using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnToMM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        //replace "SampleScene" with the name of the room
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}
