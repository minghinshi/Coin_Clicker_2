using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Tooltip : MonoBehaviour
{
    RectTransform rectTransform;
    Player player;
    Multiplier multi;
    Autoclicker autoclicker;
    ProgressBar progressBar;
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
        multi = Multiplier.instance;
        autoclicker = Autoclicker.instance;
        progressBar = ProgressBar.instance;
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
                stringToDisplay = "Clicker";
                break;
            case 1:
                stringToDisplay = "Upgrades";
                break;
            case 2:
                stringToDisplay = "Booster\n\n<color=lime>{0}</color> Total Levels";
                objects.Add(multi.Level + multi.freeLevels);
                objects.Add(multi.level);
                if (player.purchasedUpgrade[34]) {
                    stringToDisplay += "\n<color=lime>{1}</color> Purchased\n<color=lime>{2}</color> From Dropping Coins";
                    objects.Add(multi.freeLevels);
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

    public void SetCoinsTooltip() {
        string stringToDisplay = "Coins";
        List<object> objects = new List<object>();

        if (player.purchasedUpgrade[1])
        {
            objects.Add(1 + Math.Log10(player.clickpoints + 1) * 0.1);
        }
        if (player.purchasedUpgrade[4])
        {
            objects.Add(1 + player.level * 0.03);
        }
        if (player.purchasedUpgrade[18])
        {
            objects.Add(1 + autoclicker.bonus);
        }
        if (player.purchasedUpgrade[32] && autoclicker.surgeTimeRemaining > 0f)
        {
            objects.Add(1 + (autoclicker.surgeTimeRemaining * 0.05));
        }
        objects.Add(progressBar.TotalMulti());
        if (player.purchasedUpgrade[51])
        {
            objects.Add(progressBar.barMulti[0]);
        }
        objects.Add(multi.CoinMulti);
    }

    public void ClearText() {
        tooltipText.text = "";
        transform.localScale = new Vector3(0f, 0f);
    }
}
