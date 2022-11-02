using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private int weaponImageIndex;
    [SerializeField] private Texture2D cursorImage;
    [SerializeField] private Texture2D[] weaponImages;

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
    }

    public void SwapMouseCursor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || GlobalVars.isPaused || GlobalVars.waveEnded)
        {
            GlobalVars.weaponIsSelected = false;
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);

            if (GlobalVars.waveEnded)
            {
                GlobalVars.waveEnded = false;
            }
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
                    ChangeWeaponStats(6, 1, 0.9f, 0.1f, "Neutral", 0, true, 0);
                    break;
                case "ShortSword":
                    ChangeWeaponStats(8, 1, 1.2f, 0.1f, "Neutral", 0, true, 1);
                    break;
                case "Spear":
                    ChangeWeaponStats(10, 1, 1.5f, 0.1f, "Neutral", 0, true, 3);
                    break;
                case "HandAxe":
                    ChangeWeaponStats(8, 1, 0.9f, 0.1f, "Neutral", 0, true, 4);
                    break;
                case "Mace":
                    ChangeWeaponStats(12, 1, 1.4f, 0.1f, "Neutral", 0, true, 5);
                    break;
                case "LongSword":
                    ChangeWeaponStats(8, 0, 2.5f, 0.2f, "Neutral", 0, true, 2);
                    break;

                //Lvl 2 Weapons
                case "SilverShortSword":
                    ChangeWeaponStats(16, 1, 1.2f, 0.15f, "Neutral", 0, true, 6);
                    break;
                case "CharredDagger":
                    ChangeWeaponStats(12, 1, 0.9f, 0.15f, "Fire", 0, true, 7);
                    break;
                case "FrostWand":
                    ChangeWeaponStats(8, 0, 3f, 0.3f, "Ice", 0.05f, false, 8);
                    break;
                case "JoltSabre":
                    ChangeWeaponStats(18, 1, 1.4f, 0.15f, "Thunder", 0, true, 9);
                    break;
                case "DivineHammer":
                    ChangeWeaponStats(22, 1, 1.8f, 0.15f, "Holy", 0, true, 10);
                    break;
                case "TwinDaggers":
                    ChangeWeaponStats(12, 1, 0.7f, 0.15f, "Swift", 0, true, 11);
                    break;
                case "CosmicSpear":
                    ChangeWeaponStats(20, 1, 1.5f, 0.15f, "Cosmic", 0, true, 12);
                    break;

                //Lvl 3 Weapons
                case "GoldenShortSword":
                    ChangeWeaponStats(30, 1, 1.2f, 0.15f, "Neutral", 0, true, 13);
                    break;
                case "EmberBattleAxe":
                    ChangeWeaponStats(20, 0, 2.5f, 0.3f, "Fire", 0, true, 14);
                    break;
                case "FrostLongSword":
                    ChangeWeaponStats(18, 0, 2.5f, 0.3f, "Ice", 0.10f, true, 15);
                    break;
                case "ShockLance":
                    ChangeWeaponStats(34, 1, 1.5f, 0.15f, "Thunder", 0, true, 16);
                    break;
                case "SacredStaff":
                    ChangeWeaponStats(22, 0, 2.5f, 0.4f, "Holy", 0, false, 17);
                    break;
                case "Katana":
                    ChangeWeaponStats(16, 0, 1f, 0.4f, "Swift", 0, true, 18);
                    break;
                case "AstralGreatSword":
                    ChangeWeaponStats(54, 1, 2f, 0.15f, "Cosmic", 0, true, 19);
                    break;

                //Lvl 4 Weapons

            }

            if (GlobalVars.weaponIsSelected)
            {
                Cursor.SetCursor(weaponImages[weaponImageIndex], Vector2.zero, CursorMode.ForceSoftware);
            }
        }
    }


    public void ChangeWeaponStats(float damage, float projectileSpeed, float attackSpeed, float attackRange, string damageType, float slowAmt, bool useSlashAnim, int weaponIndex)
    {
        weaponScript.startingDamage = damage;
        weaponScript.bonusDamage = 0;
        weaponScript.projectileSpeed = projectileSpeed;
        weaponScript.startingSpeed = attackSpeed;
        weaponScript.startingAttackRange = attackRange;
        weaponScript.damageType = damageType;
        weaponScript.startingSlowAmt = slowAmt;
        GlobalVars.useSlashAnim = useSlashAnim;
        weaponImageIndex = weaponIndex;
        playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[weaponIndex + 1];
        GlobalVars.triggerWeaponHudStatsUpdate = true;
    }
}
