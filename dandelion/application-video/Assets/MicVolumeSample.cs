using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MicVolumeSample : MonoBehaviour
{

    [SerializeField, Range(0f, 10f)] float m_gain = 1f; // 音量に掛ける倍率
    float m_volumeRate; // 音量(0-1)
    //public List<float> volumeList = new List<float>();

    private AudioSource aud;
    private readonly float[] spectrum = new float[256];

    private List<List<string>> datalist = new List<List<string>>();
    private int count = 0;
   


    // Use this for initialization
    void Start()
    {
        aud = GetComponent<AudioSource>();
        if ((aud != null) && (Microphone.devices.Length > 0)) // オーディオソースとマイクがある
        {
            string devName = Microphone.devices[0]; // 複数見つかってもとりあえず0番目のマイクを使用
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // 最大最小サンプリング数を得る
            aud.clip = Microphone.Start(devName, true, 2, minFreq); // 音の大きさを取るだけなので最小サンプリングで十分
            aud.Play(); //マイクをオーディオソースとして実行(Play)開始
        }
        CreateFile1();
    }

    // Update is called once per frame
    void Update()
    {
        aud.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        for(int i=0;i<256;i++)
            if(spectrum[i]>0.01f)
            {
                if  (Input.GetKeyDown(KeyCode.P))
                {
                    float fHz = i;
                    string hz = fHz.ToString();
                    float fNum = spectrum[i];
                    string num = fNum.ToString();
                    datalist[count].Add(hz);
                    datalist[count].Add(num);
                }
                //datalist[count].Add(i);
                //datalist[count].Add(spectrum[i]);

                //Debug.Log(i);
                //Debug.Log(spectrum[i]);
                count += 1;
            }

        if (Input.GetKeyUp(KeyCode.P))
        {
            int a = datalist.Count;

            string path = Application.persistentDataPath + "/sample1.txt";

            for  (int i = 0; i < a; i++)
            {
                string text = datalist[i][0] + " " + datalist[i][1];
                File.WriteAllText(path, text);
            }
                    

        }


            //Debug.Log(spectrum);
            //Debug.Log(m_volumeRate);
        }

    // オーディオが読まれるたびに実行される
    private void OnAudioFilterRead(float[] data, int channels)
    {
        
        float sum = 0f;
        for (int i = 0; i < data.Length; ++i)
        {
            sum += Mathf.Abs(data[i]); // データ（波形）の絶対値を足す
        }
        // データ数で割ったものに倍率をかけて音量とする
        m_volumeRate = Mathf.Clamp01(sum * m_gain / (float)data.Length);

        //volumeList.Add(m_volumeRate);
    }

    private void CreateFile1()
    {
        var path = "sample1.txt";
        var fileStream = File.Create(path);
        fileStream.Close();
    }
}
