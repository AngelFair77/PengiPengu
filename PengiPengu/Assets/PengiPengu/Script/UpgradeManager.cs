using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Sahne ismini okumak için
using TMPro; // TextMeshPro için

public class UpgradeManager : MonoBehaviour
{
    [Header("UI Bağlantıları")]
    public GameObject upgradeMenuPanel;   // Canvas içindeki Upgrade Panelimiz
    public TextMeshProUGUI fishCountText; // Paneldeki "Balık: 5" yazısı

    [Header("Upgrade 1: Max Isı (Can) Ayarları")]
    public Button healthUpgradeButton;       
    public TextMeshProUGUI healthButtonText; 
    
    // Fiyatlar: 1. seviye 1 balık, 2. seviye 5 balık, 3. seviye 10 balık
    public int[] healthCosts = { 1, 5, 10 }; 
    public float heatIncreaseAmount = 20f; 

    private bool isMenuOpen = false;

    void Update()
    {
        // 1. ÖNCE SAHNE KONTROLÜ
        // Eğer şu anki sahne "Cave" (veya senin mağara sahnenin adı neyse) DEĞİLSE
        string activeScene = SceneManager.GetActiveScene().name;
        
        // Buraya kendi mağara sahnenin tam adını yaz: "CaveScene" vb.
        if (activeScene != "Cave") 
        {
            // Eğer yanlışlıkla menü açık kaldıysa kapat ve fonksiyonu bitir
            if (isMenuOpen) CloseMenu();
            return; 
        }

        // 2. SADECE MAĞARADAYSAK BURASI ÇALIŞIR
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }
    }

    void OpenMenu()
    {
        isMenuOpen = true;
        upgradeMenuPanel.SetActive(true); // Paneli görünür yap
        UpdateUI(); // Verileri yenile
        
        // Menü açılınca oyunu dondurmak istersen:
        // Time.timeScale = 0; 
    }

    void CloseMenu()
    {
        isMenuOpen = false;
        upgradeMenuPanel.SetActive(false); // Paneli gizle
        
        // Oyunu devam ettir:
        // Time.timeScale = 1;
    }

    public void BuyHealthUpgrade()
    {
        int currentLevel = GameManager.instance.healthUpgradeLevel;

        // Max seviye kontrolü
        if (currentLevel >= 3) 
        {
            Debug.Log("Zaten Max Seviye!");
            return;
        }

        // Dizi hatası olmaması için güvenlik kontrolü
        if (currentLevel >= healthCosts.Length)
        {
            Debug.LogError("HATA: healthCosts listesi Inspector'da eksik ayarlanmış! Listeyi kontrol et.");
            return;
        }

        int cost = healthCosts[currentLevel];

        // --- HATA AYIKLAMA (DEBUG) SATIRI ---
        Debug.Log($"Mevcut Balık: {GameManager.instance.fishCount} | İstenen Ücret: {cost} | Şu anki Seviye: {currentLevel}");
        // ------------------------------------

        if (GameManager.instance.fishCount >= cost)
        {
            GameManager.instance.fishCount -= cost;
            GameManager.instance.healthUpgradeLevel++;
            GameManager.instance.maxHeat += heatIncreaseAmount;
            GameManager.instance.currentHeat = GameManager.instance.maxHeat;

            UpdateHealthBarVisuals();
            UpdateUI();
            
            Debug.Log("Satın alma başarılı!");
        }
        if (CanvasManager.instance != null)
        {
            CanvasManager.instance.UpdateFishUI();
        }
        else
        {
            Debug.Log("Yetersiz Balık Uyarısı!");
        }
    }

    void UpdateUI()
    {
        // 1. Balık Sayısını GameManager'dan çekip yazdır
        if (fishCountText != null)
            fishCountText.text = "Balık: " + GameManager.instance.fishCount;

        // 2. Buton Durumunu Güncelle
        int currentLevel = GameManager.instance.healthUpgradeLevel;

        if (currentLevel < 3)
        {
            int cost = healthCosts[currentLevel];
            healthButtonText.text = "Isı Kapasitesi (+20)\n(Seviye " + (currentLevel + 1) + ")\nFiyat: " + cost + " Balık";
            healthUpgradeButton.interactable = true;
        }
        else
        {
            healthButtonText.text = "Isı Kapasitesi\nMAX SEVİYE";
            healthUpgradeButton.interactable = false;
        }
    }

    void UpdateHealthBarVisuals()
    {
        // HeatSlider da aynı Canvas'ın içinde olduğu için isminden bulabiliriz
        GameObject sliderObj = GameObject.Find("HeatSlider");
        if (sliderObj != null)
        {
            Slider slider = sliderObj.GetComponent<Slider>();
            slider.maxValue = GameManager.instance.maxHeat;
            slider.value = GameManager.instance.currentHeat;
        }
    }
}