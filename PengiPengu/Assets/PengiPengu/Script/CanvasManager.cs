using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Heryerden erişilebilen tek kopya
    public static CanvasManager instance;

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
}