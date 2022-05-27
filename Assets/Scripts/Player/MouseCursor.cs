using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorImage;
    [SerializeField] private Texture2D[] weaponImages;
    [SerializeField] private string currentWeapon;
    public static bool weaponIsSelected;
    public string GetSetCurrentWeapon
    {
        get { return currentWeapon; }
        set { currentWeapon = value; }
    }

    void Awake()
    {
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
    }


    void Update()
    {
        SwapMouseCursor();
    }

    public void SwapMouseCursor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponIsSelected = false;
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponIsSelected = true;
            Cursor.SetCursor(weaponImages[0], Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
