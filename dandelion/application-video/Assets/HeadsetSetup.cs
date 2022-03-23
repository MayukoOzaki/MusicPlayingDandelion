using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HeadsetSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 HMDPosition;
    public Quaternion HMDRotationQ;
    public Vector3 HMDRotation;
    public Vector3 camforward;

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
        HMDPosition = InputTracking.GetLocalPosition(XRNode.CenterEye);
        HMDRotationQ = InputTracking.GetLocalRotation(XRNode.CenterEye);
        HMDRotation = HMDRotationQ.eulerAngles;
        //Debug.Log(HMDPosition);
        //Debug.Log(HMDRotationQ);
        //Debug.Log(HMDRotation);
        camforward = GameObject.FindWithTag("MainCamera").transform.forward;
        Debug.Log(camforward);

    }
}
