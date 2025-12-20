using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    [Header("Isı Ayarları")]
    public float maxHeat = 100f;
    public float currentHeat;

    [Header("Değişim Hızları")]
    public float outdoorDecayRate = 2f; 
    public float indoorRegenRate = 5f; 

    public Slider heatBarSlider;

    void Start()
    {
        // 1. Slider'ı isminden bul ve bağla
        GameObject sliderObj = GameObject.Find("HeatSlider");
        if (sliderObj != null)
        {
            heatBarSlider = sliderObj.GetComponent<Slider>();
            heatBarSlider.maxValue = maxHeat;
            
            // Veriyi GameManager'dan çek
            if (GameManager.instance != null)
                currentHeat = GameManager.instance.currentHeat;
            else
                currentHeat = maxHeat;

            heatBarSlider.value = currentHeat;
        }
        else
        {
            // Hata vermemesi için uyarıyı silebiliriz veya bırakabiliriz
            // Debug.LogError("HeatSlider bulunamadı!");
        }
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        // Sahne ismini al
        string activeScene = SceneManager.GetActiveScene().name;

        // --- MANTIK KISMI ---
        
        // 1. Durum: Eğer sahne "IcePlace" ise -> Can AZALSIN
        // (Buradaki "IcePlace" yazısının Unity'deki sahne adınla birebir aynı olduğundan emin ol)
        if (activeScene == "IcePlace") 
        {
            TakeHeatDamage(outdoorDecayRate * Time.deltaTime);
        }
        // 2. Durum: Eğer sahne "Cave" ise -> Can ARTSIN
        // (Buradaki "Cave" yazısını kendi mağara sahne adınla değiştir. Örn: "CaveScene")
        else if (activeScene == "Cave" || activeScene == "CaveScene") 
        {
            HealHeat(indoorRegenRate * Time.deltaTime);
        }
    }

    // --- HATAYI ÇÖZEN KISIM BURASI ---
    // CaveTrigger scripti bu fonksiyonu aradığı için ekledik.
    // Ancak içi boş, çünkü artık yukarıdaki Update fonksiyonu sahneye göre karar veriyor.
    public void SetCaveStatus(bool status)
    {
        // Bu fonksiyon artık devre dışı ama hatayı önlemek için burada duruyor.
    }
    // ----------------------------------

    public void TakeHeatDamage(float amount)
    {
        GameManager.instance.currentHeat -= amount;
        UpdateUI();
        
        if (GameManager.instance.currentHeat <= 0)
        {
            Debug.Log("Penguen Dondu!");
        }
    }

    public void HealHeat(float amount)
    {
        GameManager.instance.currentHeat += amount;
        GameManager.instance.currentHeat = Mathf.Clamp(GameManager.instance.currentHeat, 0, maxHeat); 
        UpdateUI();
    }

    void UpdateUI()
    {
        if (heatBarSlider != null)
        {
            heatBarSlider.value = GameManager.instance.currentHeat;
        }
    }
}