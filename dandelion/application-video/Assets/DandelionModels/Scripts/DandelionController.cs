using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionController : MonoBehaviour
{
    public GameObject topSeeds;
    public ParticleSystem ps;
    public GameObject headOutside;
    public bool isBlow = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.B) )
        {
            Blow(Vector3.forward);
        }
    }

    public void Blow(Vector3 dir)
    {
        if (isBlow==false)
        {
            StartCoroutine(RemoveTop());
            Vector3 target = topSeeds.transform.position + dir;
            target.y = topSeeds.transform.position.y;

            ps.transform.LookAt(target);
            ps.Play();
            isBlow = true;
        }
        /*
        else
        {

        }
        */
        
        //Debug.Log("Ží‚ð”ò‚Î‚µ‚½");
    }

    IEnumerator RemoveTop()
    {
        yield return new WaitForSeconds(0.2f);
        topSeeds.SetActive(false);
        headOutside.SetActive(false);//ŽØ‚è
        yield return new WaitForSeconds(0.3f);
        //headOutside.SetActive(false);
        //gameObject.SetActive(false);
    }
}
