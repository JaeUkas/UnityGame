using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void Update()
    {
        //프레임마다 오브젝트를 로컬좌표상에서 앞으로 1의 힘만큼 날아가라
        transform.Translate(Vector3.forward * 150f*Time.deltaTime);
    }
}

