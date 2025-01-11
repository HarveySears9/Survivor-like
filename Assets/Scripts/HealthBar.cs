using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider UI component

    // Set the maximum health value
    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; // Initialize the health bar to full
    }

    // Update the health value
    public void SetHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
