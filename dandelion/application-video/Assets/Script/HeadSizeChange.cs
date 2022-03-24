using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSizeChange : MonoBehaviour
{
    public Vector3 defaultScale;
    public Vector3 localScale;

    //public Vector3 changeScale;
    // Start is called before the first frame update
    void Start()
    {
        /*
        defaultScale = new Vector3(0.21f,0.21f,0.21f); //transform.lossyScale;//0.21, 0.21, 0.21 ワールド
        localScale=transform.localScale;//1, 1, 1
        */

        /*
        defaultScale =new Vector3(0.21f,0.21f,0.21f);
        Vector3 lossScale = transform.lossyScale;// 0.21, 0.21, 0.21 ワールド
        localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x / lossScale.x * defaultScale.x,localScale.y / lossScale.y * defaultScale.y,localScale.z / lossScale.z * defaultScale.z);
        */

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHeadSize(float velocity)
    {
        //Transform tf = gameObject.GetComponent<Transform>();
        //transform.localScale= new Vector3(1.0f, 1.01f, 1.0f);

        defaultScale = transform.lossyScale;
        localScale = transform.localScale;
        Vector3 lossScale = transform.lossyScale;

        float dex = velocity * 0.0059f - 0.18f;
        defaultScale.x = dex;

        float dey = velocity * 0.0059f - 0.18f;
        defaultScale.y = dey;

        float dez = velocity * 0.0059f - 0.18f;
        defaultScale.z = dez;

        //Debug.Log(dex+"/"+dey+"/"+dez);

        transform.localScale = new Vector3(localScale.x / lossScale.x * defaultScale.x, localScale.y / lossScale.y * defaultScale.y, localScale.z / lossScale.z * defaultScale.z);

        defaultScale = transform.lossyScale;
        localScale = transform.localScale;



    }
}
