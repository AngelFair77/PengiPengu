using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float heatDamage = 20f; // Kar topunun düşüreceği ısı miktarı
    public float lifetime = 3f; // Merminin ne kadar süre sonra kaybolacağı

    void Start()
    {
        // Belirlenen süre sonunda mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kar topunun çarptığı objede Player tag'i varsa
        if (other.CompareTag("Player"))
        {
            HeatBarManager heatManager = other.GetComponent<HeatBarManager>();
            
            if (heatManager != null)
            {
                // Karakterin ısı değerini düşür
                heatManager.TakeHeatDamage(heatDamage);
            }
        }
        
        // Kar topunu çarptıktan sonra yok et
        Destroy(gameObject);
    }
}