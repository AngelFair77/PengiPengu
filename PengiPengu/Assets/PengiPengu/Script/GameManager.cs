using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public bool hasPickaxe = false; // Oyunun başında kazmamız yok
    

    [Header("Oyun Kaynakları")]
    public int fishCount = 0; // Toplanan Balık
    
    public bool hasColdResist = false; // Soğuğa dayanıklılık alındı mı?

    [Header("Isı (Can) Sistemi")]
    public float currentHeat = 100f;
    public float maxHeat = 100f;
    public int healthUpgradeLevel = 0; // Seviye (0, 1, 2)

    [Header("Hız (Speed) Sistemi")]
    public int speedUpgradeLevel = 0; // Seviye (0, 1, 2)
    public float moveSpeed = 5f;      // Karakterin hızı

    [Header("Sahne Geçişleri")]
    // --- İŞTE EKSİK OLAN VE HATAYA SEBEP OLAN SATIR ---
    public string targetSpawnPoint; 
    // --------------------------------------------------
    void Update()
    {
        // Can azalma mantığı (Zaten varsa burayı kendine göre uyarla)
        if (currentHeat > 0)
        {
            // Zamanla azalma (Örnek: Saniyede 1 azalır)
            // currentHeat -= Time.deltaTime; 
            
            // Not: Senin kodunda canı düşmanlar veya ortam azaltıyorsa burayı boş bırakabilirsin.
        }
        
        // ÖLÜM KONTROLÜ
        if (currentHeat <= 0)
        {
            currentHeat = 0; // Eksiye düşmesin
            
            // Eğer CanvasManager varsa Kaybetme ekranını aç
            if (CanvasManager.instance != null)
            {
                // Sürekli çağırmasın diye bir kontrol koyabilirsin ama 
                // Time.timeScale = 0 olacağı için tek sefer çalışır.
                CanvasManager.instance.ShowGameOver();
            }
        }
    }
    void Awake()
    {
        // Singleton Yapısı
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}