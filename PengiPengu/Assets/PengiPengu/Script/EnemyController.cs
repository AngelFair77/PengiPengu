using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 2f;
    public float patrolRadius = 5f;
    public float waitTime = 2f;

    [Header("Saldırı Ayarları")]
    public float attackRange = 4f;
    public GameObject projectilePrefab;
    public float attackRate = 1.5f;
    public float projectileSpeed = 8f;

    [Header("Ses Ayarları")] // YENİ: Ses için başlık
    public AudioClip throwSound; // YENİ: Editörden sürükleyeceğin ses dosyası

    // Özel Değişkenler
    private Transform player;
    private Vector2 startPoint;
    private Vector2 targetPoint;
    private float nextAttackTime;
    private float waitTimer;
    private bool isWaiting = false;

    private AudioSource audioSource; // YENİ: Sesi çalacak olan bileşen

    void Start()
    {
        // Oyuncuyu bul
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        startPoint = transform.position;
        PickRandomPoint();

        // YENİ: Objeye bağlı AudioSource bileşenini al
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackBehavior();
        }
        else
        {
            PatrolBehavior();
        }
    }

    void PatrolBehavior()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                PickRandomPoint();
            }
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            isWaiting = true;
            waitTimer = waitTime;
        }
    }

    void AttackBehavior()
    {
        if (Time.time >= nextAttackTime)
        {
            ShootProjectile();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void PickRandomPoint()
    {
        Vector2 randomDir = Random.insideUnitCircle * patrolRadius;
        targetPoint = startPoint + randomDir;
    }

    void ShootProjectile()
    {
        // 1. Mermiyi oluştur
        GameObject snowball = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        // 2. YENİ: SESİ ÇAL (PlayOneShot kullanımı)
        // Eğer ses dosyası ve AudioSource varsa sesi bir kez oynat
        if (audioSource != null && throwSound != null)
        {
            // PlayOneShot, sesler üst üste binse bile kesilmeden çalar.
            audioSource.PlayOneShot(throwSound);
        }

        // 3. Fırlatma fiziği
        Vector2 direction = (player.position - transform.position).normalized;
        Rigidbody2D rb = snowball.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            snowball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Vector3 center = Application.isPlaying ? (Vector3)startPoint : transform.position;
        Gizmos.DrawWireSphere(center, patrolRadius);
    }
}