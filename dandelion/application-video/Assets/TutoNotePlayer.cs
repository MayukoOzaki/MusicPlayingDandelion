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

    public uint nowPitch;//�������o�����s�b�`
    public bool nowOn = false;//�����o����
    public uint nowVolume;//���̉���

    public bool sameTone = false;//�O�Ɠ�����

    private int otocou = 0;

    public TutoDandelionManagement tutodandelionManagement;

    private Dictionary<uint, int> currentNotes = new Dictionary<uint, int>();




    // Start is called before the first frame update;
    void Start()
    {

        //��MIDI�f�o�C�X�I�[�v��
        TutoNotePlayer.midiOutOpen(out hMidiOut, MIDI_MAPPER, IntPtr.Zero, IntPtr.Zero, uint.MinValue);
        //NoteOn(50, 100, 0);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void NoteOn(uint Pitch, uint Velocity, uint ToneColor)//�������Ƃ��ɌĂяo�����@Pitch:�s�b�` Velocity:���� ToneColor:���F
    {
        nowPitch = Pitch;
        nowVolume = Velocity;


        //�����F�I��

        uint ch = 0xc0;
        uint TC_data = (ch << 0) + (ToneColor << 8);
        TutoNotePlayer.midiOutShortMsg(hMidiOut, TC_data);   // ���F���` �A�R�[�X�e�B�b�N�s�A�m0x0(0)


        // midiOutShortMsg(h, 0xc0);   
        // ���F���` �A�R�[�X�e�B�b�N�s�A�m0x0(0)
        //	MIDI�V���[�g���b�Z�[�W
        //	0xc0 1byte�� �X�e�[�^�X�o�C�g ���F���` 0xc0+�`�F���l��0
        //	0x0 2byte�� �f�[�^�[�o�C�g1 �A�R�[�X�e�B�b�N�s�A�m0x0(0) 

        //���F
        //midiOutShortMsg(h, 0x41c0);	// ���F���` �A���g�T�b�N�X0x41(65)
        //midiOutShortMsg(h, 0x44c0);	// ���F���` �I�[�{�G0x44(68)
        //midiOutShortMsg(h , 0x47c0);	// ���F���` �N�����l�b�g0x47(71)
        //midiOutShortMsg(h , 0x49c0);	// ���F���` �t���[�g0x49(73)
        //midiOutShortMsg(h, 0x4ec0);	// ���F���` ���J0x4e(78)
        //midiOutShortMsg(h , 0x38c0);	// ���F���` �g�����y�b�g0x38(56)
        //midiOutShortMsg(h, 0x39c0);	// ���F���` �g�����{�[��0x39(57)
        //midiOutShortMsg(h , 0x3ac0);	// ���F���` �`���[�o0x3a(58)
        //midiOutShortMsg(h , 0x3cc0);	// ���F���` �t�����`�E�z����0x3c(60)


        //�����t
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


        //Debug.Log("�炷"+on_data.ToString("X")+"�ŗL�ԍ�"+dandelionManagement.nowNotenumber);

        //Debug.Log("����1" +"/"+ Velocity);
        //Debug.Log("�炵��");
        //Debug.Log(Pitch+"/"+Velocity+"/"+ToneColor);
        //Debug.Log("����炵��"+otocou);

        nowOn = true;
        otocou += 1;






        //NotePlayer.midiOutShortMsg(hMidiOut,0x7f0090);

        //midiOutShortMsg(h, 0x7f0090);   
        // ���Ղ����� C0 0x0(0) 127 �`�F���l��0
        //	MIDI�V���[�g���b�Z�[�W
        //	0x90 1byte�� �X�e�[�^�X�o�C�g ���Ղ�����0x90+�`�F���l��0
        //	0x0 2byte�� �f�[�^�[�o�C�g1 ���K C 0�I�N�^�[�u
        //	0x7f 2byte�� �f�[�^�[�o�C�g2 �x���V�e�B 127


    }


    // public void NoteOff(uint Pitch, uint Velocity)//strength���O�̎��ɌĂяo�����
    // {
    //uint off = 0x80;
    //uint off_data = (off << 0) + (Pitch << 8) + (Velocity << 16);
    //NotePlayer.midiOutShortMsg(hMidiOut, off_data);
    //}

    public void NoteOff(uint Pitch, int notenumber)//strength���O�̎��ɌĂяo�����
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
                currentNotes.Remove(Pitch); //�����������

                //Debug.Log("�~�߂�" + off_data.ToString("X") + "�ŗL�ԍ�" + notenumber);
                nowOn = false;
                //nowVolume = 0;

            }
        }

        //NotePlayer.midiOutShortMsg(hMidiOut, off_data);
        //Debug.Log("�~�߂�" + off_data.ToString("X") + "�ŗL�ԍ�" + dandelionManagement.nowNotenumber);
        //Debug.Log("�~�߂�")

    }

    public void ExpressionChange(uint Volume)
    {
        nowVolume = Volume;
        uint exppression = 0xB0;
        uint byte2 = 0x0b;
        uint expression_data = (exppression << 0) + (byte2 << 8) + (Volume << 16);
        ///Debug.Log("�ς���");
        TutoNotePlayer.midiOutShortMsg(hMidiOut, expression_data);
        //Debug.Log("����2" +"/"+ Volume);
        //Debug.Log("�ς���" + expression_data.ToString("X"));
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
        //��MIDI�f�o�C�X�J��
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
