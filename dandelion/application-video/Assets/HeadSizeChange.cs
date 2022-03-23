using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSizeChange : MonoBehaviour
{
    public Vector3 defaultScale;
    public Vector3 localScale;
    public Vector3 changeScale;
    // Start is called before the first frame update
    void Start()
    {
        defaultScale = transform.lossyScale;
        localScale=transform.localScale;

        Vector3 lossScale = transform.lossyScale;


        transform.localScale = new Vector3(localScale.x / lossScale.x * defaultScale.x,localScale.y / lossScale.y * defaultScale.y,localScale.z / lossScale.z * defaultScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHeadSize(float velocity)
    {
        //Transform tf = gameObject.GetComponent<Transform>();

        Vector3 headsize = gameObject.transform.localScale;
        //ç≈è¨56(0.7), ç≈ëÂ73(1.1)
        //y=velocity*0.02-0.59
        float vx = headsize.x;
        vx = vx*velocity*0.02f - 0.59f;
        headsize.x = vx;
        float vy = headsize.y;
        vy = vy * velocity * 0.02f - 0.59f;
        headsize.y = vy;
        float vz = headsize.z;
        vz = vz * velocity * 0.02f - 0.59f;
        headsize.z = vz;

        gameObject.transform.localScale = headsize;
    }
}
