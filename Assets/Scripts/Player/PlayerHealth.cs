using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text healthUiText;
    [SerializeField] private TMP_Text slashUiText;
    [SerializeField] private TMP_Text maxHealthUiText;
    public static int playerHealth;
    public static int newPlayerHealthValue;

    void Awake()
    {
        Color green = new Vector4(0, 0.7f, 0, 0.7f);

        playerHealth = 10;
        newPlayerHealthValue = playerHealth;
        healthUiText.SetText(playerHealth.ToString());
        slashUiText.SetText("/");
        maxHealthUiText.SetText(playerHealth.ToString());

        healthUiText.color = green;
        slashUiText.color = green;
        maxHealthUiText.color = green;
    }

    void Update()
    {
        if (newPlayerHealthValue > playerHealth || newPlayerHealthValue < playerHealth)
        {
            ChangeHealthValue();
        }
    }

    public void ChangeHealthValue()
    {
        Color green = new Vector4(0, 0.7f, 0, 0.7f);
        Color yellow = new Vector4(0.9f, 0.92f, 0.016f, 0.7f);
        Color red = new Vector4(0.8f, 0, 0, 0.7f);

        playerHealth = newPlayerHealthValue;
        healthUiText.SetText(playerHealth.ToString());

        if (playerHealth >= 7)
        {
           healthUiText.color = green;
           slashUiText.color = green;
           maxHealthUiText.color = green;
        }

        else if (playerHealth <= 6 && playerHealth > 3)
        {
            healthUiText.color = yellow;
            slashUiText.color = yellow;
            maxHealthUiText.color = yellow;
        }

        else if (playerHealth <= 3)
        {
            healthUiText.color = red;
            slashUiText.color = red;
            maxHealthUiText.color = red;
        }
    }
}
