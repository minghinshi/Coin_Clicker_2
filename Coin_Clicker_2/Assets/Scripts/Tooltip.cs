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
    ProgressBarHandler progressBar;
    Text tooltipText;
    UpgradeHandler upgradeHandler;
    public static Tooltip instance;

    bool isDisplayingTooltip;

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
        progressBar = ProgressBarHandler.instance;
        rectTransform = GetComponent<RectTransform>();
        tooltipText = transform.GetChild(0).GetComponent<Text>();
        upgradeHandler = UpgradeHandler.instance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Input.mousePosition + new Vector3(20f,10f);
    }

    public void SetText(string Text) {
        tooltipText.text = Text;
        transform.localScale = new Vector3(1f, 1f);
        if (!isDisplayingTooltip)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            isDisplayingTooltip = true;
        }
    }

    public void SetText(string Text, params object[] args) {
        SetText(string.Format(Text, args));
    }

    public void SetCoinsTooltip() {
        List<object> objects = new List<object>();

        if (upgradeHandler.IsUpgradePurchased(1))
        {
            objects.Add(1 + Math.Log10(player.Clickpoints + 1) * 0.1);
        }
        if (upgradeHandler.IsUpgradePurchased(4))
        {
            objects.Add(1 + player.Level * 0.03);
        }
        if (upgradeHandler.IsUpgradePurchased(18))
        {
            objects.Add(1 + autoclicker.bonus);
        }
        if (upgradeHandler.IsUpgradePurchased(32) && autoclicker.surgeTimeRemaining > 0f)
        {
            objects.Add(1 + (autoclicker.surgeTimeRemaining * 0.05));
        }
        objects.Add(progressBar.GetTotalMultiplier());
        if (upgradeHandler.IsUpgradePurchased(51))
        {
            objects.Add(progressBar.barMulti[0]);
        }
        objects.Add(multi.CoinMulti);
    }

    public void ClearText() {
        tooltipText.text = "";
        transform.localScale = new Vector3(0f, 0f);
        isDisplayingTooltip = false;
    }
}
