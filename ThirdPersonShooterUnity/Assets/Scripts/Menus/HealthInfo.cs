using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthInfo : MonoBehaviour
{
    public Text textHealth;         //  Updating text of the current health of the player
    public Slider health;           //  Health of the player

    public void HealthUpdate(float healthValue)
    {
        //  Update the current health of the player
        health.value = healthValue;
        textHealth.text = health.value.ToString();
    }

}
