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
        healthIndicator.transform.localScale = new Vector3((float)currentHealth / totalHealth, 1, 1);
        //healthIndicator.transform.localPosition = new Vector3((1 - (float)currentHealth / totalHealth) / 2, 0, 0);
    }
}
