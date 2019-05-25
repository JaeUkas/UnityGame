using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    protected Joystick joystick;
    protected FireSkill fireSkill;
    protected HealSkill healSkill;

    public GameObject Bullet;
    public GameObject HeartBullet;

    public Animator animator;
    public Rigidbody rigidbody;

    private Transform tr;
    private PhotonView pv;

    public GameObject healthBarBackground;
    public Image healthBarFilled;

    public GameObject firePos;

    GameManager GM;

    private float h;
    private float v;

    private float moveX;
    private float moveZ;
    private float speedH = 3500f;
    private float speedZ = 3500f;   

    private Vector3 currPos;
    private Quaternion currRot;

    public float Hp;
    public float curruntHp;

    float skill1Timer;
    float skill1WaitingTime;

    float skill2Timer;
    float skill2WaitingTime;

   
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        fireSkill = FindObjectOfType<FireSkill>();
        healSkill = FindObjectOfType<HealSkill>();

        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        GM = GameObject.Find("PhotonMgr").GetComponent<GameManager>();

        pv.ObservedComponents[0] = this;

        Hp = 5;
        curruntHp = Hp;
        healthBarFilled.fillAmount = curruntHp / Hp;
        healthBarBackground.SetActive(true);
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        skill1Timer = 0f;
        skill1WaitingTime = 0.8f;

        skill2Timer = 0f;
        skill2WaitingTime = 0.8f;
    }
    void Update()
    {
        if(pv.IsMine)
        {
   
            skill1Timer += Time.deltaTime;
            skill2Timer += Time.deltaTime;

            h = joystick.Horizontal + (Input.GetAxis("Horizontal"));
            v = joystick.Vertical + (Input.GetAxis("Vertical"));
            animator.SetFloat("h", h);
            animator.SetFloat("v", v);

            moveX = h * speedH * Time.deltaTime;
            moveZ = v * speedZ * Time.deltaTime;

            rigidbody.velocity = new Vector3(moveX, 0, moveZ);

            if (curruntHp <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
                GM.playerDie++;
            }
            if (skill1Timer > skill1WaitingTime)
            {
                if (Input.GetKeyDown(KeyCode.K) || fireSkill.Pressed)
                {
                    photonView.RPC("InstantiateBullet", RpcTarget.All);
                    skill1Timer = 0;
                }
                
            }

            if (skill2Timer > skill2WaitingTime)
            {
                if (Input.GetKeyDown(KeyCode.J) || healSkill.Pressed)
                {
                    photonView.RPC("InstantiateHeartBullet", RpcTarget.All);
                    skill2Timer = 0;
                }
                
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
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 11)
        {
            if (curruntHp < 5) curruntHp++;
            healthBarFilled.fillAmount = curruntHp / Hp;
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
