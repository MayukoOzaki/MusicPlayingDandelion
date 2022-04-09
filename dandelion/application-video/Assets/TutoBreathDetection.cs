using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TutoBreathDetection : MonoBehaviour
{
    private AudioSource aud;
    private readonly float[] spectrum = new float[256];

    public float noSoundThreshold;
    public float breathDetectionThreshold;

    public TutoDandelionManagement tutoDandelionManagement;


    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        if ((aud != null) && (Microphone.devices.Length > 0)) // オーディオソースとマイクがある
        {
            string devName = Microphone.devices[0]; // 0番目のマイクを使用
            Debug.Log("Input Device: " + devName);
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // 最大最小サンプリング数を得る

            Debug.Log("Sampling Rates: " + minFreq.ToString() + " / " + maxFreq.ToString());

            aud.clip = Microphone.Start(devName, true, 3599, 48000);//16000);//48000);
            aud.Play(); //マイクをオーディオソースとして実行(Play)開始

            //CreateFile1();
            Debug.Log("マイク設定完了");
        }

    }

    // Update is called once per frame
    void Update()
    {
        aud.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        // Call DandelionManagement::isBlown based on spectrum data
        Debug.Log("spectrumを取得");

        float eL = 0;
        float eH = 0;
        float r;

        for (int i = 1; i <= 16; i++)
        {
            //Debug.Log(spectrum[i]);
            eL += spectrum[i] * spectrum[i];
        }
            //eL += spectrum[i] * spectrum[i];

        for (int i = 17; i <= 32; i++)
            eH += spectrum[i] * spectrum[i];

        r = eH / eL;

        float strength = 0;
        if (eH > noSoundThreshold)//min breath check
        {
            //Debug.Log(eH + "/" + eL + "/" + r);
            if (r > breathDetectionThreshold)//on vice check
            {
                //Debug.Log("BREATH");
                //Debug.Log(eH + "/" + eL + "/" + r);
                //strength=15.41f*Mathf.Log10(eH) +173.59f; //65 
                strength = 14.17f * Mathf.Log10(eH) + 169.84f; //70
                //strength = 19.14f*Mathf.Log10(eH) +184.86f;//50

                if (strength > 127.0f)
                {
                    strength = 127.0f;
                }
                else if (strength < 0)
                {
                    strength = 0;
                }
            }
            else
            {
                //Debug.Log("1止めた11111111111111111111");
            }
        }
        else
        {
            //Debug.Log("2止めた222222222222222222222");
        }
        Debug.Log("息の強さ:"+strength);
        tutoDandelionManagement.isBlown(strength);


        if (Input.GetKey(KeyCode.P))
        {
            //dandelionManagement.isBlown(1.0f, 127f);
        }
        if (Input.GetKey(KeyCode.O))
        {
            //dandelionManagement.isBlown(1.0f, 70f);  //63
        }
        if (Input.GetKey(KeyCode.I))
        {
            //dandelionManagement.isBlown(1.0f, 0);
        }
    }

    
}
