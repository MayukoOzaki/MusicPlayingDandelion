using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        Vector3 camerapos = mainCamera.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
