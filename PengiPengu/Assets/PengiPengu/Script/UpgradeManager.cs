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
    // UPGRADE 1: CAN (ISI) SİSTEMİ (+ ATKI BURAYA BAĞLI)
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
    // UPGRADE 3: KAZMA (PICKAXE) SİSTEMİ
    // ---------------------------------------------------------
    [Header("Upgrade 3: Kazma (Pickaxe) Ayarları")]
    public Button pickaxeButton;       
    public TextMeshProUGUI pickaxeButtonText;
    public int pickaxeCost = 25; 

    private bool isMenuOpen = false;

    void Update()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        
        if (activeScene.Trim() != "Cave") 
        {
            if (isMenuOpen) CloseMenu();
            return; 
        }

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

    public void BuyHealthUpgrade()
    {
        int currentLevel = GameManager.instance.healthUpgradeLevel;

        if (currentLevel >= 3 || currentLevel >= healthCosts.Length) return;

        int cost = healthCosts[currentLevel];

        if (GameManager.instance.fishCount >= cost)
        {
            // Parayı düş
            GameManager.instance.fishCount -= cost;
            
            // Level'ı arttır (CharacterMovement bunu görüp atkıyı takacak)
            GameManager.instance.healthUpgradeLevel++;
            
            // Canı arttır
            GameManager.instance.maxHeat += heatIncreaseAmount;
            GameManager.instance.currentHeat = GameManager.instance.maxHeat; 

            UpdateHealthBarVisuals(); 
            UpdateAllUI();
            
            Debug.Log("Can Yükseltildi! (Level " + GameManager.instance.healthUpgradeLevel + ")");
        }
    }

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
        }
    }

    public void BuyPickaxeUpgrade()
    {
        if (GameManager.instance.hasPickaxe) return; 

        if (GameManager.instance.fishCount >= pickaxeCost)
        {
            GameManager.instance.fishCount -= pickaxeCost;
            GameManager.instance.hasPickaxe = true;

            if (CharacterMovement.instance != null)
                CharacterMovement.instance.Kazma = true;

            UpdateAllUI();
        }
    }

    // =========================================================
    // UI GÜNCELLEME
    // =========================================================

    void UpdateAllUI()
    {
        UpdateUI(); 
        if (CanvasManager.instance != null) CanvasManager.instance.UpdateFishUI(); 
    }

    void UpdateUI()
    {
        if (GameManager.instance == null) return;

        if (fishCountText != null)
            fishCountText.text = "Balık: " + GameManager.instance.fishCount;

        // --- HEALTH BUTONU ---
        if (healthButtonText != null && healthUpgradeButton != null)
        {
            int hpLevel = GameManager.instance.healthUpgradeLevel;
            if (hpLevel < 3 && hpLevel < healthCosts.Length)
            {
                // Kullanıcıya bilgi veriyoruz
                healthButtonText.text = "Isı Kapasitesi (+20)\n(Lv " + (hpLevel + 1) + ")\nFiyat: " + healthCosts[hpLevel];
                healthUpgradeButton.interactable = true;
            }
            else
            {
                healthButtonText.text = "Isı: MAX";
                healthUpgradeButton.interactable = false;
            }
        }

        // --- SPEED BUTONU ---
        if (speedButtonText != null && speedUpgradeButton != null)
        {
            int spdLevel = GameManager.instance.speedUpgradeLevel;
            if (spdLevel < 3 && spdLevel < speedCosts.Length)
            {
                speedButtonText.text = "Hız Arttır\n(Lv " + (spdLevel + 1) + ")\nFiyat: " + speedCosts[spdLevel];
                speedUpgradeButton.interactable = true;
            }
            else
            {
                speedButtonText.text = "Hız: MAX";
                speedUpgradeButton.interactable = false;
            }
        }

        // --- PICKAXE BUTONU ---
        if (pickaxeButtonText != null && pickaxeButton != null)
        {
            if (!GameManager.instance.hasPickaxe)
            {
                pickaxeButtonText.text = "Kazma Satın Al\nFiyat: " + pickaxeCost;
                pickaxeButton.interactable = true;
            }
            else
            {
                pickaxeButtonText.text = "Kazma: VAR";
                pickaxeButton.interactable = false;
            }
        }
    }

    void UpdateHealthBarVisuals()
    {
        GameObject sliderObj = GameObject.Find("HeatSlider");
        if (sliderObj != null)
        {
            Slider slider = sliderObj.GetComponent<Slider>();
            if(slider != null)
            {
                slider.maxValue = GameManager.instance.maxHeat;
                slider.value = GameManager.instance.currentHeat;
            }
        }
    }
}