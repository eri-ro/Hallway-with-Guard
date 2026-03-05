using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.1f, 0f, 0f);
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        //add player collision set to trigger the guard's chase (for later)
    }

}
