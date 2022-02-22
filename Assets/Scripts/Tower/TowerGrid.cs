using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    //For whatever reason after changing the collider from a BoxCollider2D to a normal box collider, now it behaves how I want it to.
    public SpriteRenderer sprite;
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
    }

    void OnMouseExit()
    {
        //Hide the grid sprite when on mouse exit 
        sprite.enabled = false;
        //Debug.Log("Not hovering");
    }

    void OnMouseDown()
    {
        //Need options for building towers once the grid space has been clicked
        Debug.Log("Clicked on grid item");
    }
}
