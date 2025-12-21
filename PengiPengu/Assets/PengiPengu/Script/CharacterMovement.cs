using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    private Rigidbody2D fizik;

    public bool Kazma = false;
    
    public float buzCan = 100f; // Buzun canını burada tutuyoruz
    
    // ARTIK BURADAKİ "int Hiz = 5;" SATIRINI SİLİYORUZ VEYA KULLANMIYORUZ.

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        fizik = GetComponent<Rigidbody2D>();
        if (GameManager.instance != null)
        {
            Kazma = GameManager.instance.hasPickaxe;
        }
    }

    void Update()
    {
        karakterHareket();
    }

    void karakterHareket()
    {
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1;
        }
        
        if (fizik != null && GameManager.instance != null)
        {
            // DEĞİŞİKLİK BURADA:
            // "Hiz" yerine "GameManager.instance.moveSpeed" kullanıyoruz.
            fizik.velocity = new Vector2(moveX * GameManager.instance.moveSpeed, moveY * GameManager.instance.moveSpeed);
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        // Eğer çarptığımız objenin etiketi "Family" ise
        if (other.CompareTag("Family"))
        {
            // Ve elimizde Kazma varsa
            if (Kazma == true)
            {
                // Buzu azalt
                buzCan -= Time.deltaTime * 20; // Saniyede 20 azalır (Hızlı olsun diye)
                Debug.Log("Buz Kırılıyor: " + buzCan.ToString("F0"));

                if (buzCan <= 0)
                {
                    Debug.Log("Aile Kurtarıldı!");
                    
                    // Çarptığımız objeyi (Aileyi/Buzu) yok et
                    Destroy(other.gameObject);
                }
            }
            else
            {
                // Kazması yoksa uyarabilirsin (Opsiyonel)
                // Debug.Log("Bunu kırmak için KAZMA lazım!");
            }
        }
    }
}