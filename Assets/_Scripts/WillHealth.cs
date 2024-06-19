using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillHealth : MonoBehaviour
{
    public GameObject healthIndicator;

    public void SetHealth(int totalHealth, int currentHealth)
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        float width = healthIndicator.GetComponent<RectTransform>().rect.width;
        healthIndicator.transform.localScale = new Vector3((float)currentHealth / totalHealth, 1, 1);
        float newWidth = width * (float)currentHealth / totalHealth;
        // move to the left
        healthIndicator.transform.localPosition = new Vector3((newWidth - width) / 2, -18, 0);
    }
}
