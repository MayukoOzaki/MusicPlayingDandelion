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
    public List<GameObject> ObjectList = new List<GameObject>(); //たんぽぽlist

    //DandelionManagement
    //1. SetPosition() たんぽぽ生成
    //2. たんぽぽリスト
    //3. isBlown() 位置と強さを受け取ってたんぽぽを消す・音を鳴らす
    //4. 音を鳴らす関数

    // Start is called before the first frame update
    void Start()
    {
        ReadText(toneFile);

        Debug.Log(toneDatas.Count);         // 行数(271)
        Debug.Log(toneDatas[0].Length);       // 項目数(4)　0:start.1:end, 2:pitch, 3:velocity
        Debug.Log(toneDatas[1][2]);        // 2行目3列目(45)

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
        for (int r=0;r<= numSounds; r++)
        {
            //横の位置 c4:60
            //-1:0-11 0:12-23 1:24-35 2:36-47 3:48:59 4:60-71 5:72-83 6:84-95 7:96-107
            float posx = float.Parse(toneDatas[r][2])-60.0f;
            //縦の位置
            //float posz = float.Parse(toneDatas[r][0]);
            //個数
            float timelength=float.Parse(toneDatas[r][1])-float.Parse(toneDatas[r][0]);//0.25ごとに1つ
            int quantity = (int)( timelength / 0.25f);//0.25ごとに1つ


            for (int s = 0; s <= quantity-1; s++)
            {
                Vector3 pos = new Vector3(posx, 0f, posz);
                Instantiate(DandelionPrefab, pos, Quaternion.identity);
                ObjectList.Add(DandelionPrefab);
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
        if( ObjectList.Count == 0 )
            return;
        GameObject Dan = ObjectList[0];
        if (Mathf.Abs(Dan.transform.position.x-Posx) < BlownWidth)
        {
            Debug.Log("isblown");
            Destroy(ObjectList[0]);// リストの0番目のオブジェクトを消す
            ObjectList.RemoveAt(0);// リストの0番目を削除する
        }
    }

    void notBlown(float Posz) //Posz:カメラの位置
    {
        GameObject Dan = ObjectList[0];
        if(Dan.transform.position.z < Posz)
        {
            Debug.Log("notblown");
            ObjectList.RemoveAt(0);// リストの0番目を削除する
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
