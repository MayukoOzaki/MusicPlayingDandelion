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
    //public bool isWidth = true;
    public float BlownWidth = 2.5f;
    //public float ZDistance = 0.1f;




    int numSounds = 270;

//    public List<GameObject> ObjectList = new List<GameObject>(); //たんぽぽlist

    public GameObject targetDandelion;
    public GameObject lastDandelion;

    public NotePlayer notePlayer;
    public HeadsetSetup headsetSetup;


    private Vector3 maincamPos;
    private float maincamPosz;

    public  int nowNotenumber=0;
    private uint tonecolor=0x0;

    public bool dan=false;

    public string nowArrow = null;//null, right, left
    public GameObject rArrow;
    public GameObject lArrow;

    
    




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
        targetDandelion = null;
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
        string nowInstrument = "Piano" ;
        for (int r=0;r<= numSounds; r++)//音の数
        {
            //横の位置 c4:60
            //-1:0-11 0:12-23 1:24-35 2:36-47 3:48:59 4:60-71 5:72-83 6:84-95 7:96-107
            float posx = (float.Parse(toneDatas[r][2])-60.0f) * 0.05f;
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
            //ObjectList.Add(dandelion);
            posz += 0.25f;
            */

            //toneColor 24, 32,32,112,40,31
            notenum += 1;
            if (notenum == 25)
            {
                tonecolor = 0x38;//トランペット
                nowInstrument = "Trumpet";
            }
            else if(notenum == 57)
            {
                tonecolor = 0x49;//フルート
                nowInstrument = "Flute";
            }
            else if (notenum == 89)
            {
                tonecolor = 0x47;//クラリネット
                nowInstrument = "Clarinet";
            }
            else if (notenum == 201)
            {
                tonecolor = 0x41;//アルトサックス
                nowInstrument = "AssaultSaxophone";
            }
            else if (notenum == 241)
            {
                tonecolor = 0x44;//オーボエ
                nowInstrument = "Oboe";
            }



            for (int s = 0; s <= quantity-1; s++)
            {

                Vector3 pos = new Vector3(posx, 0f, posz);
                GameObject dandelion = Instantiate(DandelionPrefab, pos, Quaternion.identity);
                dandelion.GetComponent<NoteInfo>().pitch = int.Parse(toneDatas[r][2]);
                dandelion.GetComponent<NoteInfo>().start = float.Parse(toneDatas[r][0]);
                dandelion.GetComponent<NoteInfo>().end = float.Parse(toneDatas[r][1]);
                dandelion.GetComponent<NoteInfo>().soundLength = (float.Parse(toneDatas[r][1])- float.Parse(toneDatas[r][0]))-(0.25f*s);
                dandelion.GetComponent<NoteInfo>().noteNumber = notenum;
                dandelion.GetComponent<NoteInfo>().toneColor = tonecolor;
                dandelion.GetComponent<NoteInfo>().velocity = float.Parse(toneDatas[r][3]);
                changeSeedColor(dandelion,nowInstrument);

                float velocity = float.Parse(toneDatas[r][3]);
                //GameObject head = dandelion.transform.Find("HeadOutside").gameObject;
                //head.GetComponent<HeadSizeChange>().ChangeHeadSize(velocity);

                GameObject stem = dandelion.transform.Find("Stem").gameObject;
                stem.GetComponent<StemSizeChange>().ChangeStemSize(s);
                Vector3 danPos = dandelion.transform.position;
                danPos.y = (danPos.y+stem.GetComponent<StemSizeChange>().defaultScale.y - 0.105f)*2;
                dandelion.transform.position = danPos;



                //          ObjectList.Add(dandelion);
                posz += 0.25f;
                lastDandelion = dandelion;
            }


            //To Do
            //1: 連続する場合は高さを変える（茎）
            //2: velocityごとに大きさを変える（綿毛）
            //3: 色の変更(中身)中の色が変わっていることが外側から分かるようにする

        }


    }

    private void changeSeedColor(GameObject dandelion,string Instrument)
    {
        GameObject Seed = dandelion.transform.Find("HeadCore/Seed").gameObject;
        ParticleSystem.MainModule par = Seed.GetComponent<ParticleSystem>().main;
        Color test = new Color32(1, 50, 0, 1);
        par.startColor = test;
        switch(Instrument)
        {
            case "Piano":
            {
                Color color = new Color32(255, 255, 255, 255);//white
                par.startColor = color;
                break;
            }
            case "Trumpet":
            {
                Color color = new Color32(255, 255, 0, 255);//Yellow/ /1.0f, 50.0f, 0.0f, 1.0f)
                    par.startColor = color;
                break;
            }
            case "Flute":
            {
                Color color = new Color32(28, 31, 254, 255);//blue
                par.startColor = color;
                break;
            }
            case "Clarinet":
            {
                Color color = new Color32(220, 60, 220, 255);//Pink
                par.startColor = color;
                break;
            }
            case "AssaultSaxophone":
            {
                Color color = new Color32(0, 255, 0, 255);//Green
                par.startColor = color;
                break;
            }
            case "Oboe":
            {
                Color color = new Color32(255, 0, 0, 255);//red
                par.startColor = color;
                break;
            }
            default:
                Debug.LogError("toneColorが存在していません");
                break;
        }
    }

    public void isBlown(float Posx, float Strength) //Posx:吹いた位置　Strength:吹いた強さ
    {
        //int count = ObjectList.Count;
        //Debug.Log("count"+count);
        //GameObject dandelion = ObjectList[0];

        if (targetDandelion == null)
            return;


        GameObject dandelion = targetDandelion;

        uint pitch = (uint)dandelion.GetComponent<NoteInfo>().pitch;
        uint velocity = (uint)Strength;
        uint ToneColor = dandelion.GetComponent<NoteInfo>().toneColor;
        int notenum = dandelion.GetComponent<NoteInfo>().noteNumber;
        //Debug.Log(dandelion.GetComponent<NoteInfo>().noteNumber);
        bool nowOn = notePlayer.nowOn;
        Vector3 camPos = headsetSetup.camPos;
        float camPosx = camPos.x;

        /*
        if (isWidth == false)
        {
            BlownWidth = 999999.0f;
        }
        */

        if (velocity == 0)
        {
            bool arrow=JudgeAngle(targetDandelion);
            notePlayer.SmoothChangeZero();
        }
        else
        {
            if (JudgeAngle(targetDandelion))
            {
                //Debug.Log("距離："+Vector3.Distance(dandelion.transform.position, camPos));
                float distance = Vector3.Distance(dandelion.transform.position, camPos);
                if (distance < BlownWidth)
                {
                    //Debug.Log("制限：" + BlownWidth);
                    //Debug.Log("距離：" + Vector3.Distance(dandelion.transform.position, camPos));
                    int value = JudgeDistance(dandelion);
                    velocity = velocity + (uint)value;
                    if (velocity > 127)
                    {
                        velocity = 127;
                    }
                    else if (velocity < 0)
                    {
                        velocity = 0;
                    }
                    //Debug.Log(value + "/" + (int)velocity);
                    notePlayer.SmoothChange(velocity);

                    Vector3 dir = dandelion.transform.position - camPos;
                    dandelion.GetComponent<DandelionController>().Blow(dir);
                    

                    if (notenum > nowNotenumber)
                    {
                        nowNotenumber = notenum;
                        notePlayer.NoteOn(pitch, notePlayer.nowVolume, ToneColor);//音再生
                    }
                }
                else
                {
                    notePlayer.SmoothChangeZero();
                }
            }
            else
            {
                notePlayer.SmoothChangeZero();
            }
        }
    }

        /*
        if (JudgeAngle(targetDandelion))
        {

            //Debug.Log("制限１");
            if (Math.Abs(dandelion.transform.position.z - maincamPosz) < ZDistance||true)
            {
                //Debug.Log("制限２");
                //Debug.Log("isblown");
                //Destroy(ObjectList[0]);// リストの0番目のオブジェクトを消す
                //ObjectList.RemoveAt(0);// リストの0番目を削除する

                
                

                //Debug.Log("吹いた強さ"+velocity);

                if (velocity==0)
                {
                    notePlayer.SmoothChangeZero();
                }
                else
                {
                    int value = JudgeDistance(dandelion);
                    velocity = velocity + (uint)value;
                    if (velocity > 127)
                    {
                        velocity = 127;
                    }
                    else if(velocity<0)
                    {
                        velocity = 0;
                    }
                    Debug.Log(value+"/"+(int)velocity);
                    notePlayer.SmoothChange(velocity);
                    //velocity = notePlayer.nowVolume;
                    //notePlayer.ExpressionChange(velocity);
                }
                //notePlayer.ExpressionChange(velocity);
                if (notePlayer.nowVolume > 0)
                {
                    Vector3 camPos = headsetSetup.camPos;
                    Vector3 dir = dandelion.transform.position - camPos;
                    dandelion.GetComponent<DandelionController>().Blow(dir);
                    if (notenum > nowNotenumber)
                    {
                        nowNotenumber = notenum;
                        notePlayer.NoteOn(pitch, notePlayer.nowVolume, ToneColor);//音再生
                        //nowNotenumber = notenum;
                    }
                }

            }
        }
        else
        {

        }

        */

/*

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
    */

    public bool JudgeAngle(GameObject dandelion)
    {

        Vector3 camPos = headsetSetup.camPos;
        Vector3 camforward = headsetSetup.camforward;//カメラの正面

        Vector3 dandelionDir = dandelion.transform.position - camPos;
        dandelionDir = dandelionDir.normalized;//カメラからタンポポ

        float diff = Vector3.Angle(camforward, dandelionDir);
        bool danBlown = false;
        if (dandelion.GetComponent<DandelionController>().isBlow==true)
        {
           danBlown = true;
        }


        // Debug.Log(diff);

        if (diff<=45.0f || danBlown == true)
        {
            if (nowArrow == "right")
            {
                rArrow.gameObject.SetActive(false);
            }
            else if(nowArrow=="left")
            {
                lArrow.gameObject.SetActive(false);
            }
            nowArrow = null;

            return true;
        }
        else
        {

            Vector3 cross=Vector3.Cross(camforward, dandelionDir).normalized;
            if (cross.y>=0)
            {
                if(nowArrow == "right")
                {
                    rArrow.gameObject.SetActive(false);
                }
                if (nowArrow != "left")
                {
                    lArrow.gameObject.SetActive(true);
                    nowArrow = "left";
                }

            }
            else
            {
                if (nowArrow == "left")
                {
                    lArrow.gameObject.SetActive(false);
                }
                if (nowArrow != "right")
                {
                    rArrow.gameObject.SetActive(true);
                    nowArrow = "right";
                }

            }
            return false;
        }

    }

    public int JudgeDistance(GameObject dandelion)
    {
        Vector3 posDandelion = dandelion.transform.position;
        Vector3 camPos = headsetSetup.camPos;
        float dis = Vector3.Distance(posDandelion, camPos);
        //Debug.Log("距離 : " + dis);
        //short 0, middle1.25, long 2.5

        int value = (int)(-dis * 22.4 + 28);
        return value;
    }


    public void SetTargetDandelion(GameObject dandelion)
    {
        targetDandelion = dandelion;
        //dan = true;

    }



    // Update is called once per frame
    void Update()
    {
        /*
        if (dan==true)
        {
            JudgeAngle(targetDandelion);
        }
        */
             
        //maincamPos = GameObject.FindWithTag("MainCamera").transform.position;
        //maincamPosz = maincamPos.z;

        //CheckPassingDandelion(camPosz);

    }

    //位置はゴーグルからとる。

}
