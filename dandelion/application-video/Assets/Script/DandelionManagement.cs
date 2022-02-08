using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class DandelionManagement : MonoBehaviour
{

    [SerializeField] public TextAsset toneFile;
    List<string[]> toneDatas = new List<string[]>();

    public GameObject DandelionPrefab;
    public float BlownWidth = 5.0f;

    int numSounds = 10;//270;

    public List<GameObject> ObjectList = new List<GameObject>(); //�����ۂ�list

    //DandelionManagement
    //1. SetPosition() �����ۂې���
    //2. �����ۂۃ��X�g
    //3. isBlown() �ʒu�Ƌ������󂯎����Ă����ۂۂ������E�����炷
    //4. �����炷�֐�

    // Start is called before the first frame update
    void Start()
    {
        ReadText(toneFile);

        Debug.Log(toneDatas.Count);         // �s��(271)
        Debug.Log(toneDatas[0].Length);       // ���ڐ�(4)�@0:start.1:end, 2:pitch, 3:velocity
        Debug.Log(toneDatas[1][2]);        // 2�s��3����(45)

        SetPosition(toneDatas);

    }

    void ReadText(TextAsset fileName)
    {
        string[] lines = fileName.text.Replace("\r\n", "\n").Split("\n"[0]);
        foreach (var line in lines)
        {
            if (line == "") { continue; }
            toneDatas.Add(line.Split(' '));    // string[]���ǉ����Ă���
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
                ObjectList.Add(DandelionPrefab);
                posz += 0.5f;
            }

            //To Do
            //1: �A�������ꍇ�͍������ς����i�s�j
            //2: velocity���Ƃɑ傫�����ς����i�Ȗсj
            //3: �F�̕ύX(���g)���̐F���ς����Ă��邱�Ƃ��O�����番�����悤�ɂ���

        }


    }

    public void isBlown(float Posx, float Strength) //Posx:�������ʒu�@Strength:����������
    {
        if( ObjectList.Count == 0 )
            return;
        GameObject Dan = ObjectList[0];
        if (Mathf.Abs(Dan.transform.position.x-Posx) < BlownWidth)
        {
            Debug.Log("isblown");
            Destroy(ObjectList[0]);// ���X�g��0�Ԗڂ̃I�u�W�F�N�g������
            ObjectList.RemoveAt(0);// ���X�g��0�Ԗڂ��폜����
        }
    }

    public void notBlown(float Posz) //Posz:�J�����̈ʒu
    {
        GameObject Dan = ObjectList[0];
        if(Dan.transform.position.z < Posz)
        {
            Debug.Log("notblown");
            ObjectList.RemoveAt(0);// ���X�g��0�Ԗڂ��폜����
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
