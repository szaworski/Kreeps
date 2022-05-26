using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static string currentWeapon;
    [SerializeField] private Texture2D cursorImage;
    [SerializeField] private Texture2D[] weaponImages;

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
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Cursor.SetCursor(weaponImages[0], Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
