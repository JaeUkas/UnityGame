using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject Gm;
    public GameObject Gm1;
    public GameObject Bullet;

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
                if (Gm != null)
                {
                    var newRotation = Quaternion.LookRotation(Gm.transform.position - transform.position).eulerAngles;
                    newRotation.x = 0;
                    newRotation.z = 0;
                    transform.rotation = Quaternion.Euler(newRotation);
                    //복제한다. //'Bullet'을 'FirePos.transform.position' 위치에 'FirePos.transform.rotation' 회전값으로.
                    Instantiate(Bullet, transform.position, transform.rotation);
                }
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
            timer = 0;
        }
       
    }

    
}