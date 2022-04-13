using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject lastSelected = null;
    [SerializeField] private string cardName;
    [SerializeField] private bool mouseHover;
    [SerializeField] private bool expanded;
    [SerializeField] private RectTransform cardPos;
    [SerializeField] private RectTransform pos1;
    [SerializeField] private RectTransform pos2;

    void Start ()
    {
        //Set the initial gold cost value
        SetGoldCost();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Card.IsHoveringOverUiCard = true;
        }

        else
        {
            Card.IsHoveringOverUiCard = false;
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
            SetGoldCost();
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

    public void SetGoldCost()
    {
        switch (TowerGrid.towerTypeSelected)
        {
            case "Neutral":
                TowerGrid.goldCost = 75;
                break;

            case "Fire":
                TowerGrid.goldCost = 125;
                break;

            case "Ice":
                TowerGrid.goldCost = 125;
                break;

            case "Thunder":
                TowerGrid.goldCost = 175;
                break;

            case "Holy":
                TowerGrid.goldCost = 175;
                break;

            case "Swift":
                TowerGrid.goldCost = 250;
                break;

            case "Cosmic":
                TowerGrid.goldCost = 350;
                break;
        }
    }
}
