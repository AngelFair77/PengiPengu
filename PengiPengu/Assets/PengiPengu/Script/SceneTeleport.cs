using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    [Header("Gidilecek Yer Ayarları")]
    public string sceneToLoad;       // Hangi sahne açılacak? (Örn: Cave)
    public string spawnPointName;    // O sahnede hangi isimli objenin üzerinde doğacak?

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // GameManager'a "Şuraya gitmek istiyoruz" diye not bırakıyoruz
            if (GameManager.instance != null)
            {
                GameManager.instance.targetSpawnPoint = spawnPointName;
            }

            // Sahneyi yüklüyoruz
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}