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
        if (Input.GetKeyDown(KeyCode.Alpha1) || GlobalVars.isPaused || GameObject.Find("TileManager").transform.childCount == 0)
        {
            GlobalVars.weaponIsSelected = false;
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && GlobalVars.currentWeapon != "" && !GlobalVars.isPaused && GameObject.Find("TileManager").transform.childCount != 0)
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
                    ChangeWeaponStats(3, 1, 0.5f, 0.1f, "Neutral", 0, true, 0);
                    break;

                case "ShortSword":
                    ChangeWeaponStats(8, 1, 1f, 0.1f, "Neutral", 0, true, 1);
                    break;

                case "LongSword":
                    ChangeWeaponStats(10, 0, 2f, 0.2f, "Neutral", 0, true, 2);
                    break;

                case "Spear":
                    ChangeWeaponStats(15, 1, 1.25f, 0.1f, "Neutral", 0, true, 3);
                    break;

                case "HandAxe":
                    ChangeWeaponStats(8, 1, 0.6f, 0.1f, "Neutral", 0, true, 4);
                    break;

                case "Mace":
                    ChangeWeaponStats(12, 1, 1f, 0.1f, "Neutral", 0, true, 5);
                    break;

                //Lvl 2 Weapons
                case "SilverShortSword":
                    ChangeWeaponStats(16, 1, 0.9f, 0.15f, "Neutral", 0, true, 6);
                    break;
                case "CharredDagger":
                    ChangeWeaponStats(10, 1, 0.5f, 0.15f, "Fire", 0, true, 7);
                    break;
                case "FrostWand":
                    ChangeWeaponStats(8, 0, 1.25f, 0.2f, "Ice", 0.05f, false, 8);
                    break;
                case "JoltSabre":
                    ChangeWeaponStats(20, 1, 1f, 0.15f, "Thunder", 0, true, 9);
                    break;
                case "DivineHammer":
                    ChangeWeaponStats(30, 1, 1.25f, 0.15f, "Holy", 0, true, 10);
                    break;
                case "Katana":
                    ChangeWeaponStats(20, 1, 0.7f, 0.15f, "Swift", 0, true, 11);
                    break;
                case "AstralSpear":
                    ChangeWeaponStats(40, 1, 1.5f, 0.15f, "Cosmic", 0, true, 12);
                    break;

                    //Lvl 3 Weapons

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
        weaponScript.attackSpeed = attackSpeed;
        weaponScript.attackRange = attackRange;
        weaponScript.damageType = damageType;
        weaponScript.slowAmt = slowAmt;
        GlobalVars.useSlashAnim = useSlashAnim;
        weaponImageIndex = weaponIndex;
        playerHudScript.GetSetweaponHudImage.sprite = playerHudScript.GetWeaponHudImagesList[weaponIndex + 1];
    }
}
