using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Sahne ismini okumak için
using TMPro; // TextMeshPro için

public class UpgradeManager : MonoBehaviour
{
    [Header("Genel UI Bağlantıları")]
    public GameObject upgradeMenuPanel;   // Canvas içindeki Upgrade Paneli
    public TextMeshProUGUI fishCountText; // Paneldeki "Balık: 5" yazısı

    // ---------------------------------------------------------
    // UPGRADE 1: CAN (ISI) SİSTEMİ
    // ---------------------------------------------------------
    [Header("Upgrade 1: Max Isı (Can) Ayarları")]
    public Button healthUpgradeButton;       
    public TextMeshProUGUI healthButtonText; 
    
    public int[] healthCosts = { 1, 5, 10 }; // Fiyatlar
    public float heatIncreaseAmount = 20f;   // Her seviyede ne kadar artacak

    // ---------------------------------------------------------
    // UPGRADE 2: HIZ (SPEED) SİSTEMİ
    // ---------------------------------------------------------
    [Header("Upgrade 2: Hız (Speed) Ayarları")]
    public Button speedUpgradeButton;       
    public TextMeshProUGUI speedButtonText; 
    
    public int[] speedCosts = { 2, 6, 12 };  // Fiyatlar (İstediğin gibi değiştir)
    public float speedIncreaseAmount = 1f;   // Her seviyede hız ne kadar artacak

    private bool isMenuOpen = false;

    void Update()
    {
        // 1. SAHNE KONTROLÜ
        string activeScene = SceneManager.GetActiveScene().name;
        
        // Buraya mağara sahnenin adını yaz (Boşlukları temizleyerek kontrol eder)
        if (activeScene.Trim() != "Cave") 
        {
            if (isMenuOpen) CloseMenu();
            return; 
        }

        // 2. MENÜ AÇMA / KAPAMA
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
        upgradeMenuPanel.SetActive(true); 
        UpdateUI(); 
    }

    void CloseMenu()
    {
        isMenuOpen = false;
        upgradeMenuPanel.SetActive(false); 
    }

    // --- FONKSİYON 1: CAN YÜKSELTME ---
    public void BuyHealthUpgrade()
    {
        int currentLevel = GameManager.instance.healthUpgradeLevel;

        if (currentLevel >= 3) return; // Max seviye kontrolü

        // Dizi taşma kontrolü
        if (currentLevel >= healthCosts.Length) return;

        int cost = healthCosts[currentLevel];

        if (GameManager.instance.fishCount >= cost)
        {
            // Ödeme ve İşlem
            GameManager.instance.fishCount -= cost;
            GameManager.instance.healthUpgradeLevel++;
            GameManager.instance.maxHeat += heatIncreaseAmount;
            
            // Canı fulle
            GameManager.instance.currentHeat = GameManager.instance.maxHeat;

            // Görselleri Güncelle
            UpdateHealthBarVisuals(); // Can barını uzat
            UpdateUI(); // Menüdeki fiyatları yenile
            
            // Ana Ekrandaki Balık Sayısını Güncelle
            if (CanvasManager.instance != null)
                CanvasManager.instance.UpdateFishUI();
            
            Debug.Log("Can Yükseltildi!");
        }
        else
        {
            Debug.Log("Yetersiz Balık (Can Upgrade için)!");
        }
    }

    // --- FONKSİYON 2: HIZ YÜKSELTME ---
    public void BuySpeedUpgrade()
    {
        int currentLevel = GameManager.instance.speedUpgradeLevel;

        if (currentLevel >= 3) return; // Max seviye kontrolü

        // Dizi taşma kontrolü
        if (currentLevel >= speedCosts.Length) return;

        int cost = speedCosts[currentLevel];

        if (GameManager.instance.fishCount >= cost)
        {
            // Ödeme ve İşlem
            GameManager.instance.fishCount -= cost;
            GameManager.instance.speedUpgradeLevel++;
            GameManager.instance.moveSpeed += speedIncreaseAmount;

            // Görselleri Güncelle
            UpdateUI(); // Menüdeki fiyatları yenile

            // Ana Ekrandaki Balık Sayısını Güncelle
            if (CanvasManager.instance != null)
                CanvasManager.instance.UpdateFishUI();

            Debug.Log("Hız Yükseltildi! Yeni Hız: " + GameManager.instance.moveSpeed);
        }
        else
        {
            Debug.Log("Yetersiz Balık (Hız Upgrade için)!");
        }
    }

    // --- UI GÜNCELLEME ---
    void UpdateUI()
    {
        // Balık Sayısı
        if (fishCountText != null)
            fishCountText.text = "Balık: " + GameManager.instance.fishCount;

        // 1. CAN BUTONU DURUMU
        int hpLevel = GameManager.instance.healthUpgradeLevel;
        if (hpLevel < 3)
        {
            healthButtonText.text = "Isı Kapasitesi (+20)\n(Lv " + (hpLevel + 1) + ")\nFiyat: " + healthCosts[hpLevel];
            healthUpgradeButton.interactable = true;
        }
        else
        {
            healthButtonText.text = "Isı: MAX";
            healthUpgradeButton.interactable = false;
        }

        // 2. HIZ BUTONU DURUMU
        int spdLevel = GameManager.instance.speedUpgradeLevel;
        if (spdLevel < 3)
        {
            speedButtonText.text = "Hız Arttır (+" + speedIncreaseAmount + ")\n(Lv " + (spdLevel + 1) + ")\nFiyat: " + speedCosts[spdLevel];
            speedUpgradeButton.interactable = true;
        }
        else
        {
            speedButtonText.text = "Hız: MAX";
            speedUpgradeButton.interactable = false;
        }
    }

    // Can Barı Görselini Güncelleme Yardımcısı
    void UpdateHealthBarVisuals()
    {
        GameObject sliderObj = GameObject.Find("HeatSlider");
        if (sliderObj != null)
        {
            Slider slider = sliderObj.GetComponent<Slider>();
            slider.maxValue = GameManager.instance.maxHeat;
            slider.value = GameManager.instance.currentHeat;
        }
    }
}