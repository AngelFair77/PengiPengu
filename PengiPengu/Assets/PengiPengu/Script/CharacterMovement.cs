using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    private Rigidbody2D fizik;

    public bool Kazma = false;
    
    // --- BUZ KIRMA DEĞİŞKENLERİ ---
    public float buzCan = 100f; 
    private bool buzunUstunde = false; // Şu an buzun üstünde miyiz?
    private GameObject hedefBuzObjesi; // Hangi buzu kırıyoruz?
    // -----------------------------

    public GameObject atkiObjesi; // Editörden atkıyı buraya sürükleyeceğiz
    
    public GameObject kazmaObjesi; // YENİ: Kazma görselini buraya atacağız
    
    [Header("Ses Ayarları")]
    public AudioClip walkSound;   
    public AudioClip breakSound;  
    
    private AudioSource walkSource; 
    private AudioSource sfxSource;  

    private float nextBreakSoundTime = 0f;

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        fizik = GetComponent<Rigidbody2D>();
        
        // Ses Kaynaklarını Oluştur
        walkSource = gameObject.AddComponent<AudioSource>();
        walkSource.loop = true;          
        walkSource.playOnAwake = false;  
        walkSource.volume = 1f;          
        if (walkSound != null) walkSource.clip = walkSound;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;          
        sfxSource.playOnAwake = false;
        sfxSource.volume = 1f;

        if (GameManager.instance != null) Kazma = GameManager.instance.hasPickaxe;
    }

    void Update()
    {
        karakterHareket();
        BuzKirmaIslemi(); // Yeni fonksiyonu her kare çalıştırıyoruz
        AtkiKontrol();
        KazmaKontrol(); // Bunu eklemeyi unutma!
    }
    void AtkiKontrol()
    {
        // GameManager var mı ve Atkı objesi atandı mı?
        if (GameManager.instance != null && atkiObjesi != null)
        {
            // EĞER Health Upgrade Level 1 veya daha büyükse atkıyı göster
            bool atkisiOlsun = GameManager.instance.healthUpgradeLevel >= 1;
            
            atkiObjesi.SetActive(atkisiOlsun);
        }
    }
    void KazmaKontrol()
    {
        // GameManager var mı ve Kazma objesi atandı mı?
        if (GameManager.instance != null && kazmaObjesi != null)
        {
            // GameManager'da kazma alındıysa (hasPickaxe == true) objeyi aç
            kazmaObjesi.SetActive(GameManager.instance.hasPickaxe);
        }
    }
    void karakterHareket()
    {
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.A)) 
        {
            moveX = -1;
            transform.localScale = new Vector3(1, 1, 1); 
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            moveX = 1;
            transform.localScale = new Vector3(-1, 1, 1); 
        }
        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;
        
        if (fizik != null && GameManager.instance != null)
        {
            fizik.velocity = new Vector2(moveX * GameManager.instance.moveSpeed, moveY * GameManager.instance.moveSpeed);
        }

        // Yürüme Sesi
        bool hareketEdiyor = (moveX != 0 || moveY != 0);

        if (walkSource != null && walkSound != null)
        {
            if (hareketEdiyor)
            {
                if (!walkSource.isPlaying) walkSource.Play();
            }
            else
            {
                if (walkSource.isPlaying) walkSource.Stop();
            }
        }
    }

    // --- YENİ MANTIK: BUZ KIRMA ---
    // Bu fonksiyon Update içinde çağrıldığı için karakter dursa bile çalışır.
    void BuzKirmaIslemi()
    {
        if (buzunUstunde && Kazma && hedefBuzObjesi != null)
        {
            buzCan -= Time.deltaTime * 20; 
            
            // Ses çalma kısmı (Aynen kalıyor)
            if (Time.time >= nextBreakSoundTime && breakSound != null)
            {
                sfxSource.PlayOneShot(breakSound, 0.8f); 
                nextBreakSoundTime = Time.time + 0.4f; 
            }

            // --- DEĞİŞİKLİK BURADA ---
            if (buzCan <= 0)
            {
                Debug.Log("Aile Kurtarıldı - KAZANDIN!");
                
                Destroy(hedefBuzObjesi);
                hedefBuzObjesi = null;
                buzunUstunde = false;

                // Kazanma Ekranını Aç
                if (CanvasManager.instance != null)
                {
                    CanvasManager.instance.ShowWin();
                }
            }
        }
    }

    // --- TETİKLEYİCİLER ---
    // Artık OnTriggerStay yerine Enter ve Exit kullanıyoruz.
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // İçeri girince "Buzun üstündeyim" de
        if (other.CompareTag("Family"))
        {
            buzunUstunde = true;
            hedefBuzObjesi = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Dışarı çıkınca "Artık buzun üstünde değilim" de
        if (other.CompareTag("Family"))
        {
            buzunUstunde = false;
            hedefBuzObjesi = null;
        }
    }
}