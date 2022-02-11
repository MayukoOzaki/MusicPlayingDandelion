using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathDetection : MonoBehaviour
{
    public DandelionManagement dandelionManagement;

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            dandelionManagement.isBlown(-1.0f, 1.0f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dandelionManagement.isBlown( 0.0f, 1.0f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dandelionManagement.isBlown( 1.0f, 1.0f);
        }
        else
        {
            Vector3 camPos = GameObject.Find("MainCamera").transform.position;
            float z = camPos.z;
            dandelionManagement.CheckPassingDandelion(z);
        }
    }
}
