using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    // static referans: Diğer kodlardan da erişilebilir tek bir kopya
    public static CharacterMovement instance; 

    private int Hiz = 5;
    private Rigidbody2D fizik;
    
    // Awake, Start'tan önce çalışır. Bu kontrolü burada yapmak en sağlıklısıdır.
    void Awake()
    {
        // Eğer sahnede henüz bir instance (kopya) yoksa
        if (instance == null)
        {
            instance = this; // Bu objeyi ana kopya ilan et
            DontDestroyOnLoad(gameObject); // Ve bunu koru
        }
        else
        {
            // Eğer zaten bir instance varsa, demek ki bu obje fazlalık (yeni yüklenen sahneden gelen kopya)
            Destroy(gameObject); // Fazlalığı hemen yok et
        }
    }

    void Start()
    {
        fizik = GetComponent<Rigidbody2D>();
        // DontDestroyOnLoad buradaydı, sildik ve Awake içine taşıdık.
    }

    void Update()
    {
        karakterHareket();
    }

    void karakterHareket()
    {
        float moveX = 0f;
        float moveY = 0f;
        
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
        
        // Rigidbody null kontrolü eklemek her zaman iyidir (hata almamak için)
        if (fizik != null)
        {
            fizik.velocity = new Vector2(moveX * Hiz, moveY * Hiz);
        }
    }
}