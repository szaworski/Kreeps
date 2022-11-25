using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject lastSelected = null;
    [SerializeField] private RectTransform cardPos;
    [SerializeField] private RectTransform pos1;
    [SerializeField] private RectTransform pos2;
    [SerializeField] private string cardName;
    [SerializeField] private bool mouseHover;
    [SerializeField] private bool expanded;

    void Start ()
    {
        //Set the initial gold cost value
        SetGoldCost();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GlobalVars.IsHoveringOverUiCard = true;
        }

        else
        {
            GlobalVars.IsHoveringOverUiCard = false;
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
        if (mouseHover && !expanded && Input.GetMouseButtonDown(1) && !GlobalVars.isPaused)
        {
            expanded = true;
        }

        else if (mouseHover && expanded && Input.GetMouseButtonDown(1) && !GlobalVars.isPaused || !mouseHover && !GlobalVars.isPaused)
        {
            expanded = false;
        }
    }
    public void SetSelectedTowerType()
    {
        if (mouseHover && Input.GetMouseButtonDown(0) && !GlobalVars.isPaused)
        {
            GlobalVars.towerTypeSelected = cardName;
            SetGoldCost();
        }
    }

    public void KeepCardHighlighted()
    {
        // Keeps the current selected card highlighted when we click away
        if (eventSystem != null)
        {
            if (eventSystem.currentSelectedGameObject != null && !GlobalVars.isPaused)
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
        switch (GlobalVars.towerTypeSelected)
        {
            case "Neutral":
                GlobalVars.goldCost = 60;
                break;

            case "Fire":
                GlobalVars.goldCost = 80;
                break;

            case "Ice":
                GlobalVars.goldCost = 80;
                break;

            case "Thunder":
                GlobalVars.goldCost = 100;
                break;

            case "Holy":
                GlobalVars.goldCost = 100;
                break;

            case "Swift":
                GlobalVars.goldCost = 150;
                break;

            case "Cosmic":
                GlobalVars.goldCost = 250;
                break;
        }
    }
}
