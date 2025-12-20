using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    // Heryerden erişilebilen tek kopya
    public static CanvasManager instance;
    public TextMeshProUGUI mainFishText;

    void Awake()
    {
        // Eğer sahnede henüz bir CanvasManager yoksa
        if (instance == null)
        {
            instance = this; // Patron benim
            DontDestroyOnLoad(gameObject); // Sahne değişince beni yok etme
        }
        else
        {
            // Eğer zaten bir CanvasManager varsa (diğer sahneden gelen), bu fazlalık demektir.
            Destroy(gameObject); // Fazlalığı yok et
        }
    }
    public void UpdateFishUI()
    {
        if (mainFishText != null && GameManager.instance != null)
        {
            // GameManager'daki güncel sayıyı alıp ekrana yazıyoruz
            mainFishText.text = "Balık: " + GameManager.instance.fishCount;
        }
    }
}