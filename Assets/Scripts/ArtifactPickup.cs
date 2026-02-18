using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.OnArtifactCollected();
        Destroy(gameObject);
    }
}