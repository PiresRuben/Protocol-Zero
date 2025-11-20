using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private PlayerHealth player;

    void OnEnable()
    {
            player.OnHealthChanged += UpdateHealthBar;
    }

    void OnDisable()
    {
            player.OnHealthChanged -= UpdateHealthBar;
    }

    void UpdateHealthBar(float normalizedValue)
    {
            fillImage.fillAmount = normalizedValue;
    }
}
