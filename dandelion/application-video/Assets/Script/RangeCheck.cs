using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        Vector3 camerapos = camera.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
