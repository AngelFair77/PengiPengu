using UnityEngine;

public class FishCatch : MonoBehaviour
{
    [Header("Balık Yakalama Ayarları")]
    public float timeToCatchFish = 5f; 
    public int fishAmount = 1; 

    private float catchTimer = 0f;
    private bool playerInZone = false;
    private GameObject playerObject; 

    void Update()
    {
        if (playerInZone)
        {
            catchTimer += Time.deltaTime;

            if (catchTimer >= timeToCatchFish)
            {
                CatchFish();
                catchTimer = 0f; 
            }
        }
    }

    private void CatchFish()
    {
        // 1. Envantere ekle (Senin eski sistemin)
        if (playerObject != null)
        {
            PlayerInventory inventory = playerObject.GetComponent<PlayerInventory>();
            if (inventory != null) inventory.AddFish(fishAmount);
        }

        // 2. GameManager'a ekle (Upgrade sistemi için)
        if (GameManager.instance != null)
        {
            GameManager.instance.fishCount += fishAmount;
        }

        // --- 3. EKRANI GÜNCELLE (YENİ KISIM) ---
        // CanvasManager'a "Yazıyı yenile" komutu gönderiyoruz
        if (CanvasManager.instance != null)
        {
            CanvasManager.instance.UpdateFishUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            playerObject = other.gameObject;
            catchTimer = 0f; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            catchTimer = 0f;
            playerObject = null;
        }
    }
}