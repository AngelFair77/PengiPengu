using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using UnityEngine.UI; // Slider için gerekli

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [Header("Paneller")]
    public GameObject gameOverPanel; 
    public GameObject winPanel;      
    public GameObject hudPanel;      

    [Header("UI Elemanları")]
    public TextMeshProUGUI fishText; // Balık sayısını gösteren text
    public Slider heatSlider;        // Isı (Can) Barı

    [Header("Sahne İsimleri")]
    public string mainMenuSceneName = "MainMenu"; 

    void Awake()
    {
        // Bu script HER SAHNEDE YENİDEN doğar (Singleton değildir)
        // O yüzden sadece instance ataması yapıyoruz.
        if (instance == null) instance = this;
    }

    void Start()
    {
        // --- SENKRONİZASYON (Sorunun Çözümü) ---
        // Yeni sahne açıldığında verileri GameManager'dan çekip ekrana yazdır.
        UpdateFishUI();
        UpdateHeatUI();
    }

    void Update()
    {
        // Isı sürekli değişebileceği için her karede barı güncellemek en garantisidir.
        UpdateHeatUI();
    }

    // --- GÜNCELLEME FONKSİYONLARI ---

    public void UpdateFishUI()
    {
        if (GameManager.instance != null && fishText != null)
        {
            fishText.text = "Balık: " + GameManager.instance.fishCount;
        }
    }

    public void UpdateHeatUI()
    {
        if (GameManager.instance != null && heatSlider != null)
        {
            // Slider'ın max değerini ve o anki değerini ayarla
            heatSlider.maxValue = GameManager.instance.maxHeat;
            heatSlider.value = GameManager.instance.currentHeat;
        }
    }

    // --- PANELLER VE BUTONLAR ---

    public void ShowGameOver()
    {
        Time.timeScale = 0; 
        if (hudPanel != null) hudPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void ShowWin()
    {
        Time.timeScale = 0;
        if (hudPanel != null) hudPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Ölünce canı fullemek gerekir, yoksa oyun açılır açılmaz tekrar ölürsün :)
        if (GameManager.instance != null)
        {
            GameManager.instance.currentHeat = GameManager.instance.maxHeat;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}