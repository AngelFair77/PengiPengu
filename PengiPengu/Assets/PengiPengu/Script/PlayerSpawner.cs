using UnityEngine;
using UnityEngine.SceneManagement; // Sahne olaylarını dinlemek için şart

public class PlayerSpawner : MonoBehaviour
{
    // Script aktif olduğunda "Sahne Yüklendi" olayına abone oluyoruz
    void OnEnable()
    {
        SceneManager.sceneLoaded += SahneYuklendi;
    }

    // Script pasif olduğunda abonelikten çıkıyoruz (Hata almamak için)
    void OnDisable()
    {
        SceneManager.sceneLoaded -= SahneYuklendi;
    }

    // Bu fonksiyon her sahne değiştiğinde OTOMATİK çalışacak
    void SahneYuklendi(Scene scene, LoadSceneMode mode)
    {
        // GameManager var mı ve hedef nokta belli mi?
        if (GameManager.instance != null && !string.IsNullOrEmpty(GameManager.instance.targetSpawnPoint))
        {
            // Sahnedeki hedef noktayı isminden bul
            GameObject spawnPoint = GameObject.Find(GameManager.instance.targetSpawnPoint);

            if (spawnPoint != null)
            {
                // Karakteri ışınla
                transform.position = spawnPoint.transform.position;
                
                // Debug.Log("Karakter Işınlandı: " + GameManager.instance.targetSpawnPoint);
            }
            else
            {
                // Eğer nokta bulunamazsa konsola uyarı ver (İsim hatası yapmış olabilirsin)
                Debug.LogWarning("Spawn Noktası BULUNAMADI: " + GameManager.instance.targetSpawnPoint);
            }
        }
    }
}