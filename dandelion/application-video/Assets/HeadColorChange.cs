using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColorChange : MonoBehaviour
{
    

   // public float intensity = 10f;

    float alpha_Sin;
    public bool coroutineOn=false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;
        EmissionCoroutine();
    }

    IEnumerator EmissionCoroutine()
    {

        while (coroutineOn)
        {
            yield return new WaitForEndOfFrame();

            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            float factor = Mathf.Pow(2, alpha_Sin);
            GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1f * factor, 0.92f * factor, 0.016f * factor,1.0f*factor));

        }
    }
}
