using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class NoteLayout : MonoBehaviour
{

    [SerializeField] public TextAsset toneFile;
    List<string[]> toneDatas = new List<string[]>();

    public GameObject DandelionPrefab;
    int numSounds = 10; //270;

    // Start is called before the first frame update
    void Start()
    {
        ReadText(toneFile);

        Debug.Log(toneDatas.Count);         // �s��(271)
        Debug.Log(toneDatas[0].Length);       // ���ڐ�(4)�@0:start.1:end, 2:pitch, 3:velocity
        Debug.Log(toneDatas[1][2]);        // 2�s��3���(45)

        SetPosition(toneDatas);
    }

    void ReadText(TextAsset fileName)
    {
        string[] lines = fileName.text.Replace("\r\n", "\n").Split("\n"[0]);
        foreach (var line in lines)
        {
            if (line == "") { continue; }
            toneDatas.Add(line.Split(' '));    // string[]��ǉ����Ă���
        }

    }

    void SetPosition(List<string[]> data)
    {
        float posz = 0f;
        for (int r=0;r<= numSounds; r++)
        {
            //���̈ʒu c4:60
            //-1:0-11 0:12-23 1:24-35 2:36-47 3:48:59 4:60-71 5:72-83 6:84-95 7:96-107
            float posx = float.Parse(toneDatas[r][2])-60.0f;
            //�c�̈ʒu
            //float posz = float.Parse(toneDatas[r][0]);
            //��
            float timelength=float.Parse(toneDatas[r][1])-float.Parse(toneDatas[r][0]);//0.25���Ƃ�1��
            int quantity = (int)( timelength / 0.25f);//0.25���Ƃ�1��


            for (int s = 0; s <= quantity-1; s++)
            {
                Vector3 pos = new Vector3(posx, 0f, posz);
                Instantiate(DandelionPrefab, pos, Quaternion.identity);
                posz += 0.5f;
            }

            //To Do
            //1: �A������ꍇ�͍�����ς���i�s�j
            //2: velocity���Ƃɑ傫����ς���i�Ȗсj
            //3: �F�̕ύX(���g)���̐F���ς���Ă��邱�Ƃ��O�����番����悤�ɂ���@����������Ȃǂ�
            
        }

        



    }

    // Update is called once per frame
    void Update()
    {

    }

}
