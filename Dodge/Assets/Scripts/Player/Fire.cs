using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject Gm;
    public GameObject Gm2;
    public GameObject Bullet;
    public Transform FirePos;

    float timer;
    float waitingTime;

    private void Awake()
    {
        timer = 0f;
        waitingTime = 0.2f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime)
        {
            //if (Input.GetMouseButtonDown(0))
            {
                transform.LookAt(new Vector3(Gm.transform.position.x, Gm.transform.position.y + 5.8f, Gm.transform.position.z));
                //복제한다. //'Bullet'을 'FirePos.transform.position' 위치에 'FirePos.transform.rotation' 회전값으로.
                Instantiate(Bullet, FirePos.transform.position, FirePos.transform.rotation);

                transform.LookAt(new Vector3(Gm2.transform.position.x, Gm2.transform.position.y + 5.8f, Gm2.transform.position.z));
                //복제한다. //'Bullet'을 'FirePos.transform.position' 위치에 'FirePos.transform.rotation' 회전값으로.
                Instantiate(Bullet, FirePos.transform.position, FirePos.transform.rotation);
            }
            timer = 0;
        }
       
    }

    
}