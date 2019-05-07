﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript1 : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rigidbody;

    private float h;
    private float v;

    private float moveX;
    private float moveZ;
    private float speedH = 800f;
    private float speedZ = 800f;

    public int HP;

    void Awake()
    {
        HP = 3;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        animator.SetFloat("h", h);
        animator.SetFloat("v", v);

        moveX = h * speedH * Time.deltaTime;
        moveZ = v * speedZ * Time.deltaTime;

        rigidbody.velocity = new Vector3(moveX, 0, moveZ);

        if (HP == 0) Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            //HP--;
            Destroy(other.gameObject);
            
        }
    }
}