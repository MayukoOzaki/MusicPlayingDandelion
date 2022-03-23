using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionModelchange : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Camera;
    public GameObject HeadOutside;
    public GameObject HeadModel;
    public float DistanceCamera;
    bool Head=false;
    Transform Cameratransform;
    Transform Dandeliontransform;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //カメラの座標
        Cameratransform = Camera.transform;
        Vector3 Camerapos = Cameratransform.position;

        //たんぽぽの座標
        Dandeliontransform = this.transform;
        Vector3 Dandelionpos = Dandeliontransform.position;

        //カメラとたんぽぽ間の距離
        DistanceCamera = Dandelionpos.z - Camerapos.z;

        //距離が一定以上近くなったら簡易版のモデルを非表示にして、処理が重いモデルを表示する
        if (DistanceCamera < 30.0f)
        {
            if (Head==false)
            {
                HeadOutside.SetActive(false);
                HeadModel.SetActive(true);
                Head = true;
            }
           
        }
    }
}
