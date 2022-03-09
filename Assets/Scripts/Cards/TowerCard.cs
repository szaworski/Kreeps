using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCard : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject lastSelected = null;
    public string cardName;
    public bool mouseExit;
    public RectTransform cardPos;
    public RectTransform pos1;
    public RectTransform pos2;

    void Update()
    {
        ReturnToStartPos();
        KeepCardSelected();
    }

    void OnMouseOver()
    {
        // Slide the card up
        cardPos.transform.position = Vector3.MoveTowards(cardPos.transform.position, pos2.transform.position, 5 * Time.deltaTime);

    }
    void OnMouseExit()
    {
        mouseExit = true;
    }

    public void ReturnToStartPos()
    {
        if (mouseExit)
        {
            // Slide the card back down
            cardPos.transform.position = Vector3.MoveTowards(cardPos.transform.position, pos1.transform.position, 5 * Time.deltaTime);

            if (cardPos.transform.position == pos1.transform.position)
            {
                mouseExit = false;
            }
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
