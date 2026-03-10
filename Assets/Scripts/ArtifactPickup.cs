using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    public GameObject pickupEffect;
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(pickupEffect, transform.position, Quaternion.identity);
        GameManager.Instance.OnArtifactCollected();
        Destroy(gameObject);
    }
}