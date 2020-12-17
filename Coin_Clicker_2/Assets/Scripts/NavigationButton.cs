using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CanvasGroup targetPanel;
    CanvasGroup[] allPanels;
    public int tooltipType;

    // Start is called before the first frame update
    void Start()
    {
        allPanels = NavigationBar.instance.panelsToNavigate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnToggle() {
        foreach (CanvasGroup panel in allPanels) {
            panel.alpha = 0;
            panel.blocksRaycasts = false;
            panel.interactable = false;
        }
        targetPanel.alpha = 1;
        targetPanel.blocksRaycasts = true;
        targetPanel.interactable = true;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        Tooltip.instance.SetNavigationTooltip(tooltipType);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        Tooltip.instance.ClearText();
    }
}
