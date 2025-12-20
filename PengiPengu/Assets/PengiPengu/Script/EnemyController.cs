using UnityEngine;
using System.Collections; // Coroutine kullanımı için gerekli

public class EnemyController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 2f;        // Düşmanın yürüme hızı
    public float patrolRadius = 5f;     // Başlangıç noktasından ne kadar uzağa gidebilir?
    public float waitTime = 2f;         // Bir noktaya varınca kaç saniye beklesin?

    [Header("Saldırı Ayarları")]
    public float attackRange = 4f;      // Oyuncuyu fark etme mesafesi
    public GameObject projectilePrefab; // Kar topu prefabı
    public float attackRate = 1.5f;     // Saniyede kaç atış?
    public float projectileSpeed = 8f;

    // Özel Değişkenler
    private Transform player;
    private Vector2 startPoint;         // Düşmanın ilk doğduğu yer (Merkez)
    private Vector2 targetPoint;        // Yürümek istediği rastgele nokta
    private float nextAttackTime;
    private float waitTimer;
    private bool isWaiting = false;

    void Start()
    {
        // Oyuncuyu bul
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Başlangıç noktasını kaydet (Düşman bu nokta etrafında devriye atar)
        startPoint = transform.position;
        
        // İlk rastgele hedefi belirle
        PickRandomPoint();
    }

    void Update()
    {
        if (player == null) return;

        // Oyuncu ile düşman arasındaki mesafeyi ölç
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // DURUM 1: Oyuncu menzilde -> Saldır!
            AttackBehavior();
        }
        else
        {
            // DURUM 2: Oyuncu uzakta -> Devriye Gez
            PatrolBehavior();
        }
    }

    void PatrolBehavior()
    {
        // Eğer bekleme modundaysak, sayacı çalıştır
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

        // Hedef noktaya doğru yürü
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        // Hedefe vardık mı? (Mesafe çok küçükse varmışızdır)
        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            // Vardık, şimdi bekleme moduna geç
            isWaiting = true;
            waitTimer = waitTime;
        }
    }

    void AttackBehavior()
    {
        // Saldırı anında hareket etmesin (İsterseniz burayı silebilirsiniz)
        // Sadece bekler ve ateş eder.

        if (Time.time >= nextAttackTime)
        {
            ShootProjectile();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void PickRandomPoint()
    {
        // Başlangıç noktası etrafında rastgele bir nokta bul (Daire içinde)
        Vector2 randomDir = Random.insideUnitCircle * patrolRadius;
        targetPoint = startPoint + randomDir;
    }

    void ShootProjectile()
    {
        // Mermiyi oluştur
        GameObject snowball = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        // Oyuncuya doğru yönü hesapla
        Vector2 direction = (player.position - transform.position).normalized;

        // Mermiye hız ver
        Rigidbody2D rb = snowball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
            
            // Merminin açısını hedefe göre döndür (Opsiyonel görsel güzellik)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            snowball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // Editörde alanları görebilmek için yardımcı çizimler
    void OnDrawGizmosSelected()
    {
        // Saldırı Menzili (Kırmızı)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Devriye Alanı (Yeşil) - Oyun çalışırken startPoint, çalışmıyorken transform.position
        Gizmos.color = Color.green;
        Vector3 center = Application.isPlaying ? (Vector3)startPoint : transform.position;
        Gizmos.DrawWireSphere(center, patrolRadius);
    }
}