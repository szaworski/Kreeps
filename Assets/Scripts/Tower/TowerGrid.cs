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
    [SerializeField] private string card1;
    [SerializeField] private string card2;
    [SerializeField] private string card3;
    [SerializeField] private bool hasTower;

    void Awake()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        towerGhostSprite.enabled = false;
    }

    void Update()
    {
        //Check if the mouse is over any UI elements to disable other functionality underneath
        if (GlobalVars.IsHoveringOverUiCard)
        {
            sprite.enabled = false;
        }

        if (!GlobalVars.isPaused)
        {
            UpgradeTower();
            SellTower();
        }

        if (GlobalVars.isPaused || GlobalVars.weaponIsSelected)
        {
            HideGridSprites();
        }
    }

    void OnMouseOver()
    {
        //Check to make sure we aren't hovering over a UI element and that the game isn't paused 
        if (!GlobalVars.IsHoveringOverUiCard && !GlobalVars.isPaused && !GlobalVars.weaponIsSelected)
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
                if (!hasTower && GlobalVars.gold >= GlobalVars.goldCost && !GlobalVars.stoneIsSelected)
                {
                    //Get the tower GameObject
                    GameObject towerContainer = GameObject.Find("Towers");
                    GameObject tower = (GameObject)Instantiate(Resources.Load("Towers/" + GlobalVars.towerTypeSelected + "Tower"), towerContainer.transform);

                    //Place the tower
                    tower.transform.position = this.transform.position;
                    placedTower = tower;
                    hasTower = true;

                    //Get the attack radius GameObject attached to the tower
                    towerAttackRadius = placedTower.transform.GetChild(0).gameObject;
                    towerStats = placedTower.transform.GetChild(1).gameObject;
                    towerScript = placedTower.GetComponent<Tower>();

                    //Subtract gold from the player
                    GlobalVars.newGoldValue = GlobalVars.gold - GlobalVars.goldCost;
                    GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("PlaceTower");
                }

                else if (!hasTower && GlobalVars.gold < GlobalVars.goldCost || GlobalVars.stoneIsSelected)
                {
                    GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("Error");
                }
            }

            if (hasTower)
            {
                //Show the towers attack radius
                towerAttackRadius.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    if (towerScript.hasUpgrades)
                    {
                        GlobalVars.selectedTowerHasUpgrades = true;
                    }

                    if (GlobalVars.stoneIsSelected)
                    {
                        GlobalVars.stoneIsSelected = false;
                        GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("PowerUp");
                        towerScript.bonusDamage2 += GlobalVars.bonusStats["StonePwr"];
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    //Show or destroy the tower upgrade cards on right click
                    if (towerScript.hasUpgrades)
                    {
                        SpawnTowerUpgradeCards();
                        GlobalVars.upgradeCardsArePresent = true;
                    }
                }

                if (Input.GetMouseButtonDown(2))
                {
                    if (towerScript.hasRectangleRadius && GameObject.Find("TileManager").transform.childCount == 0)
                    {
                        towerScript.rectIsVertical = !towerScript.rectIsVertical;
                        towerScript.triggerRadiusFlip = true;
                    }
                    else
                    {
                        GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("Error");
                    }
                }

                //Sell the tower
                if (Input.GetKeyDown(KeyCode.Delete) && !GlobalVars.isPaused)
                {
                    DestroyTower(placedTower);
                    hasTower = false;
                    GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("SellTower");

                    //Give gold to the player (Give back a set amount based on the tower type)
                    switch (towerScript.damageType)
                    {
                        case var _ when towerScript.damageType.Contains("Neutral"):
                            GlobalVars.newGoldValue = GlobalVars.gold + 35;
                            break;

                        case var _ when towerScript.damageType.Contains("Fire"):
                            GlobalVars.newGoldValue = GlobalVars.gold + 60;
                            break;

                        case var _ when towerScript.damageType.Contains("Ice"):
                            GlobalVars.newGoldValue = GlobalVars.gold + 60;
                            break;

                        case var _ when towerScript.damageType.Contains("Thunder"):
                            GlobalVars.newGoldValue = GlobalVars.gold + 80;
                            break;

                        case var _ when towerScript.damageType.Contains("Holy"):
                            GlobalVars.newGoldValue = GlobalVars.gold + 80;
                            break;

                        case var _ when towerScript.damageType.Contains("Swift"):
                            GlobalVars.newGoldValue = GlobalVars.gold + 120;
                            break;

                        case var _ when towerScript.damageType.Contains("Cosmic"):
                            GlobalVars.newGoldValue = GlobalVars.gold + 200;
                            break;
                    }

                    if (GlobalVars.upgradeCardsArePresent)
                    {
                        DestroyTowerUpgradeCards();
                        GlobalVars.upgradeCardsArePresent = false;
                    }
                }
            }
        }
    }

    void OnMouseExit()
    {
        HideGridSprites();
    }

    void HideGridSprites()
    {
        //Hide the grid sprite on mouse exit 
        sprite.enabled = false;
        towerGhostSprite.enabled = false;

        if (hasTower)
        {
            //Hide the attack radius sprite on mouse exit 
            GlobalVars.IsHoveringOverTower = false;
            GlobalVars.selectedTowerHasUpgrades = false;
            towerAttackRadius.SetActive(false);
            towerStats.SetActive(false);
        }
    }

    void SetSelectedTowerGhost()
    {
        switch (GlobalVars.towerTypeSelected)
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
            GlobalVars.IsHoveringOverTower = true;
            towerStats.SetActive(true);
        }

        else if (hasTower && towerStats.activeSelf)
        {
            GlobalVars.IsHoveringOverTower = false;
            towerStats.SetActive(false);
        }
    }

    void SpawnTowerUpgradeCards()
    {
        if (GlobalVars.upgradeCardsArePresent)
        {
            DestroyTowerUpgradeCards();
            GlobalVars.upgradeCardsArePresent = false;
        }

        //Get the towers position
        GlobalVars.upgradePosition = placedTower.transform.position;
        //Get the current tower object
        GlobalVars.oldTowerObj = placedTower;
        //Get the grid position object containing the tower
        GlobalVars.gridObj = this.gameObject;

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
            GlobalVars.upgradeCard1Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/" + card1), cardSlot1.transform);
            GlobalVars.upgradeCard1Obj.transform.position = cardSlot1.transform.position;
        }

        if (!string.IsNullOrEmpty(card2))
        {
            GlobalVars.upgradeCard2Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/" + card2), cardSlot2.transform);
            GlobalVars.upgradeCard2Obj.transform.position = cardSlot2.transform.position;
        }

        if (!string.IsNullOrEmpty(card3))
        {
            GlobalVars.upgradeCard3Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/" + card3), cardSlot3.transform);
            GlobalVars.upgradeCard3Obj.transform.position = cardSlot3.transform.position;
        }

        GlobalVars.upgradeCard4Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/Close"), cardSlot4.transform);
        GlobalVars.upgradeCard5Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/Upgrade"), cardSlot5.transform);
        GlobalVars.upgradeCard6Obj = (GameObject)Instantiate(Resources.Load("UI/UpgradeCards/" + towerScript.damageType + "/SellButton"), cardSlot6.transform);
        GlobalVars.upgradeCard4Obj.transform.position = cardSlot4.transform.position;
        GlobalVars.upgradeCard5Obj.transform.position = cardSlot5.transform.position;
        GlobalVars.upgradeCard6Obj.transform.position = cardSlot6.transform.position;
    }

    void UpgradeTower()
    {
        //Destory all card game objects after a selection is made. See Card.cs
        if (GlobalVars.triggerTowerUpgrade)
        {
            TowerGrid gridScript = GlobalVars.gridObj.GetComponent<TowerGrid>();
            GlobalVars.selectedRectIsVertical = gridScript.towerScript.rectIsVertical;

            SetSelectedUpgrade();
            DestroyTowerUpgradeCards();
            DestroyTower(GlobalVars.oldTowerObj);
            //Reset bools for next upgrade card selection
            GlobalVars.triggerTowerUpgrade = false;
            GlobalVars.upgradeCardsArePresent = false;
        }

        if (GlobalVars.triggerUpgradeCardDestruction)
        {
            DestroyTowerUpgradeCards();
            //Reset bools for next upgrade card selection
            GlobalVars.triggerUpgradeCardDestruction = false;
            GlobalVars.upgradeCardsArePresent = false;
        }
    }

    void SellTower()
    {
        if (GlobalVars.triggerTowerSell)
        {
            TowerGrid gridScript = GlobalVars.gridObj.GetComponent<TowerGrid>();
            gridScript.hasTower = false;

            DestroyTowerUpgradeCards();
            DestroyTower(GlobalVars.oldTowerObj);
            GameObject.Find("UiSounds").GetComponent<AudioManager>().PlaySound("SellTower");

            //Give gold to the player (Give back a set amount based on the tower type)
            switch (gridScript.towerScript.damageType)
            {
                case var _ when gridScript.towerScript.damageType.Contains("Neutral"):
                    GlobalVars.newGoldValue = GlobalVars.gold + 40;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Fire"):
                    GlobalVars.newGoldValue = GlobalVars.gold + 60;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Ice"):
                    GlobalVars.newGoldValue = GlobalVars.gold + 60;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Thunder"):
                    GlobalVars.newGoldValue = GlobalVars.gold + 80;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Holy"):
                    GlobalVars.newGoldValue = GlobalVars.gold + 80;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Swift"):
                    GlobalVars.newGoldValue = GlobalVars.gold + 120;
                    break;

                case var _ when gridScript.towerScript.damageType.Contains("Cosmic"):
                    GlobalVars.newGoldValue = GlobalVars.gold + 200;
                    break;
            }
            //Reset bools for next upgrade card selection
            GlobalVars.triggerTowerSell = false;
            GlobalVars.upgradeCardsArePresent = false;
        }
    }

    void SetSelectedUpgrade()
    {
        //Get the tower GameObject
        GameObject towerContainer = GameObject.Find("Towers");
        GameObject tower = (GameObject)Instantiate(Resources.Load("Towers/Upgrades/" + GlobalVars.upgradeTypeSelected + "/" + GlobalVars.upgradeCardSelected + "Tower"), towerContainer.transform);

        //Place the tower
        TowerGrid gridScript = GlobalVars.gridObj.GetComponent<TowerGrid>();
        tower.transform.position = GlobalVars.upgradePosition;
        gridScript.placedTower = tower;
        gridScript.hasTower = true;
        GlobalVars.selectedRectIsVertical = true;

        //Get the attack radius GameObject attached to the tower
        gridScript.towerAttackRadius = gridScript.placedTower.transform.GetChild(0).gameObject;
        gridScript.towerStats = gridScript.placedTower.transform.GetChild(1).gameObject;
        gridScript.towerScript = gridScript.placedTower.GetComponent<Tower>();
        gridScript.towerAttackRadius.SetActive(false);

        //Subtract gold from the player
        GlobalVars.newGoldValue = GlobalVars.gold - GlobalVars.upgradeGoldCost;
    }

    void DestroyTowerUpgradeCards()
    {
        //Destory all upgrade card game objects
        if (GlobalVars.upgradeCard1Obj != null)
        {
            Destroy(GlobalVars.upgradeCard1Obj.gameObject);
        }

        if (GlobalVars.upgradeCard2Obj != null)
        {
            Destroy(GlobalVars.upgradeCard2Obj.gameObject);
        }

        if (GlobalVars.upgradeCard3Obj != null)
        {
            Destroy(GlobalVars.upgradeCard3Obj.gameObject);
        }

        if (GlobalVars.upgradeCard4Obj != null)
        {
            Destroy(GlobalVars.upgradeCard4Obj.gameObject);
        }

        if (GlobalVars.upgradeCard5Obj != null)
        {
            Destroy(GlobalVars.upgradeCard5Obj.gameObject);
        }

        if (GlobalVars.upgradeCard6Obj != null)
        {
            Destroy(GlobalVars.upgradeCard6Obj.gameObject);
        }
    }

    public void DestroyTower(GameObject towerObj)
    {
        Destroy(towerObj);
        GlobalVars.IsHoveringOverTower = false;
    }
}
