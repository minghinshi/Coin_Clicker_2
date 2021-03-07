using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Multiplier : MonoBehaviour {
    public static Multiplier instance;

    public Player player;
    public Autoclicker auto;
    public DiamondUpgrades diamond;
    private UpgradeHandler upgradeHandler;

    public double Cost {
        get {
            double baseCost = 2 + upgradeHandler.GetEffect(12);
            return 1000 * Math.Pow(baseCost, level) * upgradeHandler.GetEffect(14);
        }
    }

    public double GetMultiplier()
    {
        double d = 1 + ((Level + freeLevels) * 0.15);
        d *= upgradeHandler.GetTotalEffect(13, 21);
        return d;
    }

    public double DiamondCoinMulti {
        get
        {
            return Math.Pow(GetMultiplier(), 0.05);
        }
    }

    public double CoinMulti {
        get
        {
            return GetMultiplier() * upgradeHandler.GetEffect(10);
        }
    }

    public int level;
    public int Level {
        get {
            return level + diamond.diamondMultiLevels;
        }
    }
    public int freeLevels;

    public Text costDisplay;
    public Text multiplierDisplay;
    public Text levelDisplay;
    public Text levelSourceDisplay;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        upgradeHandler = UpgradeHandler.instance;

        costDisplay.text = NumberFormatter.instance.FormatNumber(Cost);
        multiplierDisplay.text = GetMultiplier().ToString("N1") + "x coins from clicks";
    }

    public void Upgrade() {
        if (player.Coins >= Cost) {
            player.Coins -= Cost;
            level++;

            player.UpdateDisplays();
            UpdateDisplays();
        }
    }

    public void UpdateDisplays() {
        costDisplay.text = NumberFormatter.instance.FormatNumber(Cost);
        levelDisplay.text = "Level " + (Level+freeLevels).ToString("N0");
        levelSourceDisplay.text = level.ToString("N0") + " levels from upgrading";
        if (freeLevels > 0)
            levelSourceDisplay.text += "\n" + freeLevels.ToString("N0") + " free levels from dropping coins";
        if (diamond.diamondMultiLevels > 0)
            levelSourceDisplay.text += "\n" + diamond.diamondMultiLevels.ToString("N0") + " levels from diamond upgrade";

        multiplierDisplay.text = CoinMulti.ToString("N2") + "x coins";
        if (upgradeHandler.IsUpgradePurchased(9))
            multiplierDisplay.text += "\n" + ((GetMultiplier() - 1) * 0.2 + 1).ToString("N2") + "x clickpoints";
        if (upgradeHandler.IsUpgradePurchased(11))
            multiplierDisplay.text += "\n" + ((GetMultiplier() - 1) * 0.2 + 1).ToString("N2") + "x experience";
        if (upgradeHandler.IsUpgradePurchased(31))
            multiplierDisplay.text += "\n+" + (Level * 0.75f).ToString("N1") + "% extra coin chance";
        if (upgradeHandler.IsUpgradePurchased(45))
            multiplierDisplay.text += "\n" + DiamondCoinMulti.ToString("N3") + "x diamond coins";
    }
}
