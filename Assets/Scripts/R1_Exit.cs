using UnityEngine;

public class R1_Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.hasArtifact)
        {
            GameManager.Instance.WinGame();
        }
    }
}
