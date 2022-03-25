using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    public NotePlayer noteplayer;
    public DandelionManagement dandelionManagement;
    public GameLoop gameloop;

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
            dandelionManagement.SetTargetDandelion(collision.gameObject);
            float soundLength = collision.gameObject.GetComponent<NoteInfo>().soundLength;
            int i_pitch = collision.gameObject.GetComponent<NoteInfo>().pitch;
            uint pitch = (uint)i_pitch;
            //notePlayer.NoteOn(50, 100, 0);//テスト用

            StartCoroutine(StopNote(pitch, soundLength));
            
        }
    }

    IEnumerator StopNote(uint pitch,float delay)
    {
        //noteplayer.NoteOn(pitch, 100, 0);
        delay = delay;   // -0.1f;
        yield return new WaitForSeconds(delay);
        //noteplayer.NoteOff(pitch);

        //Debug.Log("コライダーが止めた");
        //Debug.Log()
        //Debug.Log(pitch);
    }

    
    void OnTriggerExit(Collider c)
    {
        if (c.gameObject == dandelionManagement.lastDandelion)
        {
            //Debug.Log("抜けた");
            //Destroy(c.gameObject);
            gameloop.ChangeToEndScene();

        }
        if (c.gameObject.tag == "Dandelion")
        {
            Destroy(c.gameObject);
        }
    }
    
}
