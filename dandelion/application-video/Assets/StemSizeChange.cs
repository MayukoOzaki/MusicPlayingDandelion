using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemSizeChange : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 defaultScale;
    public Vector3 localScale;

    void Start()
    {
        /*
        defaultScale = new Vector3(0.021f, 0.205f, 0.021f);// transform.lossyScale; //0.021, 0.105, 0.021 ワールド
        localScale = transform.localScale;//0.1, 0.1, 0.1

        Vector3 lossScale = transform.lossyScale;// 0.021, 0.105, 0.021 ワールド
        //transform.localScale = new Vector3(localScale.x / lossScale.x * defaultScale.x, localScale.y / lossScale.y * defaultScale.y, localScale.z / lossScale.z * defaultScale.z);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeStemSize(int number)
    {
        //Transform tf = gameObject.GetComponent<Transform>();
        float num = (float)number;

        defaultScale = transform.lossyScale;
        localScale = transform.localScale;
        Vector3 lossScale = transform.lossyScale;

        float dey = 0f;
        dey= defaultScale.y + (0.01f * num);
        defaultScale.y = dey;

        //Debug.Log(dex+"/"+dey+"/"+dez);

        transform.localScale = new Vector3(localScale.x / lossScale.x * defaultScale.x, localScale.y / lossScale.y * defaultScale.y, localScale.z / lossScale.z * defaultScale.z);

        defaultScale = transform.lossyScale;
        localScale = transform.localScale;



    }
}
