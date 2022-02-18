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

    [SerializeField, Range(0f, 10f)] float m_gain = 1f; // ���ʂɊ|����{��
    float m_volumeRate; // ����(0-1)
    //public List<float> volumeList = new List<float>();

    private AudioSource aud;
    private readonly float[] spectrum = new float[256];

    private List<List<string>> datalist = new List<List<string>>();
    private int count = 0;
   


    // Use this for initialization
    void Start()
    {
        aud = GetComponent<AudioSource>();
        if ((aud != null) && (Microphone.devices.Length > 0)) // �I�[�f�B�I�\�[�X�ƃ}�C�N������
        {
            string devName = Microphone.devices[0]; // �����������Ă��Ƃ肠����0�Ԗڂ̃}�C�N���g�p
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // �ő�ŏ��T���v�����O���𓾂�
            aud.clip = Microphone.Start(devName, true, 2, minFreq); // ���̑傫������邾���Ȃ̂ōŏ��T���v�����O�ŏ\��
            aud.Play(); //�}�C�N���I�[�f�B�I�\�[�X�Ƃ��Ď��s(Play)�J�n
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

    // �I�[�f�B�I���ǂ܂�邽�тɎ��s�����
    private void OnAudioFilterRead(float[] data, int channels)
    {
        
        float sum = 0f;
        for (int i = 0; i < data.Length; ++i)
        {
            sum += Mathf.Abs(data[i]); // �f�[�^�i�g�`�j�̐�Βl�𑫂�
        }
        // �f�[�^���Ŋ��������̂ɔ{���������ĉ��ʂƂ���
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
