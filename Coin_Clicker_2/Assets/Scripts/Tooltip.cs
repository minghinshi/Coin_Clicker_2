using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    RectTransform rectTransform;
    Player player;
    Multiplier multiplayer;
    Autoclicker autoclicker;
    Text tooltipText;
    public static Tooltip instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        multiplayer = Multiplier.instance;
        autoclicker = Autoclicker.instance;
        rectTransform = GetComponent<RectTransform>();
        tooltipText = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Input.mousePosition + new Vector3(20f,10f);
    }

    public void SetText(string Text) {
        tooltipText.text = Text;
        transform.localScale = new Vector3(1f, 1f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void SetText(string Text, object arg)
    {
        SetText(string.Format(Text, arg));
    }

    public void SetText(string Text, params object[] args) {
        SetText(string.Format(Text, args));
    }

    public void SetNavigationTooltip(int i) {
        string stringToDisplay = "";
        List<object> objects = new List<object>();
        switch (i) {
            case 0:
                stringToDisplay = "Clicker\n\n<color=lime>{0}</color> Manual Clicks";
                objects.Add(player.numberOfManualClicks);
                break;
            case 1:
                stringToDisplay = "Upgrades\n\n<color=lime>{0}</color> Upgrades Purchased";
                objects.Add(player.numberOfPurchasedUpgrades);
                break;
            case 2:
                stringToDisplay = "Booster\n\n<color=lime>{0}</color> Total Levels";
                objects.Add(multiplayer.Level + multiplayer.freeLevels);
                objects.Add(multiplayer.level);
                if (player.purchasedUpgrade[34]) {
                    stringToDisplay += "\n<color=lime>{1}</color> Purchased\n<color=lime>{2}</color> From Dropping Coins";
                    objects.Add(multiplayer.freeLevels);
                }
                break;
            case 3:
                stringToDisplay = "Autoclicker\n\n<color=lime>{0}</color> Total Levels";
                objects.Add(autoclicker.Level);
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
                stringToDisplay = "<color=red>A tooltip should be here but it is missing. Please report this bug.</color>";
                break;
        }
        SetText(stringToDisplay, objects.ToArray());
    }

    public void ClearText() {
        tooltipText.text = "";
        transform.localScale = new Vector3(0f, 0f);
    }
}
