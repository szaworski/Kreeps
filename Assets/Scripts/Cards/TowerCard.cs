using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCard : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject lastSelected = null;
    public string cardName;

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Slide the card up
        }

        else
        {
            // Slide the card back down
        }

        KeepCardSelected();
    }

    public void KeepCardSelected()
    {
        // Keeps the current selected card highlighted when we click away
        if (eventSystem != null)
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                lastSelected = eventSystem.currentSelectedGameObject;
            }

            else
            {
                eventSystem.SetSelectedGameObject(lastSelected);
            }
        }
    }
}
