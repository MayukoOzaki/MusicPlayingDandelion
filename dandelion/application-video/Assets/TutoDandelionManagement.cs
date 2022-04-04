using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class TutoDandelionManagement : MonoBehaviour
{
    public List<GameObject> ObjectList = new List<GameObject>();

    public HeadsetSetup headsetSetup;
    public TutoNotePlayer notePlayer;
    public float BlownWidth = 10f;
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
        if (ObjectList.Count == 0)
            return;

        GameObject dandelion = ObjectList[0];


        uint pitch = (uint)dandelion.GetComponent<NoteInfo>().pitch;
        uint velocity = (uint)Strength;
        uint ToneColor = dandelion.GetComponent<NoteInfo>().toneColor;
        int notenum = dandelion.GetComponent<NoteInfo>().noteNumber;
        //Debug.Log(dandelion.GetComponent<NoteInfo>().noteNumber);
        bool nowOn = notePlayer.nowOn;
        Vector3 camPos = headsetSetup.camPos;
        float camPosx = camPos.x;

        

        if (velocity == 0)
        {
            notePlayer.SmoothChangeZero();
        }
        else
        {
            if (JudgeAngle(dandelion))
            {
                //Debug.Log("距離："+Vector3.Distance(dandelion.transform.position, camPos));
                float distance = Vector3.Distance(dandelion.transform.position, camPos);
                if (distance < BlownWidth)
                {
                    //Debug.Log("制限：" + BlownWidth);
                    //Debug.Log("距離：" + Vector3.Distance(dandelion.transform.position, camPos));
                    int value = JudgeDistance(dandelion);
                    velocity = velocity + (uint)value;
                    if (velocity > 127)
                    {
                        velocity = 127;
                    }
                    else if (velocity < 0)
                    {
                        velocity = 0;
                    }
                    //Debug.Log(value + "/" + (int)velocity);
                    notePlayer.SmoothChange(velocity);

                    Vector3 dir = dandelion.transform.position - camPos;
                    dandelion.GetComponent<DandelionController>().Blow(dir);


                    if (notenum > nowNotenumber)
                    {
                        nowNotenumber = notenum;
                        notePlayer.NoteOn(pitch, notePlayer.nowVolume, ToneColor);//音再生
                    }
                }
                else
                {
                    notePlayer.SmoothChangeZero();
                }
            }
            else
            {
                notePlayer.SmoothChangeZero();
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
