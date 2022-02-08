using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Threading.Tasks;

public class NotePlayer : MonoBehaviour
{
    IntPtr hMidiOut;
    const int MIDI_MAPPER = -1;

    [DllImport("winmm.dll")]
    public static extern uint midiOutOpen(out IntPtr lphMidiOut, int uDeviceID, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);

    [DllImport("winmm.dll")]
    public static extern uint midiOutShortMsg(IntPtr hMidiOut, uint dwMsg);

    [DllImport("winmm.dll")]
    public static extern uint midiOutClose(IntPtr hMidiOut);

    [DllImport("winmm.dll")]
    public static extern uint midiOutReset(IntPtr hMidiOut);
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void NoteOn(float Pitch)//吹いたときに呼び出される
    {
        
        //▼MIDIデバイスオープン
        NotePlayer.midiOutOpen(out hMidiOut, MIDI_MAPPER, IntPtr.Zero, IntPtr.Zero, uint.MinValue);

        //▼音色選択。 ピアノ：0x0000C0、アコーディオン：0x0016C0、シンセ：0x0051C0
        NotePlayer.midiOutShortMsg(hMidiOut, 0x0016C0);

        //▼演奏
        //ド
        NotePlayer.midiOutShortMsg(hMidiOut, 0x7F3C90);
        Task.Delay(800).Wait();
        NotePlayer.midiOutShortMsg(hMidiOut, 0x003C90);

        //レ
        NotePlayer.midiOutShortMsg(hMidiOut, 0x7F3E90);
        Task.Delay(800).Wait();
        NotePlayer.midiOutShortMsg(hMidiOut, 0x003E90);

        //ミ
        NotePlayer.midiOutShortMsg(hMidiOut, 0x7F4090);
        Task.Delay(800).Wait();
        NotePlayer.midiOutShortMsg(hMidiOut, 0x004090);

        //▼MIDIデバイス開放
        NotePlayer.midiOutReset(hMidiOut);
        NotePlayer.midiOutClose(hMidiOut);


    }

    void NoteOff(float Pitch)//strengthが０の時に呼び出される
    {

    }
}
