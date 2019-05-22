using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 10)
        {
            Destroy(other.gameObject);
        }
    }
}
