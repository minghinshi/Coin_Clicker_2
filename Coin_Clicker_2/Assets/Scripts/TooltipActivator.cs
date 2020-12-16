using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Tooltip tooltip;
    public string tooltipToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        tooltip = Tooltip.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(tooltip);
        tooltip.SetText(tooltipToDisplay);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.ClearText();
    }
}
