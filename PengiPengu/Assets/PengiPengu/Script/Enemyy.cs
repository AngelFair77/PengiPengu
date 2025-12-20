using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyy : MonoBehaviour
{
    [Header("Saldırı Ayarları")]
    public GameObject projectilePrefab; // Fırlatılacak kar topu prefab'ı
    public float attackRate = 2f; // Saniyede kaç kez saldırı yapılacağı
    public float projectileSpeed = 10f;

    private float nextAttackTime;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nextAttackTime = Time.time;
    }

    void Update()
    {
        if (player != null && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void AttackPlayer()
    {
        // Penguenin pozisyonuna doğru yön hesapla
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Kar topunu oluştur
        GameObject snowball = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Kar topuna hız ver
        Rigidbody2D rb = snowball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }
}
