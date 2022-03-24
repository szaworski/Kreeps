using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    //For whatever reason after changing the collider from a BoxCollider2D to a normal box collider, now it behaves how I want it to.
    public SpriteRenderer sprite;
    public SpriteRenderer towerGhostSprite;
    public Sprite[] ghostSpriteArray;
    public GameObject placedTower;
    public GameObject towerAttackRadius;
    public GameObject towerStats;
    public Tower towerScript;

    [Header("Grid position vars")]
    public bool hasTower;
    public static int goldCost;
    public static string towerTypeSelected;

    void Awake()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        towerGhostSprite.enabled = false;
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
            SetSelectedTowerGhost();
            sprite.enabled = true;
            towerGhostSprite.enabled = true;
            //Debug.Log("Is hovering");

            if (Input.GetMouseButtonDown(0))
            {
                //Hide or show the tower stats on left mouse down
                HideShowTowerStats();

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
                    //Get the attack radius GameObject attached to the tower
                    towerAttackRadius = placedTower.transform.GetChild(0).gameObject;
                    towerStats = placedTower.transform.GetChild(1).gameObject;
                    towerScript = placedTower.GetComponent<Tower>();

                    //Subtract gold from the player
                    PlayerHud.newGoldValue = PlayerHud.gold - goldCost;
                }
            }

            if (hasTower)
            {
                //Show the towers attack radius
                towerAttackRadius.SetActive(true);

                if (Input.GetMouseButtonDown(1))
                {
                    //Testing that we are able to retrieve values from this towers script  
                    if (towerScript.damageType == "Neutral")
                    {
                        Debug.Log("Deleted Neutral Tower");
                    }

                    DestroyTower();
                }
            }
        }
    }

    void OnMouseExit()
    {
        //Hide the grid sprite on mouse exit 
        sprite.enabled = false;
        towerGhostSprite.enabled = false;

        if (hasTower)
        {
            //Hide the attack radius sprite on mouse exit 
            towerAttackRadius.SetActive(false);
            towerStats.SetActive(false);
        }
        //Debug.Log("Not hovering");
    }

    void SetSelectedTowerGhost()
    {
        switch (towerTypeSelected)
        {
            case "Neutral":
                towerGhostSprite.sprite = ghostSpriteArray[0];
                break;

            case "Fire":
                towerGhostSprite.sprite = ghostSpriteArray[1];
                break;

            case "Ice":
                towerGhostSprite.sprite = ghostSpriteArray[2];
                break;

            case "Thunder":
                towerGhostSprite.sprite = ghostSpriteArray[3];
                break;

            case "Holy":
                towerGhostSprite.sprite = ghostSpriteArray[4];
                break;

            case "Swift":
                towerGhostSprite.sprite = ghostSpriteArray[5];
                break;

            case "Cosmic":
                towerGhostSprite.sprite = ghostSpriteArray[6];
                break;
        }
    }

    void HideShowTowerStats()
    {
        if (hasTower && !towerStats.activeSelf)
        {
            towerStats.SetActive(true);
        }

        else if (hasTower && towerStats.activeSelf)
        {
            towerStats.SetActive(false);
        }
    }

    void DestroyTower()
    {
        //Destroy the tower on right click (Sell the tower)
        Destroy(placedTower);
        hasTower = false;

        //Give gold to the player = to half of the original cost of the tower
        //PlayerHud.newGoldValue = PlayerHud.gold + goldCost;
    }
}
