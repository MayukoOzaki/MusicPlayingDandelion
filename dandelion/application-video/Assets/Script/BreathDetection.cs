using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathDetection : MonoBehaviour
{
    public bool breathflag = false;
    //DandelionManegimen BreScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyDetection();
    }
    void FixedUpdate()
    {
        
    }

    void KeyDetection()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            breathflag = true;
            Debug.Log("true");
        }
        else
        {
            breathflag = false;
            //Debug.Log("false");
        }
    }
}
