using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MusicTest : MonoBehaviour
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


    // Start is called before the first frame update
    void Start()
    {

        //▼MIDIデバイスオープン
        MusicTest.midiOutOpen(out hMidiOut, MIDI_MAPPER, IntPtr.Zero, IntPtr.Zero, uint.MinValue);
        //play();

        uint ch = 0xc0;
        uint ToneColor = 0x0;
        uint TC_data = (ch << 0) + (ToneColor << 8);
        Debug.Log(TC_data.ToString("X"));
        midiOutShortMsg(hMidiOut, TC_data);   // 音色を定義 アコースティックピアノ0x0(0)

        uint on = 0x90;
        uint off = 0x80;
        uint Pitch = 0x30;
        uint Velocity = 0x7f;
        uint on_data = (on << 0) + (Pitch << 8) + (Velocity << 16);
        uint off_data = (off << 0) + (Pitch << 8) + (Velocity << 16);
        Debug.Log(on_data.ToString("X"));

        NotePlayer.midiOutShortMsg(hMidiOut, on_data);
        Task.Delay(554).Wait();
        midiOutShortMsg(hMidiOut, off_data);

        //Debug.Log(hMidiOut);

        NotePlayer.midiOutReset(hMidiOut);
        NotePlayer.midiOutClose(hMidiOut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void play()
    {
        midiOutShortMsg(hMidiOut, 0x2ac0);  // 音色を定義 チェロ0x2a(42)

        midiOutShortMsg(hMidiOut, 0x7f4390);  // 鍵盤を押す G5 0x43(67) 127 チェンネル0
        Task.Delay(277).Wait();
        midiOutShortMsg(hMidiOut, 0x7f4380);  // 鍵盤を離す G5 0x43(67) 127 チェンネル0

        midiOutShortMsg(hMidiOut, 0x7f4390);  // 鍵盤を押す G5 0x43(67) 127 チェンネル0
        Task.Delay(277).Wait();
        midiOutShortMsg(hMidiOut, 0x7f4380);  // 鍵盤を離す G5 0x43(67) 127 チェンネル0

        midiOutShortMsg(hMidiOut, 0x7f4390);  // 鍵盤を押す G5 0x43(67) 127 チェンネル0
        Task.Delay(277).Wait();
        midiOutShortMsg(hMidiOut, 0x7f4380);  // 鍵盤を離す G5 0x43(67) 127 チェンネル0

        midiOutShortMsg(hMidiOut, 0x7f3f90);  // 鍵盤を押す D+,E-5 0x3f(63) 127 チェンネル0
        Task.Delay(554).Wait();
        midiOutShortMsg(hMidiOut, 0x7f3f80);  // 鍵盤を離す D+,E-5 0x3f(63) 127 チェンネル0
    }
}
