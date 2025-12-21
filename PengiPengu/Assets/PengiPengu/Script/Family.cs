using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Family : MonoBehaviour
{
    public float buz = 100f; // Buzu 100'den başlatıyoruz

    
    
    // OnTriggerStay2D: Obje alanın içinde kaldığı SÜRECE her karede çalışır
    void OnTriggerStay2D(Collider2D other)
    {
        // 1. Çarpan şey Player mı?
        if (other.CompareTag("Player"))
        {
            // 2. Player'ın içindeki CharacterMovement scriptine ulaş
            CharacterMovement playerScript = other.GetComponent<CharacterMovement>();

            // 3. Script bulunduysa VE Kazma özelliği açıksa
            if (playerScript != null && playerScript.Kazma == true)
            {
                // Saniyede 1 birim azalt (Time.deltaTime saniyeye yayar)
                buz -= Time.deltaTime * 5; // Hızlı denemek için *5 yaptım, istersen sil.
                
                // Konsola yazdıralım (Virgülden sonra 1 basamak görünsün diye "F1")
                Debug.Log("Buz Eriyor: " + buz.ToString("F1"));

                // 4. Buz biterse ne olsun?
                if (buz <= 0)
                {
                    Debug.Log("Aile Kurtarıldı!");
                    Destroy(gameObject); // Buz objesini yok et
                }
            }
        }
    }
}