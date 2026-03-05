using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    //Temlate for managing lasers
    //declarations for each laser should look like below
    //public GameObject laser1;
    //public bool laser1Off;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OnOff", 0.1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnOff()
    {
        //template for the switching of lasers on and off should look identical to below with laser1.SetActive(true/false) and laser1Off renamed to the same names as declared above
        //each laser should have it's own if else if statement like the one below
        if (laser1Off == false)
        {
            //Debug.Log("Disabling Laser1");
            laser1.SetActive(false);
            laser1Off = true;
        }
        else if (laser1Off == true)
        {
            //Debug.Log("Enabling Laser1");
            laser1.SetActive(true);
            laser1Off = false;
        }

    }


}
