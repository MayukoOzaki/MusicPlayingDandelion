using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    public NotePlayer noteplayer;
    public DandelionManagement dandelionManagement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Dandelion")
        {
            dandelionManagement.SetCurrentDandelion(collision);

            float soundLength = collision.gameObject.GetComponent<NoteInfo>().soundLength;
            int i_pitch = collision.gameObject.GetComponent<NoteInfo>().pitch;
            uint pitch = (uint)i_pitch;
            //notePlayer.NoteOn(50, 100, 0);//テスト用

            StartCoroutine(StopNote(pitch, soundLength));
            dandelionManagement.SetTargetDandelion( collision.gameObject );
        }
    }

    IEnumerator StopNote(uint pitch,float delay)
    {
        //noteplayer.NoteOn(pitch, 100, 0);
        yield return new WaitForSeconds(delay);
        noteplayer.NoteOff(pitch);
        //Debug.Log("コライダーが止めた");
        //Debug.Log()
        //Debug.Log(pitch);
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Dandelion")
        {
            Debug.Log("抜けた");
            Destroy(c.gameObject);

        }
    }
}
