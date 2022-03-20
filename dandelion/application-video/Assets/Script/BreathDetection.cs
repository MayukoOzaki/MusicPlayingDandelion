using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BreathDetection : MonoBehaviour
{
    public DandelionManagement dandelionManagement;
    public NotePlayer noteplayer;

    public float noSoundThreshold;
    public float breathDetectionThreshold;

    private AudioSource aud;
    private readonly float[] spectrum = new float[256];

    /*
    private int nocou = 0;
    private int brecou = 0;
    private int all = 0;
    private int rcou = 0;
    private int eHcou = 0;
    private bool isEH = false;
    private bool isR = false;
    */

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        aud.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        // Call DandelionManagement::isBlown based on spectrum data

        float eL = 0;
        float eH = 0;
        float r;

        for (int i = 1; i <= 16; i++)
            eL += spectrum[i] * spectrum[i];

        for (int i = 17; i <= 32; i++)
            eH += spectrum[i] * spectrum[i];

        r = eH / eL;

        
        /*
        
        if (eH > noSoundThreshold)
        {
            //Debug.Log(eH + "/" + eL + "/" + r);
            if (r > breathDetectionThreshold)
            {
                Debug.Log("BREATH");
                //Debug.Log(eH + "/" + eL + "/" + r);
                float strength = 19.14f*Mathf.Log10(eH) +184.86f;
                if (strength > 127.0f)
                {
                    strength = 127.0f;
                }
                //Debug.Log("strength" + strength);
                dandelionManagement.isBlown(1.0f, strength);
                //dandelionManagement.isBlown(1.0f, 100f);
            }
            else
            {
                //Debug.Log(eH + "/" + eL + "/" + r);
                uint pitch = noteplayer.nowPitch;
                noteplayer.NoteOff(pitch);
                //Debug.Log("1止めた11111111111111111111");
            }
                

        }
        else
        {
        
            uint pitch=noteplayer.nowPitch;
            noteplayer.NoteOff(pitch);
            //Debug.Log("2止めた222222222222222222222");

        }
        */

        

        
        
        if (Input.GetKey(KeyCode.P))
        {
            //Debug.Log("BREATH");
            dandelionManagement.isBlown(1.0f, 127f);
            //最低50, 最高127
        }
        else
        {

            uint pitch = noteplayer.nowPitch;
            noteplayer.NoteOff(pitch);
            //Debug.Log("止めた");
        }
        



            /*

            if (Input.GetKey(KeyCode.P))
            {
                all += 1;

                if (eH > noSoundThreshold)
                {
                    isEH = true;
                    //eHcou += 1;
                    //Debug.Log(eH + "/" + eL + "/" + r);
                    //Debug.Log(eH + "/" + eL + "/" + r + "/" + isEH);
                    if (r > breathDetectionThreshold)
                    {
                        isEH = false;
                        isR = true;
                        Debug.Log(eH + "/" + eL + "/" + r + "/" + isR);
                        Debug.Log("BREATH");
                    }


                    //rcou += 1;
                    //Debug.Log("BREATH");
                }
                else
                {
                    nocou += 1;
                    Debug.Log(eH + "/" + eL + "/" + r + "/" + isEH);
                    Debug.Log("誤");

                }

            }
            if (Input.GetKey(KeyCode.O))
            {
                Debug.Log("誤判定" + nocou + "/" + brecou + "/" + nocou / brecou);

                brecou = 0;
                nocou = 0;

            }
            */
        }


        //void KeyDetection()
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        dandelionManagement.isBlown(-1.0f, 1.0f);
        //    }
        //    else if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        dandelionManagement.isBlown( 0.0f, 1.0f);
        //    }
        //    else if (Input.GetKeyDown(KeyCode.D))
        //     {
        //         dandelionManagement.isBlown( 1.0f, 1.0f);
        //}
        /* // ここで音を止めるコードを書くと、実際のマイクからの制御に悪影響がある
        else
        {
            Vector3 camPos = GameObject.FindWithTag("MainCamera").transform.position;
            float z = camPos.z;
            dandelionManagement.CheckPassingDandelion(z);
            noteplayer.NoteOff(noteplayer.nowPitch);
        }
        */
        //}

        /*

        private void CreateFile1()
        {
            Encoding utf8 = System.Text.Encoding.UTF8;
            sw = new StreamWriter(@"D:\ozaki\MusicPlayingDandelion\dandelion\application-video\missCVS001.csv", true, utf8);///true 追記 ,false　すでにファイルが存在する場合そのファイルは消去され上書き保存される。
            sw.Close();
        }

        public void SaveData2(string[] num)
        {
            Encoding utf8 = System.Text.Encoding.UTF8;
            sw = new StreamWriter(@"D:\ozaki\MusicPlayingDandelion\dandelion\application-video\missCVS001.csv", true, utf8);//true 追記
            string s2 = string.Join(",", num);
            sw.WriteLine(s2);
            sw.Close();


        */
    }
