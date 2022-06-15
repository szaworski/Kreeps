using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text healthUiText;
    [SerializeField] private TMP_Text slashUiText;
    [SerializeField] private TMP_Text maxHealthUiText;

    void Awake()
    {
        Color green = new Vector4(0, 0.7f, 0, 0.7f);

        GlobalVars.playerHealth = 10;
        GlobalVars.newPlayerHealthValue = GlobalVars.playerHealth;
        healthUiText.SetText(GlobalVars.playerHealth.ToString());
        slashUiText.SetText("/");
        maxHealthUiText.SetText(GlobalVars.playerHealth.ToString());

        healthUiText.color = green;
        slashUiText.color = green;
        maxHealthUiText.color = green;
    }

    void Update()
    {
        if (GlobalVars.newPlayerHealthValue > GlobalVars.playerHealth || GlobalVars.newPlayerHealthValue < GlobalVars.playerHealth)
        {
            ChangeHealthValue();
        }
    }

    public void ChangeHealthValue()
    {
        Color green = new Vector4(0, 0.7f, 0, 0.7f);
        Color yellow = new Vector4(0.9f, 0.92f, 0.016f, 0.7f);
        Color red = new Vector4(0.8f, 0, 0, 0.7f);

        GlobalVars.playerHealth = GlobalVars.newPlayerHealthValue;
        healthUiText.SetText(GlobalVars.playerHealth.ToString());

        if (GlobalVars.playerHealth >= 7)
        {
           healthUiText.color = green;
           slashUiText.color = green;
           maxHealthUiText.color = green;
        }

        else if (GlobalVars.playerHealth <= 6 && GlobalVars.playerHealth > 3)
        {
            healthUiText.color = yellow;
            slashUiText.color = yellow;
            maxHealthUiText.color = yellow;
        }

        else if (GlobalVars.playerHealth <= 3)
        {
            healthUiText.color = red;
            slashUiText.color = red;
            maxHealthUiText.color = red;
        }
    }
}
