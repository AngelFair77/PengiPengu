using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro; 

public class UpgradeManager : MonoBehaviour
{
    [Header("Genel UI Bağlantıları")]
    public GameObject upgradeMenuPanel;   
    public TextMeshProUGUI fishCountText; 

    // ---------------------------------------------------------
    // UPGRADE 1: CAN (ISI) SİSTEMİ
    // ---------------------------------------------------------
    [Header("Upgrade 1: Max Isı (Can) Ayarları")]
    public Button healthUpgradeButton;       
    public TextMeshProUGUI healthButtonText; 
    
    public int[] healthCosts = { 1, 5, 10 }; 
    public float heatIncreaseAmount = 20f;   

    // ---------------------------------------------------------
    // UPGRADE 2: HIZ (SPEED) SİSTEMİ
    // ---------------------------------------------------------
    [Header("Upgrade 2: Hız (Speed) Ayarları")]
    public Button speedUpgradeButton;       
    public TextMeshProUGUI speedButtonText; 
    
    public int[] speedCosts = { 2, 6, 12 };  
    public float speedIncreaseAmount = 1f;   

    // ---------------------------------------------------------
    // UPGRADE 3: KAZMA (PICKAXE) SİSTEMİ (YENİ)
    // ---------------------------------------------------------
    [Header("Upgrade 3: Kazma (Pickaxe) Ayarları")]
    public Button pickaxeButton;       
    public TextMeshProUGUI pickaxeButtonText;
    public int pickaxeCost = 25; // Tek seferlik fiyat

    private bool isMenuOpen = false;

    void Update()
    {
        // 1. SAHNE KONTROLÜ
        string activeScene = SceneManager.GetActiveScene().name;
        
        // "Cave" sahnesinde değilsek menüyü kapat ve dur.
        if (activeScene.Trim() != "Cave") 
        {
            if (isMenuOpen) CloseMenu();
            return; 
        }

        // 2. MENÜ AÇMA / KAPAMA (E Tuşu)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isMenuOpen) CloseMenu();
            else OpenMenu();
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

    // =========================================================
    // SATIN ALMA FONKSİYONLARI
    // =========================================================

    // --- 1. CAN YÜKSELTME ---
    public void BuyHealthUpgrade()
    {
        int currentLevel = GameManager.instance.healthUpgradeLevel;

        if (currentLevel >= 3 || currentLevel >= healthCosts.Length) return;

        int cost = healthCosts[currentLevel];

        if (GameManager.instance.fishCount >= cost)
        {
            GameManager.instance.fishCount -= cost;
            GameManager.instance.healthUpgradeLevel++;
            GameManager.instance.maxHeat += heatIncreaseAmount;
            GameManager.instance.currentHeat = GameManager.instance.maxHeat; // Canı fulle

            UpdateHealthBarVisuals(); 
            UpdateAllUI();
            
            Debug.Log("Can Yükseltildi!");
        }
        else
        {
            Debug.Log("Yetersiz Balık (Can)!");
        }
    }

    // --- 2. HIZ YÜKSELTME ---
    public void BuySpeedUpgrade()
    {
        int currentLevel = GameManager.instance.speedUpgradeLevel;

        if (currentLevel >= 3 || currentLevel >= speedCosts.Length) return;

        int cost = speedCosts[currentLevel];

        if (GameManager.instance.fishCount >= cost)
        {
            GameManager.instance.fishCount -= cost;
            GameManager.instance.speedUpgradeLevel++;
            GameManager.instance.moveSpeed += speedIncreaseAmount;

            UpdateAllUI();
            Debug.Log("Hız Yükseltildi!");
        }
        else
        {
            Debug.Log("Yetersiz Balık (Hız)!");
        }
    }

    // --- 3. KAZMA SATIN ALMA (YENİ) ---
    public void BuyPickaxeUpgrade()
    {
        if (GameManager.instance.hasPickaxe) return; // Zaten varsa işlem yapma

        if (GameManager.instance.fishCount >= pickaxeCost)
        {
            GameManager.instance.fishCount -= pickaxeCost;
            
            // GameManager'a kaydet
            GameManager.instance.hasPickaxe = true;

            // Karakterdeki değişkeni güncelle
            if (CharacterMovement.instance != null)
                CharacterMovement.instance.Kazma = true;

            UpdateAllUI();
            Debug.Log("Kazma Satın Alındı!");
        }
        else
        {
            Debug.Log("Yetersiz Balık (Kazma)!");
        }
    }

    // =========================================================
    // UI GÜNCELLEME (GÜVENLİ MOD)
    // =========================================================

    void UpdateAllUI()
    {
        UpdateUI(); // Upgrade Paneli
        if (CanvasManager.instance != null) CanvasManager.instance.UpdateFishUI(); // Ana Ekran Balık Sayısı
    }

    void UpdateUI()
    {
        if (GameManager.instance == null) return;

        // --- BALIK SAYISI ---
        if (fishCountText != null)
            fishCountText.text = "Balık: " + GameManager.instance.fishCount;

        // --- CAN BUTONU ---
        if (healthButtonText != null && healthUpgradeButton != null)
        {
            int hpLevel = GameManager.instance.healthUpgradeLevel;
            if (hpLevel < 3 && hpLevel < healthCosts.Length)
            {
                healthButtonText.text = "Isı Kapasitesi (+20)\n(Lv " + (hpLevel + 1) + ")\nFiyat: " + healthCosts[hpLevel];
                healthUpgradeButton.interactable = true;
            }
            else
            {
                healthButtonText.text = "Isı: MAX";
                healthUpgradeButton.interactable = false;
            }
        }

        // --- HIZ BUTONU ---
        if (speedButtonText != null && speedUpgradeButton != null)
        {
            int spdLevel = GameManager.instance.speedUpgradeLevel;
            if (spdLevel < 3 && spdLevel < speedCosts.Length)
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

        // --- KAZMA BUTONU (YENİ) ---
        if (pickaxeButtonText != null && pickaxeButton != null)
        {
            bool hasIt = GameManager.instance.hasPickaxe;
            if (!hasIt)
            {
                pickaxeButtonText.text = "Kazma Satın Al\nFiyat: " + pickaxeCost;
                pickaxeButton.interactable = true;
            }
            else
            {
                pickaxeButtonText.text = "Kazma\n(SAHİPSİN)";
                pickaxeButton.interactable = false;
            }
        }
    }

    // Helper: Can barı görselini güncelleme
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