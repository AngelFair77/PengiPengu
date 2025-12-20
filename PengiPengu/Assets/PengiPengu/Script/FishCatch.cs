using UnityEngine;

public class FishCatch : MonoBehaviour
{
    [Header("Balık Yakalama Ayarları")]
    [Tooltip("Balık yakalamak için alanda durulması gereken süre (saniye)")]
    public float timeToCatchFish = 5f; 
    public int fishAmount = 1; // Yakalanan balık miktarı

    private float catchTimer = 0f;
    private bool playerInZone = false;
    private GameObject playerObject; // Penguen objesini tutar

    void Update()
    {
        if (playerInZone)
        {
            catchTimer += Time.deltaTime;

            if (catchTimer >= timeToCatchFish)
            {
                CatchFish();
                catchTimer = 0f; // Yeni balık için sayacı sıfırla
            }
        }
    }

    private void CatchFish()
    {
        // TO-DO: Inventory/Güçlendirme sistemine balık ekle
        Debug.Log(fishAmount + " adet balık yakalandı! Güçlendirme için hazır.");
        
        // Bu alana balık yakalama efekti/sesi eklenebilir.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            playerObject = other.gameObject;
            catchTimer = 0f; // Alana girince sayacı sıfırla
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