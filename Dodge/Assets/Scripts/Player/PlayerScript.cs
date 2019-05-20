using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator animator;
    public Rigidbody rigidbody;

    private Transform tr;
    private PhotonView pv;

    private float h;
    private float v;

    private float moveX;
    private float moveZ;
    private float speedH = 4000f;
    private float speedZ = 4000f;   

    private Vector3 currPos;
    private Quaternion currRot;

    public int HP;

    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

        HP = 3;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if(pv.IsMine)
        {
            // 자신의 플레이어만 키보드 조작을 허용한다.
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            animator.SetFloat("h", h);
            animator.SetFloat("v", v);

            moveX = h * speedH * Time.deltaTime;
            moveZ = v * speedZ * Time.deltaTime;

            rigidbody.velocity = new Vector3(moveX, 0, moveZ);
            if (HP == 0) Destroy(gameObject);
        }
        else
        {
            //네트워크로 연결된 다른 유저일 경우에는 실시간 전송 받는 변수를 이용해 이동시켜준다.
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            //HP--;
            Destroy(other.gameObject);
            
        }
    }

     public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
