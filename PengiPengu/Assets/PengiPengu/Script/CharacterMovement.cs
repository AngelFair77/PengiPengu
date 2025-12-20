using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private int Hiz = 5;
    private int MaxHealth;
    int currentHealth;
    private Rigidbody2D fizik;
    public Vector3 vec;

    void Start()
    {
        fizik = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        karakterHareket();
    }
    void karakterHareket()
    {
        float moveX = 0f;
        float moveY = 0f;
        
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
         if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1;
        }
        fizik.velocity = new Vector2(moveX * Hiz, moveY * Hiz);    }
}
