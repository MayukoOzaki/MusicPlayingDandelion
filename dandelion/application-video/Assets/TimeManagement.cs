using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    public NotePlayer noteplayer;
    
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
            float soundLength = collision.gameObject.GetComponent<NoteInfo>().soundLength;
            int i_pitch = collision.gameObject.GetComponent<NoteInfo>().pitch;
            uint pitch = (uint)i_pitch;

            //StartCoroutine(StopNote(pitch, soundLength));
        }
    }   

    IEnumerator StopNote(uint pitch,float delay)
    {
        //noteplayer.NoteOn(pitch, 100, 0);
        yield return new WaitForSeconds(delay);
        noteplayer.NoteOff(pitch);
        //Debug.Log(pitch);
    }
}
