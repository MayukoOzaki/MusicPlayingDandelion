using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    BreathDetection BreScript;
    // Start is called before the first frame update
    void Start()
    {
        BreScript = GameObject.Find("BreathDitection").GetComponent<BreathDetection>();
        if (BreScript.breathflag)
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
