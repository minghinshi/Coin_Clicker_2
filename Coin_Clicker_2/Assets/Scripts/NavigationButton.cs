using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NavigationButton : MonoBehaviour
{
    public CanvasGroup targetPanel;
    private NavigationBar bar;
    private Button button;
    public int tooltipType;

    // Start is called before the first frame update
    void Start()
    {
        bar = NavigationBar.instance;

        button = GetComponent<Button>();
        button.onClick.AddListener(Toggle);
    }

    /*public override void UpdateTooltipText() {
        switch (tooltipType)
        {
            case 0:
                stringToDisplay = "Clicker";
                break;
            case 1:
                stringToDisplay = "Upgrades";
                break;
            case 2:
                stringToDisplay = "Booster\n\n<color=lime>{0}</color> Total Levels";
                objects.Add(multiplier.Level + multiplier.freeLevels);
                if (upgradeHandler.IsUpgradePurchased(34))
                {
                    stringToDisplay += "\n<color=lime>{1}</color> Purchased\n<color=lime>{2}</color> From Dropping Coins";
                    objects.Add(multiplier.level);
                    objects.Add(multiplier.freeLevels);
                }
                break;
            case 3:
                stringToDisplay = "Autoclicker\n\n<color=lime>{0}</color> Total Levels";
                objects.Add(autoclicker.TotalLevel);
                break;
            case 4:
                stringToDisplay = "Diamond Upgrades";
                break;
            case 5:
                stringToDisplay = "Progress Bars";
                break;
            case 6:
                stringToDisplay = "Options";
                break;
            default:
                stringToDisplay = "<color=red>A tooltip should be here but it is missing.\nPlease report this bug.</color>";
                break;
        }
    }*/

    public void Toggle() {
        foreach (CanvasGroup panel in bar.panelsToNavigate) {
            panel.alpha = 0;
            panel.blocksRaycasts = false;
            panel.interactable = false;
        }
        targetPanel.alpha = 1;
        targetPanel.blocksRaycasts = true;
        targetPanel.interactable = true;
    }

    
}
