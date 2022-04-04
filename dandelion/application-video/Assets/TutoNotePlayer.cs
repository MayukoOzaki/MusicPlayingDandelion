using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Threading.Tasks;

public class TutoNotePlayer : MonoBehaviour
{


    [DllImport("winmm.dll")]
    public static extern uint midiOutOpen(out IntPtr lphMidiOut, int uDeviceID, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);

    [DllImport("winmm.dll")]
    public static extern uint midiOutShortMsg(IntPtr hMidiOut, uint dwMsg);

    [DllImport("winmm.dll")]
    public static extern uint midiOutClose(IntPtr hMidiOut);

    [DllImport("winmm.dll")]
    public static extern uint midiOutReset(IntPtr hMidiOut);

    IntPtr hMidiOut;
    const int MIDI_MAPPER = -1;

    public uint nowPitch;//今音を出したピッチ
    public bool nowOn = false;//音を出した
    public uint nowVolume;//今の音量

    public bool sameTone = false;//前と同じ音

    private int otocou = 0;

    public TutoDandelionManagement tutodandelionManagement;

    private Dictionary<uint, int> currentNotes = new Dictionary<uint, int>();




    // Start is called before the first frame update;
    void Start()
    {

        //▼MIDIデバイスオープン
        TutoNotePlayer.midiOutOpen(out hMidiOut, MIDI_MAPPER, IntPtr.Zero, IntPtr.Zero, uint.MinValue);
        //NoteOn(50, 100, 0);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void NoteOn(uint Pitch, uint Velocity, uint ToneColor)//吹いたときに呼び出される　Pitch:ピッチ Velocity:強さ ToneColor:音色
    {
        nowPitch = Pitch;
        nowVolume = Velocity;


        //▼音色選択

        uint ch = 0xc0;
        uint TC_data = (ch << 0) + (ToneColor << 8);
        TutoNotePlayer.midiOutShortMsg(hMidiOut, TC_data);   // 音色を定義 アコースティックピアノ0x0(0)


        // midiOutShortMsg(h, 0xc0);   
        // 音色を定義 アコースティックピアノ0x0(0)
        //	MIDIショートメッセージ
        //	0xc0 1byte目 ステータスバイト 音色を定義 0xc0+チェンネル0
        //	0x0 2byte目 データーバイト1 アコースティックピアノ0x0(0) 

        //音色
        //midiOutShortMsg(h, 0x41c0);	// 音色を定義 アルトサックス0x41(65)
        //midiOutShortMsg(h, 0x44c0);	// 音色を定義 オーボエ0x44(68)
        //midiOutShortMsg(h , 0x47c0);	// 音色を定義 クラリネット0x47(71)
        //midiOutShortMsg(h , 0x49c0);	// 音色を定義 フルート0x49(73)
        //midiOutShortMsg(h, 0x4ec0);	// 音色を定義 口笛0x4e(78)
        //midiOutShortMsg(h , 0x38c0);	// 音色を定義 トランペット0x38(56)
        //midiOutShortMsg(h, 0x39c0);	// 音色を定義 トロンボーン0x39(57)
        //midiOutShortMsg(h , 0x3ac0);	// 音色を定義 チューバ0x3a(58)
        //midiOutShortMsg(h , 0x3cc0);	// 音色を定義 フレンチ・ホルン0x3c(60)


        //▼演奏
        uint on = 0x90;
        uint on_data = (on << 0) + (Pitch << 8) + (Velocity << 16);


        int value;
        bool hasValue = currentNotes.TryGetValue(Pitch, out value);
        if (hasValue)
        {
            NoteOff(Pitch, value);
        }
        TutoNotePlayer.midiOutShortMsg(hMidiOut, on_data);
        currentNotes.Add(Pitch, tutodandelionManagement.nowNotenumber);


        //Debug.Log("鳴らす"+on_data.ToString("X")+"固有番号"+dandelionManagement.nowNotenumber);

        //Debug.Log("強さ1" +"/"+ Velocity);
        //Debug.Log("鳴らした");
        //Debug.Log(Pitch+"/"+Velocity+"/"+ToneColor);
        //Debug.Log("音を鳴らした"+otocou);

        nowOn = true;
        otocou += 1;






        //NotePlayer.midiOutShortMsg(hMidiOut,0x7f0090);

        //midiOutShortMsg(h, 0x7f0090);   
        // 鍵盤を押す C0 0x0(0) 127 チェンネル0
        //	MIDIショートメッセージ
        //	0x90 1byte目 ステータスバイト 鍵盤を押す0x90+チェンネル0
        //	0x0 2byte目 データーバイト1 音階 C 0オクターブ
        //	0x7f 2byte目 データーバイト2 ベロシティ 127


    }


    // public void NoteOff(uint Pitch, uint Velocity)//strengthが０の時に呼び出される
    // {
    //uint off = 0x80;
    //uint off_data = (off << 0) + (Pitch << 8) + (Velocity << 16);
    //NotePlayer.midiOutShortMsg(hMidiOut, off_data);
    //}

    public void NoteOff(uint Pitch, int notenumber)//strengthが０の時に呼び出される
    {
        uint Velocity = 0x0;
        uint off = 0x90;
        uint off_data = (off << 0) + (Pitch << 8) + (Velocity << 16);

        int value;
        bool hasValue = currentNotes.TryGetValue(Pitch, out value);
        if (hasValue)
        {
            if (value == notenumber)
            {
                TutoNotePlayer.midiOutShortMsg(hMidiOut, off_data);
                currentNotes.Remove(Pitch); //辞書から消去

                //Debug.Log("止めた" + off_data.ToString("X") + "固有番号" + notenumber);
                nowOn = false;
                //nowVolume = 0;

            }
        }

        //NotePlayer.midiOutShortMsg(hMidiOut, off_data);
        //Debug.Log("止めた" + off_data.ToString("X") + "固有番号" + dandelionManagement.nowNotenumber);
        //Debug.Log("止めた")

    }

    public void ExpressionChange(uint Volume)
    {
        nowVolume = Volume;
        uint exppression = 0xB0;
        uint byte2 = 0x0b;
        uint expression_data = (exppression << 0) + (byte2 << 8) + (Volume << 16);
        ///Debug.Log("変えた");
        TutoNotePlayer.midiOutShortMsg(hMidiOut, expression_data);
        //Debug.Log("強さ2" +"/"+ Volume);
        //Debug.Log("変えた" + expression_data.ToString("X"));
    }

    public void SmoothChangeZero()
    {
        uint volume = 0;
        if (nowVolume <= 1)
        {
            volume = 0;
        }
        else
        {
            volume = nowVolume - 1;
        }

        ExpressionChange(volume);
        //Debug.Log("SmoothChangeZero");
    }


    public void SmoothChange(uint Velocity)
    {
        uint volume = nowVolume;

        int diff = Math.Abs((int)nowVolume - (int)Velocity);
        if (nowVolume > Velocity)
        {

            //Debug.Log("SmoothChange");
            int num = 1;
            if (diff > 4)
            {
                num = (int)(diff / 4);
            }

            uint d = (uint)num;

            ExpressionChange(volume - d);

        }
        else
        {
            ExpressionChange(Velocity);
        }

    }




    void EndPerformance()
    {
        //▼MIDIデバイス開放
        TutoNotePlayer.midiOutReset(hMidiOut);
        TutoNotePlayer.midiOutClose(hMidiOut);
        //Debug.Log("EndPerformance")
    }
    private void OnDestroy()
    {
        EndPerformance();
        //Debug.Log("OnDestroy");
    }

}
