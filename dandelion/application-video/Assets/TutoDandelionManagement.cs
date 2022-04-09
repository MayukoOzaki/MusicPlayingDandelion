using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class TutoDandelionManagement : MonoBehaviour
{
    //public List<GameObject> ObjectList = new List<GameObject>();
    public GameObject dandelion;

    public HeadsetSetup headsetSetup;
    public float BlownWidth = 15f;
    public int nowNotenumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isBlown(float Strength) //Posx:吹いた位置　Strength:吹いた強さ
    {
        /*
        if (ObjectList.Count == 0)
            return;

        GameObject dandelion = ObjectList[0];
        */

        uint velocity = (uint)Strength;
        int notenum = dandelion.GetComponent<NoteInfo>().noteNumber;
        
        Vector3 camPos = headsetSetup.camPos;
        float camPosx = camPos.x;

        

        if (velocity>0)
        {
            if (JudgeAngle(dandelion))
            {
                //Debug.Log("角度OK");
                //Debug.Log("距離："+Vector3.Distance(dandelion.transform.position, camPos));
                float distance = Vector3.Distance(dandelion.transform.position, camPos);
                //Debug.Log("距離：" +distance+"制限"+BlownWidth);
                if (distance < BlownWidth)
                {

                    Vector3 dir = dandelion.transform.position - camPos;
                    dandelion.GetComponent<TutoDandelionController>().Blow(dir);
                    nowNotenumber = notenum;
                    
                }
            }
            
        }
    }

    public bool JudgeAngle(GameObject dandelion)
    {

        Vector3 camPos = headsetSetup.camPos;
        Vector3 camforward = headsetSetup.camforward;//カメラの正面

        Vector3 dandelionDir = dandelion.transform.position - camPos;
        dandelionDir = dandelionDir.normalized;//カメラからタンポポ

        float diff = Vector3.Angle(camforward, dandelionDir);
       

        // Debug.Log(diff);

        if (diff <= 45.0f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public int JudgeDistance(GameObject dandelion)
    {
        Vector3 posDandelion = dandelion.transform.position;
        Vector3 camPos = headsetSetup.camPos;
        float dis = Vector3.Distance(posDandelion, camPos);
        //Debug.Log("距離 : " + dis);
        //short 0, middle1.25, long 2.5

        int value = (int)(-dis * 22.4 + 28);
        return value;
    }
}
