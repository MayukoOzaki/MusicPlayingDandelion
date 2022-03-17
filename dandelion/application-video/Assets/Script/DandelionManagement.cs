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
    public bool isWidth = true;
    public float BlownWidth = 5.0f;
    public float ZDistance = 0.1f;

    int numSounds = 10;//270;
    public List<GameObject> ObjectList = new List<GameObject>(); //たんぽぽlist

    public NotePlayer notePlayer;

    private float camPosz;
    private int nowNotenumber=0;





    //DandelionManagement
    //1. SetPosition() たんぽぽ生成
    //2. たんぽぽリスト
    //3. isBlown() 位置と強さを受け取ってたんぽぽを消す・音を鳴らす
    //4. 音を鳴らす関数

    // Start is called before the first frame update
    void Start()
    {
        ReadText(toneFile);

        //Debug.Log(toneDatas.Count);         // 行数(271)
        //Debug.Log(toneDatas[0].Length);       // 項目数(4)　0:start.1:end, 2:pitch, 3:velocity
        //Debug.Log(toneDatas[1][2]);        // 2行目3列目(45)

        SetPosition(toneDatas);

    }

    void ReadText(TextAsset fileName)
    {
        string[] lines = fileName.text.Replace("\r\n", "\n").Split("\n"[0]);
        foreach (var line in lines)
        {
            if (line == "") { continue; }
            toneDatas.Add(line.Split(' '));    // string[]を追加している
        }

    }

    void SetPosition(List<string[]> data)
    {

        float posz = 0f;
        int notenum = 0;//note number 
        for (int r=0;r<= numSounds; r++)//音の数
        {
            
            //横の位置 c4:60
            //-1:0-11 0:12-23 1:24-35 2:36-47 3:48:59 4:60-71 5:72-83 6:84-95 7:96-107
            float posx = float.Parse(toneDatas[r][2])-60.0f;
            //縦の位置
            //float posz = float.Parse(toneDatas[r][0]);
            //個数
            float timelength=float.Parse(toneDatas[r][1])-float.Parse(toneDatas[r][0]);//0.25ごとに1つ
            int quantity = (int)( timelength / 0.25f);//0.25ごとに1つ

            /*
            //１つのオブジェクトだけ生成
            Vector3 pos = new Vector3(posx, 0f, posz);
            GameObject dandelion = Instantiate(DandelionPrefab, pos, Quaternion.identity);
            dandelion.GetComponent<NoteInfo>().pitch = int.Parse(toneDatas[r][2]);
            dandelion.GetComponent<NoteInfo>().start = float.Parse(toneDatas[r][0]);
            dandelion.GetComponent<NoteInfo>().end = float.Parse(toneDatas[r][1]);
            dandelion.GetComponent<NoteInfo>().soundLength = float.Parse(toneDatas[r][1]) - float.Parse(toneDatas[r][0]);
            ObjectList.Add(dandelion);
            posz += 0.5f;
            */

            notenum += 1;
           
            for (int s = 0; s <= quantity-1; s++)
            {
                Vector3 pos = new Vector3(posx, 0f, posz);
                GameObject dandelion = Instantiate(DandelionPrefab, pos, Quaternion.identity);
                dandelion.GetComponent<NoteInfo>().pitch = int.Parse(toneDatas[r][2]);
                dandelion.GetComponent<NoteInfo>().start = float.Parse(toneDatas[r][0]);
                dandelion.GetComponent<NoteInfo>().end = float.Parse(toneDatas[r][1]);
                dandelion.GetComponent<NoteInfo>().soundLength = float.Parse(toneDatas[r][1])- float.Parse(toneDatas[r][0]);
                dandelion.GetComponent<NoteInfo>().noteNumber = notenum;
                ObjectList.Add(dandelion);
                posz += 0.5f;
            }
            


            //To Do
            //1: 連続する場合は高さを変える（茎）
            //2: velocityごとに大きさを変える（綿毛）
            //3: 色の変更(中身)中の色が変わっていることが外側から分かるようにする

        }


    }

    public void isBlown(float Posx, float Strength) //Posx:吹いた位置　Strength:吹いた強さ
    {
        
        //int count = ObjectList.Count;
        //Debug.Log("count"+count);
        if (ObjectList.Count == 0)
            return;
        GameObject dandelion = ObjectList[0];
        if (isWidth==false)
        {
            BlownWidth = 999999.0f;
        }
        if (Mathf.Abs(dandelion.transform.position.x - Posx) < BlownWidth)
        {
            if(Math.Abs(dandelion.transform.position.z-camPosz)<ZDistance)
            {
                Debug.Log("isblown");
                //Destroy(ObjectList[0]);// リストの0番目のオブジェクトを消す
                //ObjectList.RemoveAt(0);// リストの0番目を削除する

                int i_pitch = dandelion.GetComponent<NoteInfo>().pitch;
                uint pitch = (uint)i_pitch;
                int i_velocity = (int)Strength;
                uint velocity = (uint)i_velocity;
                uint ToneColor = 0x0;

                Debug.Log(dandelion.GetComponent<NoteInfo>().noteNumber);

                int notenum = dandelion.GetComponent<NoteInfo>().noteNumber;

                if (notenum > nowNotenumber)
                {
                    notePlayer.NoteOn(pitch, velocity, ToneColor);//音再生
                    nowNotenumber = notenum;
                }

                //notePlayer.NoteOn(pitch, velocity, ToneColor);//音再生

                //notePlayer.NoteOn(50, 100, 0);//テスト用

                Destroy(ObjectList[0]);// リストの0番目のオブジェクトを消す
                ObjectList.RemoveAt(0);// リストの0番目を削除する

                //同じ音番号の時はリターン音を再生しない。

                //音の再生を止める条件は、息が止まった時と、endの範囲外になったとき。
            }

            /*
            Debug.Log("isblown");
            //Destroy(ObjectList[0]);// リストの0番目のオブジェクトを消す
            //ObjectList.RemoveAt(0);// リストの0番目を削除する

            int i_pitch = dandelion.GetComponent<NoteInfo>().pitch;
            uint pitch = (uint)i_pitch;
            int i_velocity = (int)Strength;
            uint velocity = (uint)i_velocity;
            uint ToneColor = 0x0;

            notePlayer.NoteOn(pitch, velocity, ToneColor);//音再生

            //notePlayer.NoteOn(50, 100, 0);//テスト用

            Destroy(ObjectList[0]);// リストの0番目のオブジェクトを消す
            ObjectList.RemoveAt(0);// リストの0番目を削除する

            //同じ音番号の時はリターン音を再生しない。

            //音の再生を止める条件は、息が止まった時と、endの範囲外になったとき。
            */
        }
        
    }

    public void CheckPassingDandelion(float Posz) //Posz:カメラの位置
    {
        if (ObjectList.Count == 0)
            return;
        GameObject dandelion = ObjectList[0];
        if(dandelion.transform.position.z < Posz)
        {
            //Debug.Log("notblown");
            ObjectList.RemoveAt(0);// リストの0番目を削除する
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = GameObject.FindWithTag("MainCamera").transform.position;
        camPosz = camPos.z;
        CheckPassingDandelion(camPosz);
    }

    //位置はゴーグルからとる。

}
