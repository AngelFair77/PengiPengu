using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Heryerden erişim için

    // --- BÖLÜM 1: Karakter Verileri (Isı ve Balık) ---
    [Header("Karakter Verileri")]
    public float currentHeat;      // Hata veren eksik değişken buydu!
    public float maxHeat = 100f;
    public int fishCount = 0;

    
    [Header("Upgrade Seviyeleri")]
    public int healthUpgradeLevel = 0; // 0: Hiç alınmadı, 1: Seviye 1, 2: Seviye 2, 3: Max
    
    // --- BÖLÜM 2: Işınlanma Verileri ---
    [Header("Işınlanma Hedefi")]
    public string targetSpawnPoint; // Gideceğimiz noktanın adı

    private void Awake()
    {
        // Singleton Yapısı (Bu objeden sadece 1 tane olsun)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişince silinmesin

            // Oyun ilk açıldığında ısıyı fulle
            currentHeat = maxHeat;
        }
        else
        {
            // Zaten bir GameManager varsa yenisini yok et
            Destroy(gameObject);
        }
    }
}