using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    //For whatever reason after changing the collider from a BoxCollider2D to a normal box collider, now it behaves how I want it to.
    public SpriteRenderer sprite;
    public GameObject placedTower;

    [Header("Grid position vars")]
    public bool hasTower;
    public static int goldCost;
    public static string towerTypeSelected;

    void Awake()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        //Set the default selected tower type
        towerTypeSelected = "Neutral";
    }

    void Update()
    {
        //Check if the mouse is over any UI elements to disable other functionality underneath
        if (Card.IsHoveringOverUiCard)
        {
            sprite.enabled = false;
        }
    }

    void OnMouseOver()
    {
        //Check to make sure we aren't hovering over a UI element first
        if (!Card.IsHoveringOverUiCard)
        {
            //Reveal the grid sprite on mouse over
            sprite.enabled = true;
            //Debug.Log("Is hovering");

            if (Input.GetMouseButtonDown(0))
            {
                //Will eventually give a menu of tower choices, and check against a gold value to see if the tower can be placed or not
                //Spawning a tower on mouse click if one is not present to test for now
                if (!hasTower && PlayerHud.gold >= goldCost)
                {
                    //Get the tower GameObject
                    GameObject towerContainer = GameObject.Find("Towers");
                    GameObject tower = (GameObject)Instantiate(Resources.Load("Towers/" + towerTypeSelected + "Tower"), towerContainer.transform);

                    //Place the tower
                    tower.transform.position = this.transform.position;
                    placedTower = tower;
                    hasTower = true;

                    //Subtract gold from the player
                    PlayerHud.newGoldValue = PlayerHud.gold - goldCost;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //Destroy the tower on right click (Sell the tower)
                if (hasTower)
                {
                    Destroy(placedTower);
                    hasTower = false;
                }
            }
        }
    }

    void OnMouseExit()
    {
        //Hide the grid sprite when on mouse exit 
        sprite.enabled = false;
        //Debug.Log("Not hovering");
    }
}
