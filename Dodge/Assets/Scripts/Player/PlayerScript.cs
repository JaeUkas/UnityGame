using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject Bullet;
    public GameObject HeartBullet;

    public Animator animator;
    public Rigidbody rigidbody;

    private Transform tr;
    private PhotonView pv;

    public GameObject healthBarBackground;
    public Image healthBarFilled;

    public GameObject firePos;

    private float h;
    private float v;

    private float moveX;
    private float moveZ;
    private float speedH = 4000f;
    private float speedZ = 4000f;   

    private Vector3 currPos;
    private Quaternion currRot;

    public float Hp;
    public float curruntHp;

    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

        Hp = 3;
        curruntHp = Hp;
        healthBarFilled.fillAmount = 1f;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
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
            //if (Hp == 0) Destroy(gameObject);

            if (Input.GetKeyDown(KeyCode.K))
            {
               photonView.RPC("InstantiateBullet", RpcTarget.All);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                photonView.RPC("InstantiateHeartBullet", RpcTarget.All);
            }
            
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
            if(curruntHp>0) curruntHp--;
            healthBarFilled.fillAmount = curruntHp / Hp;
            healthBarBackground.SetActive(true);
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 11)
        {
            if (curruntHp < 3) curruntHp++;
            healthBarFilled.fillAmount = curruntHp / Hp;
            healthBarBackground.SetActive(true);
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
    [PunRPC]
    public void InstantiateBullet()
    {
        Instantiate(Bullet, firePos.transform.position, firePos.transform.rotation);
    }

    [PunRPC]
    public void InstantiateHeartBullet()
    {
        Instantiate(HeartBullet, firePos.transform.position, firePos.transform.rotation);
    }


}
