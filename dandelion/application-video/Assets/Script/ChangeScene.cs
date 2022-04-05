using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("Change", 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return)){
            SceneManager.LoadScene("SeedTest");
        }
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SceneManager.LoadScene("SeedTest");
        }

    }
    
    public void Change()
    {
        SceneManager.LoadScene("SeedTest");
    }
    
}
