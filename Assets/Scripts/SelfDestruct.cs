using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destructionTime;
    void Start()
    {
        Destroy(this.gameObject, destructionTime);
    }


}
