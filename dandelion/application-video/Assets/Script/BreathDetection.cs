using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BreathDetection : MonoBehaviour
{
    public DandelionManagement dandelionManagement;
    public NotePlayer noteplayer;

    float m_volumeRate; // 音量(0-1)

    private AudioSource aud;
    private readonly float[] spectrum = new float[256];

    void Start()
    {
        aud = GetComponent<AudioSource>();
        if ((aud != null) && (Microphone.devices.Length > 0)) // オーディオソースとマイクがある
        {
            string devName = Microphone.devices[0]; // 0番目のマイクを使用
            //Debug.Log("Input Device: " + devName);
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // 最大最小サンプリング数を得る
            //Debug.Log("Sampling Rates: " + minFreq.ToString() + " / " + maxFreq.ToString());
            aud.clip = Microphone.Start(devName, true, 2, minFreq); // 音の大きさを取るだけなので最小サンプリング
            aud.Play(); //マイクをオーディオソースとして実行(Play)開始
        }
    }

    // Update is called once per frame
    void Update()
    {
        aud.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        // Call DandelionManagement::isBlown based on spectrum data

        //string[] array = new string[256];
        float eL = 0;
        float eH = 0;
        float r = 0;

        for (int i = 0; i < 256; i++)
        {
            float fHz = i;
            float fNum = spectrum[i];
            string hz = fHz.ToString();//周波数
            string num = fNum.ToString();//数
            //Debug.Log(hz + " " + num);
            if (i >= 1 && i < 17)
            {
                float sqnum = Mathf.Pow(fNum, 2.0f);
                eL += sqnum;
            }
            if (i >= 17 && i < 33)
            {
                float sqnum = Mathf.Pow(fNum, 2);
                eH += sqnum;
            }
            
            //array[i] = num;
        }

        //Debug.Log(eH + " " + eL);
        r = eH / eL;
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log(r);
        }
            
        if (r >= 1.993)
        {
            Debug.Log("息を吹いている");

        }
        else
        {
            //Debug.Log("息が吹かれていない");
        }


    }

    void KeyDetection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            dandelionManagement.isBlown(-1.0f, 1.0f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dandelionManagement.isBlown( 0.0f, 1.0f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dandelionManagement.isBlown( 1.0f, 1.0f);
        }
        /* // ここで音を止めるコードを書くと、実際のマイクからの制御に悪影響がある
        else
        {
            Vector3 camPos = GameObject.FindWithTag("MainCamera").transform.position;
            float z = camPos.z;
            dandelionManagement.CheckPassingDandelion(z);
            noteplayer.NoteOff(noteplayer.nowPitch);
        }
        */
    }
}
