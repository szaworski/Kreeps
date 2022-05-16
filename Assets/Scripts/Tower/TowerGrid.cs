using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpriteRenderer towerGhostSprite;
    [SerializeField] private Sprite[] ghostSpriteArray;
    [SerializeField] private GameObject placedTower;
    [SerializeField] private GameObject towerAttackRadius;
    [SerializeField] private GameObject towerStats;
    [SerializeField] private Tower towerScript;

    [Header("Grid position vars")]
    private bool hasTower;
    public static int goldCost;
    public static int upgradeGoldCost;
    public static string towerTypeSelected;

    [Header("Tower Upgrade vars")]
    public static GameObject oldTowerObj;
    public static GameObject gridObj;
    public static Vector3 upgradePosition;
    public static string upgradeCardSelected;
    public static string upgradeTypeSelected;
    public static bool upgradeCardsArePresent;
    public static bool triggerUpgradeCardDestruction;
    public static bool triggerTowerUpgrade;
    public static bool triggerTowerSell;
    public static GameObject upgradeCard1Obj;
    public static GameObject upgradeCard2Obj;
    public static GameObject upgradeCard3Obj;
    public static GameObject upgradeCard4Obj;
    public static GameObject upgradeCard5Obj;
    public static GameObject upgradeCard6Obj;

    [SerializeField] private string card1;
    [SerializeField] private string card2;
    [SerializeField] private string card3;

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

        UpgradeTower();
        SellTower();
    }

    void OnMouseOver()
    {
        //Check to make sure we aren't hovering over a UI element first
        if (!Card.IsHoveringOverUiCard)
        {
            //Reveal the grid sprite on mouse over
            SetSelectedTowerGhost();
            sprite.enabled = true;

            if (!hasTower)
            {
                towerGhostSprite.enabled = true;
            }
            //Debug.Log("Is hovering");

            if (Input.GetMouseButtonDown(0))
            {
                //Hide or show the tower stats on left mouse down
                HideShowTowerStats();

                //Spawn a tower on mouse click if one is not present
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
                    //Show or destroy the tower upgrade cards on right click
                    if (!upgradeCardsArePresent && towerScript.hasUpgrades)
                    {
                        SpawnTowerUpgradeCards();
                        upgradeCardsArePresent = true;
                    }

                    else if (upgradeCardsArePresent)
                    {
                        DestroyTowerUpgradeCards();
                        upgradeCardsArePresent = false;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    DestroyTower(placedTower);
                    hasTower = false;

                    //Give gold to the player (Give back a set amount based on the tower type)
                    switch (towerScript.damageType)
                    {
                        case var _ when towerScript.damageType.Contains("Neutral"):
                            PlayerHud.newGoldValue = PlayerHud.gold + 50;
                            break;

                        case var _ when towerScript.damageType.Contains("Fire"):
                            PlayerHud.newGoldValue = PlayerHud.gold + 100;
                            break;

                        case var _ when towerScript.damageType.Contains("Ice"):
                            PlayerHud.newGoldValue = PlayerHud.gold + 100;
                            break;

                        case var _ when towerScript.damageType.Contains("Thunder"):
                            PlayerHud.newGoldValue = PlayerHud.gold + 125;
                            break;

                        case var _ when towerScript.damageType.Contains("Holy"):
                            PlayerHud.newGoldValue = PlayerHud.gold + 125;
                            break;

                        case var _ when towerScript.damageType.Contains("Swift"):
                            PlayerHud.newGoldValue = PlayerHud.gold + 200;
                            break;

                        case var _ when towerScript.damageType.Contains("Cosmic"):
                            PlayerHud.newGoldValue = PlayerHud.gold + 250;
                            break;
                    }

                    if (upgradeCardsArePresent)
                    {
                        DestroyTowerUpgradeCards();
                        upgradeCardsArePresent = false;
                    }
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

    void SpawnTowerUpgradeCards()
    {
        //Get the towers position
        upgradePosition = placedTower.transform.position;
        //Get the current tower object
        oldTowerObj = placedTower;
        //Get the grid position object containing the tower
        gridObj = this.gameObject;
        //Debug.Log("Old Tower position: " + upgradePosition);
        //Debug.Log("Upgrade grid object: " + gridObj);

        card1 = towerScript.upgrade1;
        card2 = towerScript.upgrade2;
        card3 = towerScript.upgrade3;

        //Instantiate the Upgrade Cards
        GameObject cardSlot1 = GameObject.Find("UpgradeSlot1");
        GameObject cardSlot2 = GameObject.Find("UpgradeSlot2");
        GameObject cardSlot3 = GameObject.Find("UpgradeSlot3");
        GameObject cardSlot4 = GameObject.Find("CloseButton");
        GameObject cardSlot5 = GameObject.Find("UpgradeSign");
        GameObject cardSlot6 = GameObject.Find("SellButton");

        if (!string.IsNullOrEmpty(card1))
        {
            upgradeCard1Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/" + card1), cardSlot1.transform);
            upgradeCard1Obj.transform.position = cardSlot1.transform.position;
        }

        if (!string.IsNullOrEmpty(card2))
        {
            upgradeCard2Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/" + card2), cardSlot2.transform);
            upgradeCard2Obj.transform.position = cardSlot2.transform.position;
        }

        if (!string.IsNullOrEmpty(card3))
        {
            upgradeCard3Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/" + card3), cardSlot3.transform);
            upgradeCard3Obj.transform.position = cardSlot3.transform.position;
        }

        upgradeCard4Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/Close"), cardSlot4.transform);
        upgradeCard5Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/Upgrade"), cardSlot5.transform);
        upgradeCard6Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/SellButton"), cardSlot6.transform);
        upgradeCard4Obj.transform.position = cardSlot4.transform.position;
        upgradeCard5Obj.transform.position = cardSlot5.transform.position;
        upgradeCard6Obj.transform.position = cardSlot6.transform.position;
    }

    void UpgradeTower()
    {
        //Destory all card game objects after a selection is made. See Card.cs
        if (triggerTowerUpgrade)
        {
            SetSelectedUpgrade();
            DestroyTowerUpgradeCards();
            DestroyTower(oldTowerObj);
            //Reset bools for next upgrade card selection
            triggerTowerUpgrade = false;
            upgradeCardsArePresent = false;
        }

        if (triggerUpgradeCardDestruction)
        {
            DestroyTowerUpgradeCards();
            //Reset bools for next upgrade card selection
            triggerUpgradeCardDestruction = false;
            upgradeCardsArePresent = false;
        }
    }

    void SellTower()
    {
        if (triggerTowerSell)
        {
            TowerGrid gridScript = gridObj.GetComponent<TowerGrid>();
            gridScript.hasTower = false;

            DestroyTowerUpgradeCards();
            DestroyTower(oldTowerObj);

            //Give gold to the player (Give back a set amount based on the tower type)
            switch (gridScript.towerScript.damageType)
            {
                case var _ when gridScript.towerScript.damageType.Contains("Neutral"):
                    PlayerHud.newGoldValue = PlayerHud.gold + 50;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Fire"):
                    PlayerHud.newGoldValue = PlayerHud.gold + 100;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Ice"):
                    PlayerHud.newGoldValue = PlayerHud.gold + 100;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Thunder"):
                    PlayerHud.newGoldValue = PlayerHud.gold + 125;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Holy"):
                    PlayerHud.newGoldValue = PlayerHud.gold + 125;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Swift"):
                    PlayerHud.newGoldValue = PlayerHud.gold + 200;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Cosmic"):
                    PlayerHud.newGoldValue = PlayerHud.gold + 250;
                    break;
            }
            //Reset bools for next upgrade card selection
            triggerTowerSell = false;
            upgradeCardsArePresent = false;
        }
    }

    void SetSelectedUpgrade()
    {
        //Get the tower GameObject
        GameObject towerContainer = GameObject.Find("Towers");
        GameObject tower = (GameObject)Instantiate(Resources.Load("Towers/Upgrades/" + upgradeTypeSelected + "/" + upgradeCardSelected + "Tower"), towerContainer.transform);

        //Place the tower
        TowerGrid gridScript = gridObj.GetComponent<TowerGrid>();
        tower.transform.position = upgradePosition;
        gridScript.placedTower = tower;
        gridScript.hasTower = true;

        //Get the attack radius GameObject attached to the tower
        gridScript.towerAttackRadius = gridScript.placedTower.transform.GetChild(0).gameObject;
        gridScript.towerStats = gridScript.placedTower.transform.GetChild(1).gameObject;
        gridScript.towerScript = gridScript.placedTower.GetComponent<Tower>();
        gridScript.towerAttackRadius.SetActive(false);

        //Subtract gold from the player
        PlayerHud.newGoldValue = PlayerHud.gold - upgradeGoldCost;
    }

    void DestroyTowerUpgradeCards()
    {
        //Destory all upgrade card game objects
        if (upgradeCard1Obj != null)
        {
            Destroy(upgradeCard1Obj.gameObject);
        }

        if (upgradeCard2Obj != null)
        {
            Destroy(upgradeCard2Obj.gameObject);
        }

        if (upgradeCard3Obj != null)
        {
            Destroy(upgradeCard3Obj.gameObject);
        }

        if (upgradeCard4Obj != null)
        {
            Destroy(upgradeCard4Obj.gameObject);
        }

        if (upgradeCard5Obj != null)
        {
            Destroy(upgradeCard5Obj.gameObject);
        }

        if (upgradeCard6Obj != null)
        {
            Destroy(upgradeCard6Obj.gameObject);
        }
    }

    public void DestroyTower(GameObject towerObj)
    {
        //Destroy the tower
        Destroy(towerObj);
    }
}
