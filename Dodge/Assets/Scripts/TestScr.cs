using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScr : MonoBehaviour
{
    public GameObject Gm;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newRotation = Quaternion.LookRotation(Gm.transform.position - transform.position).eulerAngles;
        newRotation.x = 0;
        newRotation.z = 0;
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
