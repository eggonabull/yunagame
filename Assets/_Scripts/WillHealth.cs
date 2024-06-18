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
        healthIndicator.transform.localScale = new Vector3((float)currentHealth / totalHealth, 1, 1);
    }
}
