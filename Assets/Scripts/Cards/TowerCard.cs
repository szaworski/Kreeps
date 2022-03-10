using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EventSystem eventSystem;
    public GameObject lastSelected = null;
    public string cardName;
    public bool mouseHover;
    public static bool IsHoveringOverTowerCard;
    public RectTransform cardPos;
    public RectTransform pos1;
    public RectTransform pos2;

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            IsHoveringOverTowerCard = true;
        }

        else
        {
            IsHoveringOverTowerCard = false;
        }

        MoveCard();
        KeepCardSelected();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHover = false;
    }

    public void MoveCard()
    {
        //Move the card up
        if (mouseHover)
        {
            cardPos.transform.position = Vector3.MoveTowards(cardPos.transform.position, pos2.transform.position, 10 * Time.deltaTime);
        }
        //Move the card down
        else
        {
            cardPos.transform.position = Vector3.MoveTowards(cardPos.transform.position, pos1.transform.position, 10 * Time.deltaTime);
        }
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
