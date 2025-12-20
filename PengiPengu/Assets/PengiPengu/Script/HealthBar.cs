using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    // ARTIK BURADA "maxHeat" DEĞİŞKENİ YOK!
    // Her şeyi GameManager'dan soracağız.
    
    public float outdoorDecayRate = 2f; 
    public float indoorRegenRate = 5f;  

    public Slider heatBarSlider;

    void Start()
    {
        GameObject sliderObj = GameObject.Find("HeatSlider");
        if (sliderObj != null)
        {
            heatBarSlider = sliderObj.GetComponent<Slider>();
            
            // Başlangıçta Slider'ın sınırını GameManager'dan al
            if (GameManager.instance != null)
            {
                heatBarSlider.maxValue = GameManager.instance.maxHeat;
                heatBarSlider.value = GameManager.instance.currentHeat;
            }
        }
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        string activeScene = SceneManager.GetActiveScene().name;

        // IcePlace -> Can Azalır
        if (activeScene == "IcePlace") 
        {
            TakeHeatDamage(outdoorDecayRate * Time.deltaTime);
        }
        // Cave -> Can Artar
        else if (activeScene == "Cave" || activeScene == "CaveScene") 
        {
            HealHeat(indoorRegenRate * Time.deltaTime);
        }
        
        // Slider'ın max değerini sürekli kontrol et (Upgrade anında yansıması için)
        if (heatBarSlider != null)
        {
             // Eğer upgrade yaptıysak slider'ın boyunu uzat
             if (heatBarSlider.maxValue != GameManager.instance.maxHeat)
             {
                 heatBarSlider.maxValue = GameManager.instance.maxHeat;
             }
        }
    }

    public void TakeHeatDamage(float amount)
    {
        GameManager.instance.currentHeat -= amount;
        UpdateUI();
        
        if (GameManager.instance.currentHeat <= 0)
        {
            Debug.Log("Öldün!");
            // Ölüm ekranı veya sahne yenileme kodu buraya...
        }
    }

    public void HealHeat(float amount)
    {
        GameManager.instance.currentHeat += amount;

        // --- EN ÖNEMLİ DEĞİŞİKLİK BURADA ---
        // Eskiden burada kendi "maxHeat" değişkenine bakıyordu (o da 100'dü).
        // Şimdi GameManager'ın "maxHeat" değişkenine bakıyor.
        GameManager.instance.currentHeat = Mathf.Clamp(
            GameManager.instance.currentHeat, 
            0, 
            GameManager.instance.maxHeat 
        ); 
        // -----------------------------------

        UpdateUI();
    }

    void UpdateUI()
    {
        if (heatBarSlider != null)
        {
            heatBarSlider.value = GameManager.instance.currentHeat;
        }
    }
    public void SetCaveStatus(bool status) 
    {
        // Burası bilerek boş bırakıldı.
    }
}