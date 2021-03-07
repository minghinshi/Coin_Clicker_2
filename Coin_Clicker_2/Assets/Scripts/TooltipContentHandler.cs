using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipContentHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tooltip tooltip;
    public string tooltipText;
    public bool isDisplayingTooltip = false;

    public string stringToDisplay = "";
    public List<object> objects = new List<object>();

    // Update is called once per frame
    public void Update()
    {
        if (isDisplayingTooltip)
            UpdateTooltip();
    }

    public virtual void UpdateTooltipText()
    {
        throw new NotImplementedException();
    }

    void UpdateTooltip() {
        stringToDisplay = "";
        objects.Clear();
        UpdateTooltipText();
        tooltip.SetText(String.Format(stringToDisplay, objects.ToArray()));
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isDisplayingTooltip = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isDisplayingTooltip = false;
        tooltip.ClearText();
    }
}
