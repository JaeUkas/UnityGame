using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Fire : MonoBehaviourPunCallbacks
{
    public int HP;
    public GameObject Gm;
    public GameObject Gm1;

    public GameObject Em;
    public GameObject Em1;
    public GameObject Bullet;

    GameManager GM;
    
    float timer;
    float waitingTime;

    void Awake()
    {   GM = GameObject.Find("PhotonMgr").GetComponent<GameManager>();
     
        timer = 0f;
        waitingTime = 0.4f;
    }

    void Update()
    {
        if(Gm == null) Gm = GM.pClone;
        if(Gm1 == null) Gm1 = GM.pClone1;

        if (Em == null) Em = GM.eClone;
        if (Em1 == null) Em1 = GM.eClone1;

        timer += Time.deltaTime;

        if (timer > waitingTime)
        {
            //if (Input.GetMouseButtonDown(0))
            {
                if (gameObject == Em)
                {
                    if (Gm != null)
                    {
                        var newRotation = Quaternion.LookRotation(Gm.transform.position - transform.position).eulerAngles;
                        newRotation.x = 0;
                        newRotation.z = 0;
                        transform.rotation = Quaternion.Euler(newRotation);
                        //복제한다. //'Bullet'을 'FirePos.transform.position' 위치에 'FirePos.transform.rotation' 회전값으로.
                        Instantiate(Bullet, transform.position, transform.rotation);
                    }
                }
                if (gameObject == Em1)
                {
                    if (Gm1 != null)
                    {
                        var newRotation = Quaternion.LookRotation(Gm1.transform.position - transform.position).eulerAngles;
                        newRotation.x = 0;
                        newRotation.z = 0;
                        transform.rotation = Quaternion.Euler(newRotation);
                        //복제한다. //'Bullet'을 'FirePos.transform.position' 위치에 'FirePos.transform.rotation' 회전값으로.
                        Instantiate(Bullet, transform.position, transform.rotation);
                    }
                }
            }
            timer = 0;
        }
       
    }

 
}