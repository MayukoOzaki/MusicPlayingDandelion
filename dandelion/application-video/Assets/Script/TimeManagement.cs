using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    public NotePlayer noteplayer;
    public DandelionManagement dandelionManagement;
    public GameLoop gameloop;

    public GameObject particleObject;
    // private int noteNumber;

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
            int noteNumber = collision.GetComponent<NoteInfo>().noteNumber;
            StartCoroutine(StopNote(pitch, soundLength,noteNumber));
            
        }
    }

    IEnumerator StopNote(uint pitch,float delay,int noteNumber)
    {
        //noteplayer.NoteOn(pitch, 100, 0);
        //delay = delay;   // -0.1f;
        yield return new WaitForSeconds(delay);
        noteplayer.NoteOff(pitch,noteNumber);

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
            GameObject camera = transform.root.gameObject;
            Vector3 velocity = new Vector3(0f, 0f, 0f);
            camera.GetComponent<Rigidbody>().velocity = velocity;

            GetComponent<AudioSource>().Play();

            Vector3 particlePos = transform.position;
            particlePos.y = 5.0f;

            Instantiate(particleObject, particlePos, Quaternion.identity);
            gameloop.Invoke("ChangeToEndScene", 5.0f);
            

            //gameloop.ChangeToEndScene();

        }
        if (c.gameObject.tag == "Dandelion")
        {
            Destroy(c.gameObject);
        }
    }
    
}
