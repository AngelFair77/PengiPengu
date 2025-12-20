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
        // Karakterin üzerindeki PlayerInventory scriptine ulaşmaya çalış
        if (playerObject != null)
        {
            PlayerInventory inventory = playerObject.GetComponent<PlayerInventory>();
            
            if (inventory != null)
            {
                inventory.AddFish(fishAmount); // Envantere balığı ekle
                Debug.Log("Balık yakalandı! Toplam: " + inventory.currentFishCount);
            }
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