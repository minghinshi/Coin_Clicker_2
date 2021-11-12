using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isDisplayingTooltip = false;
    Func<string> stringToDisplay;

    // Update is called once per frame
    public void Update()
    {
        if (isDisplayingTooltip)
            Tooltip.instance.DisplayTooltip(stringToDisplay());
    }

    public void SetStringToDisplay(Func<string> function) {
        stringToDisplay = function;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isDisplayingTooltip = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isDisplayingTooltip = false;
        Tooltip.instance.HideTooltip();
    }
}
