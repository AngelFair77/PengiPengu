using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Oyun Kaynakları")]
    public int fishCount = 0; // Toplanan Balık

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