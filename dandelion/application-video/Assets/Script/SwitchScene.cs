using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("Switch", 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            SceneManager.LoadScene("TitleScene");
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    /*
    void Switch()
    {
        SceneManager.LoadScene("TitleScene");
    }
    */
    
}
