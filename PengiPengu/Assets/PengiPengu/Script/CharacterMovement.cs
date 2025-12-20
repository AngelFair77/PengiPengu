using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    private Rigidbody2D fizik;
    
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
}