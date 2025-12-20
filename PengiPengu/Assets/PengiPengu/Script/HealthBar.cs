using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    
    [Header("Isı Ayarları")]
    public float maxHeat = 100f;
    public float currentHeat;

    [Header("Isı Değişim Hızları")]
    [Tooltip("Dışarıda saniyede kaybedilen ısı miktarı")]
    public float outdoorDecayRate = 2f; 
    [Tooltip("Mağarada saniyede kazanılan ısı miktarı")]
    public float indoorRegenRate = 5f; 
    
    [Header("UI Bağlantıları")]
    public Slider heatBarSlider; 
    
    // Isı Yönetimi Değişkenleri
    private bool isInsideCave = false; // Karakterin şu an nerede olduğunu tutar

    void Start()
    {
        currentHeat = maxHeat;
        
        // Slider başlangıç ayarları
        if (heatBarSlider != null)
        {
            heatBarSlider.maxValue = maxHeat;
            heatBarSlider.value = currentHeat;
        }
    }

    void Update()
    {
        // Isı Yönetimi
        if (isInsideCave)
        {
            // Mağaradayken ısıyı artır
            HealHeat(indoorRegenRate * Time.deltaTime);
        }
        else
        {
            // Dışarıdayken ısıyı azalt
            TakeHeatDamage(outdoorDecayRate * Time.deltaTime);
        }
    }

    // Isı kaybetme (Dışarıda durma veya kar topu hasarı)
    public void TakeHeatDamage(float amount)
    {
        currentHeat -= amount;
        UpdateHeatBar();

        if (currentHeat <= 0)
        {
            Die();
        }
    }

    // Isı kazanma (Mağarada durma)
    public void HealHeat(float amount)
    {
        currentHeat += amount;
        // Isının Max değeri geçmemesini sağla
        currentHeat = Mathf.Clamp(currentHeat, 0, maxHeat); 
        UpdateHeatBar();
    }

    void UpdateHeatBar()
    {
        if (heatBarSlider != null)
        {
            heatBarSlider.value = currentHeat;
        }
    }

    void Die()
    {
        Debug.Log("Penguen Dondu! Oyun Bitti.");
        // Gerekli oyun sonu işlemleri buraya eklenecek
    }

    // Mağara Girişi/Çıkışı Kontrolü için Harici Fonksiyon
    public void SetCaveStatus(bool isInside)
    {
        isInsideCave = isInside;
        Debug.Log("Penguen: " + (isInside ? "Mağaraya girdi." : "Dışarı çıktı."));
    }
    
}
