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
        //�J�����̍��W
        Cameratransform = Camera.transform;
        Vector3 Camerapos = Cameratransform.position;

        //����ۂۂ̍��W
        Dandeliontransform = this.transform;
        Vector3 Dandelionpos = Dandeliontransform.position;

        //�J�����Ƃ���ۂۊԂ̋���
        DistanceCamera = Dandelionpos.z - Camerapos.z;

        //���������ȏ�߂��Ȃ�����ȈՔł̃��f�����\���ɂ��āA�������d�����f����\������
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
