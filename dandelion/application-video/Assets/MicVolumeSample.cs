using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicVolumeSample : MonoBehaviour
{

    [SerializeField, Range(0f, 10f)] float m_gain = 1f; // ���ʂɊ|����{��
    float m_volumeRate; // ����(0-1)
    //public List<float> volumeList = new List<float>();

    private AudioSource aud;
    private readonly float[] spectrum = new float[256];
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
    }

    // Update is called once per frame
    void Update()
    {
        aud.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        for(int i=0;i<256;i++)
            if(spectrum[i]>0.01f)
            {
                Debug.Log(i);
                Debug.Log(spectrum[i]);
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
}
