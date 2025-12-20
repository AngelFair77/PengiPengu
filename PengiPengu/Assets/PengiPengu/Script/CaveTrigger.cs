using UnityEngine;

public class CaveTrigger : MonoBehaviour
{
    // Karakter bu alana girdiğinde
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Karakterin tag'i 'Player' ise
        if (other.CompareTag("Player"))
        {
            // Karakterin HeatBarManager script'ini al
            HealthBar heatManager = other.GetComponent<HealthBar>();
            if (heatManager != null)
            {
                heatManager.SetCaveStatus(true); // Mağarada: Isı artmaya başlasın
            }
        }
    }

    // Karakter bu alandan çıktığında
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthBar heatManager = other.GetComponent<HealthBar>();
            if (heatManager != null)
            {
                heatManager.SetCaveStatus(false); // Dışarıda: Isı azalmaya başlasın
            }
        }
    }
}