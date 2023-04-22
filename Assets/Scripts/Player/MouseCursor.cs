using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private int weaponImageIndex;
    [SerializeField] private Texture2D cursorImage;
    [SerializeField] private Texture2D[] weaponImages;
    [SerializeField] private Texture2D[] itemImages;

    GameObject playerWeapon;
    GameObject playerHud;
    Weapon weaponScript;
    PlayerHud playerHudScript;

    void Awake()
    {
        playerWeapon = GameObject.Find("PlayerWeapon");
        playerHud = GameObject.Find("PlayerHud");
        weaponScript = playerWeapon.GetComponent<Weapon>();
        playerHudScript = playerHud.GetComponent<PlayerHud>();

        GlobalVars.newWeapon = "";
        GlobalVars.weaponIsSelected = false;
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        SetNewWeapon();
        SwapMouseCursor();

        if (GlobalVars.waveEnded)
        {
            GlobalVars.waveEnded = false;
        }
    }

    public void SwapMouseCursor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || GlobalVars.isPaused)
        {
            GlobalVars.weaponIsSelected = false;
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && GlobalVars.currentWeapon != "" && !GlobalVars.isPaused)
        {
            GlobalVars.weaponIsSelected = true;
            Cursor.SetCursor(weaponImages[weaponImageIndex], Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void SetNewWeapon()
    {
        if (GlobalVars.currentWeapon != GlobalVars.newWeapon)
        {
            GlobalVars.currentWeapon = GlobalVars.newWeapon;

            switch (GlobalVars.currentWeapon)
            {
                //Lvl 1 Weapons
                case "Dagger":
                    ChangeWeaponStats(10, 1, 0.9f, 0.1f, "Neutral", 0, true, 0);
                    break;
                case "ShortSword":
                    ChangeWeaponStats(14, 1, 1.2f, 0.1f, "Fire", 0, true, 1);
                    break;
                case "LongSword":
                    ChangeWeaponStats(10, 0, 1.8f, 0.3f, "Cosmic", 0, true, 2);
                    break;
                case "Spear":
                    ChangeWeaponStats(16, 1, 1.5f, 0.1f, "Ice", 0.1f, true, 3);
                    break;
                case "HandAxe":
                    ChangeWeaponStats(8, 1, 0.7f, 0.1f, "Swift", 0, true, 4);
                    break;
                case "Mace":
                    ChangeWeaponStats(18, 1, 1.5f, 0.1f, "Thunder", 0, true, 5);
                    break;
                case "WalkingStick":
                    ChangeWeaponStats(12, 1, 1f, 0.1f, "Holy", 0, true, 6);
                    break;

                //Lvl 2 Weapons
                case "SilverShortSword":
                    ChangeWeaponStats(30, 1, 1.2f, 0.1f, "Neutral", 0, true, 7);
                    break;
                case "CharredDagger":
                    ChangeWeaponStats(20, 1, 1f, 0.1f, "Fire", 0, true, 8);
                    break;
                case "FrostWand":
                    ChangeWeaponStats(18, 0, 3f, 0.3f, "Ice", 0.1f, false, 9);
                    break;
                case "JoltSabre":
                    ChangeWeaponStats(32, 1, 1.4f, 0.1f, "Thunder", 0, true, 10);
                    break;
                case "DivineHammer":
                    ChangeWeaponStats(40, 1, 2f, 0.1f, "Holy", 0, true, 11);
                    break;
                case "TwinDaggers":
                    ChangeWeaponStats(16, 1, 0.6f, 0.1f, "Swift", 0, true, 12);
                    break;
                case "CosmicSpear":
                    ChangeWeaponStats(35, 1, 1.5f, 0.1f, "Cosmic", 0, true, 13);
                    break;

                //Lvl 3 Weapons
                case "GoldenShortSword":
                    ChangeWeaponStats(60, 1, 1.2f, 0.1f, "Neutral", 0, true, 14);
                    break;
                case "EmberBattleAxe":
                    ChangeWeaponStats(35, 0, 3f, 0.3f, "Fire", 0, true, 15);
                    break;
                case "FrostLongSword":
                    ChangeWeaponStats(70, 1, 1.4f, 0.1f, "Ice", 0.2f, true, 16);
                    break;
                case "ShockLance":
                    ChangeWeaponStats(75, 1, 1f, 0.1f, "Thunder", 0, true, 17);
                    break;
                case "SacredStaff":
                    ChangeWeaponStats(40, 0, 3f, 0.3f, "Holy", 0, false, 18);
                    break;
                case "Katana":
                    ChangeWeaponStats(20, 0, 2f, 0.3f, "Swift", 0, true, 19);
                    break;
                case "AstralGreatSword":
                    ChangeWeaponStats(90, 1, 1.5f, 0.1f, "Cosmic", 0, true, 20);
                    break;

                //Lvl 4 Weapons
                case "TitaniumShortSword":
                    ChangeWeaponStats(120, 1, 1f, 0.1f, "Neutral", 0, true, 21);
                    break;
                case "InfernalLongSword":
                    ChangeWeaponStats(120, 1, 0.8f, 0.1f, "Fire", 0, true, 22);
                    break;
                case "BlizzardBroadSword":
                    ChangeWeaponStats(80, 0, 2.5f, 0.3f, "Ice", 0.30f, true, 23);
                    break;
                case "LightningGreatAxe":
                    ChangeWeaponStats(280, 1, 1.5f, 0.1f, "Thunder", 0, true, 24);
                    break;
                case "HeavenlyGreatSword":
                    ChangeWeaponStats(100, 0, 2f, 0.4f, "Holy", 0, true, 25);
                    break;
                case "ReinforcedMorningStar":
                    ChangeWeaponStats(70, 1, 0.5f, 0.1f, "Swift", 0, true, 26);
                    break;
                case "GalacticScepter":
                    ChangeWeaponStats(120, 0, 2.5f, 0.4f, "Cosmic", 0, false, 27);
                    break;
            }

            if (GlobalVars.weaponIsSelected)
            {
                Cursor.SetCursor(weaponImages[weaponImageIndex], Vector2.zero, CursorMode.ForceSoftware);
            }
        }
    }


    public void ChangeWeaponStats(float damage, float projectileSpeed, float attackSpeed, float attackRange, string damageType, float slowAmt, bool useSlashAnim, int weaponIndex)
    {
        weaponScript.damageType = damageType;
        weaponScript.bonusStats[0] = 0;
        weaponScript.bonusStats[1] = 0;
        weaponScript.bonusStats[2] = 0;
        weaponScript.bonusStats[3] = 0;
        weaponScript.bonusStats[4] = 0;
        weaponScript.startingStats[0] = damage;
        weaponScript.startingStats[1] = attackSpeed;
        weaponScript.startingStats[2] = attackRange;
        weaponScript.startingStats[4] = slowAmt;
        weaponScript.projectileSpeed = projectileSpeed;
        GlobalVars.useSlashAnim = useSlashAnim;
        weaponImageIndex = weaponIndex;
        playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[weaponIndex + 1];
        GlobalVars.triggerWeaponHudStatsUpdate = true;
    }
}
