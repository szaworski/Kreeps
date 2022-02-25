﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    //For whatever reason after changing the collider from a BoxCollider2D to a normal box collider, now it behaves how I want it to.
    public SpriteRenderer sprite;
    public GameObject placedTower;

    [Header("Grid position vars")]
    public bool hasTower;
    public string towerTypeSelected;

    void Awake()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    void OnMouseOver()
    {
        //Reveal the grid sprite on mouse over
        sprite.enabled = true;
        //Debug.Log("Is hovering");

        if (Input.GetMouseButtonDown(0))
        {
            //Will eventually give a menu of tower choices, and check against a gold value to see if the tower can be placed or not
            //Spawning a tower on mouse click if one is not present to test for now
            if (!hasTower)
            {
                //Get the tower GameObject
                GameObject towerContainer = GameObject.Find("Towers");
                GameObject tower = (GameObject)Instantiate(Resources.Load("Towers/NeutralTower"), towerContainer.transform);

                //Place the tower
                tower.transform.position = this.transform.position;
                placedTower = tower;
                hasTower = true;
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

    void OnMouseExit()
    {
        //Hide the grid sprite when on mouse exit 
        sprite.enabled = false;
        //Debug.Log("Not hovering");
    }
}