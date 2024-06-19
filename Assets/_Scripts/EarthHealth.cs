using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthHealth : MonoBehaviour
{
    int totalHealth = 0;
    int currentHealth = 0;

    public GameObject healthIndicator;


    public void IncreaseHealth(int amount)
    {
        totalHealth += amount;
        currentHealth += amount;
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        float width = healthIndicator.GetComponent<RectTransform>().rect.width;
        healthIndicator.transform.localScale = new Vector3((float)currentHealth / totalHealth, 1, 1);
        float newWidth = width * (float)currentHealth / totalHealth;
        healthIndicator.transform.localPosition = new Vector3(-46 + (width - newWidth) / 2, -18, 0);
    }
}
