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
    public bool expanded;
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
        SlideActions();
        KeepCardHighlighted();
        SetSelectedTowerType();
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
        if (expanded)
        {
            cardPos.transform.position = Vector3.MoveTowards(cardPos.transform.position, pos2.transform.position, 10 * Time.deltaTime);
        }

        //Move the card down
        else if (!expanded)
        {
            cardPos.transform.position = Vector3.MoveTowards(cardPos.transform.position, pos1.transform.position, 10 * Time.deltaTime);
        }
    }

    public void SlideActions()
    {
        if (mouseHover && !expanded && Input.GetMouseButtonDown(1))
        {
            expanded = true;
        }

        else if (mouseHover && expanded && Input.GetMouseButtonDown(1) || !mouseHover)
        {
            expanded = false;
        }
    }
    public void SetSelectedTowerType()
    {
        if (mouseHover && Input.GetMouseButtonDown(0))
        {
            TowerGrid.towerTypeSelected = cardName;
        }
    }

    public void KeepCardHighlighted()
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
