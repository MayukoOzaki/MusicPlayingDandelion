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
        localScale = transform.localScale;//0.1, 0.5, 0.1

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
        float num = (float)number;

        //transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
        //transform.position = new Vector3(0f, 0f, 0f);

        defaultScale = transform.lossyScale;
        localScale = transform.localScale;
        Vector3 lossScale = transform.lossyScale;

        float dey= defaultScale.y + (0.01f * num);
        defaultScale.y = dey;

        transform.localScale = new Vector3(localScale.x / lossScale.x * defaultScale.x, localScale.y / lossScale.y * defaultScale.y, localScale.z / lossScale.z * defaultScale.z);

        defaultScale = transform.lossyScale;
        localScale = transform.localScale;


        Vector3 stemPos = transform.position;
        float addPosy = (defaultScale.y - 0.105f);
        stemPos.y = stemPos.y - addPosy;
        transform.position = stemPos;

    }
}
