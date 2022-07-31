using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager tooltipInstance;
    public TextMeshProUGUI textObj;

    private void Awake()
    {
        if (tooltipInstance != null && tooltipInstance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            tooltipInstance = this;
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(0, 10, 0);
        //mouseScreenPosition = Input.mousePosition;
        //transform.position =  camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, camera.nearClipPlane));
    }

    public void SetAndShowTooltip(string text)
    {
        gameObject.SetActive(true);
        textObj.text = text;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        textObj.text = string.Empty;
    }
}
