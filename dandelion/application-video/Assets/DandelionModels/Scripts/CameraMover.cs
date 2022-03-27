using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 camvelocity = new Vector3(0f, 0f, 1.0f);
        GetComponent<Rigidbody>().velocity = camvelocity;//Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StopCamera()
    {
        Vector3 velocity = new Vector3(0f, 0f, 0f);
        GetComponent<Rigidbody>().velocity = velocity;
    }
}
