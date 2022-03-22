using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HeadsetSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 headsetPos;
    public Quaternion headsetRotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // Rキーで位置トラッキングをリセットする
        if (Input.GetKeyDown(KeyCode.R))
        {
            OVRManager.display.RecenterPose();
            Debug.Log("Reset");
        }
        GetRotation();
    }

    public void GetRotation()
    {
        headsetPos = InputTracking.GetLocalPosition(XRNode.CenterEye);
        headsetRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);
        Debug.Log(headsetPos);
        Debug.Log(headsetRotation);
    }
}
