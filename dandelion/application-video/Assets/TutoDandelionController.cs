using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoDandelionController : MonoBehaviour
{
    public GameObject topSeeds;
    public ParticleSystem ps;
    public GameObject headOutside;
    public bool isBlow = false;
    public GameObject particleObject;

    public ChangeScene changeScene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Blow(Vector3.forward);
        }
    }

    public void Blow(Vector3 dir)
    {
        if (isBlow == false)
        {
            StartCoroutine(RemoveTop());
            Vector3 target = topSeeds.transform.position + dir;
            target.y = topSeeds.transform.position.y;

            ps.transform.LookAt(target);
            ps.Play();
            isBlow = true;
            changeScene.Invoke("Change", 1.0f);

        }
        
    }

    IEnumerator RemoveTop()
    {
        yield return new WaitForSeconds(0.2f);
        topSeeds.SetActive(false);
        headOutside.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        //headOutside.SetActive(false);
        //gameObject.SetActive(false);
    }
}
