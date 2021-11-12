using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Tooltip : MonoBehaviour
{
    RectTransform rectTransform;
    Multiplier multi;
    Text tooltipText;
    public static Tooltip instance;

    bool isDisplayingTooltip;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        multi = Multiplier.instance;
        rectTransform = GetComponent<RectTransform>();
        tooltipText = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Input.mousePosition + new Vector3(20f,10f);
    }

    public void DisplayTooltip(string Text) {
        tooltipText.text = Text;
        transform.localScale = new Vector3(1f, 1f);
        if (!isDisplayingTooltip)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            isDisplayingTooltip = true;
        }
    }

    public void DisplayTooltip(string Text, params object[] args) {
        DisplayTooltip(string.Format(Text, args));
    }

    public void SetCoinsTooltip() {
        List<object> objects = new List<object>();

        /*if (upgradeHandler.IsUpgradePurchased(1))
        {
            objects.Add(1 + Math.Log10(player.Clickpoints + 1) * 0.1);
        }
        if (upgradeHandler.IsUpgradePurchased(4))
        {
            objects.Add(1 + player.Level * 0.03);
        }
        if (upgradeHandler.IsUpgradePurchased(18))
        {
            objects.Add(1 + autoclicker.autoclickerPower);
        }
        if (upgradeHandler.IsUpgradePurchased(32) && autoclicker.SurgeDuration > 0f)
        {
            objects.Add(1 + (autoclicker.SurgeDuration * 0.05));
        }
        objects.Add(progressBar.GetTotalMultiplier());
        if (upgradeHandler.IsUpgradePurchased(51))
        {
            objects.Add(progressBar.barMulti[0]);
        }*/
        objects.Add(multi.CoinMulti);
    }

    public void HideTooltip() {
        tooltipText.text = "";
        transform.localScale = new Vector3(0f, 0f);
        isDisplayingTooltip = false;
    }
}
