using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi şart!

public class PlayerInventory : MonoBehaviour
{
    [Header("Envanter Bilgisi")]
    public int currentFishCount = 0; // Mevcut balık sayısı

    [Header("UI Bağlantısı")]
    public TextMeshProUGUI fishText; // Ekrandaki yazı objesi

    void Start()
    {
        UpdateUI(); // Oyun başladığında yazıyı güncelle (0 ise 0 yazsın)
    }

    // Dışarıdan çağrılacak balık ekleme fonksiyonu
    public void AddFish(int amount)
    {
        currentFishCount += amount;
        UpdateUI(); // Sayı her değiştiğinde ekranı güncelle
    }

    // Yazıyı güncelleyen yardımcı fonksiyon
    void UpdateUI()
    {
        if (fishText != null)
        {
            fishText.text = "Balık: " + currentFishCount.ToString();
        }
    }
}