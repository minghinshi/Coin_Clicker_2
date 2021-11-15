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

    public double Cost {
        get {
            double baseCost = UpgradeHandler.IsUpgradePurchased(3, 0) ? 1.15 : 1.3;
            return 1e4
                * Math.Pow(baseCost, level)
                / UpgradeHandler.GetEffectOfUpgrade(3, 1)
                / UpgradeHandler.GetEffectOfUpgrade(3, 4);
        }
    }

    public double GetMultiplier()
    {
        double d = 1 + Level * 0.15;
        d *= UpgradeHandler.GetEffectOfUpgrade(3, 2);
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
            return GetMultiplier() * UpgradeHandler.GetEffectOfUpgrade(0, 3);
        }
    }

    public int level;
    public int Level {
        get {
            return level + diamond.diamondMultiLevels + freeLevels;
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
        costDisplay.text = NumberFormatter.FormatNumber(Cost);
        multiplierDisplay.text = GetMultiplier().ToString("N1") + "x coins from clicks";
    }

    public void Upgrade() {
        while (player.Coins >= Cost)
        {
            player.Coins -= Cost;
            level++;
            if (!Input.GetKey(KeyCode.LeftShift))
                break;
        }
        player.UpdateDisplays();
        UpdateDisplays();
    }

    public void UpdateDisplays() {
        costDisplay.text = NumberFormatter.FormatNumber(Cost);
        levelDisplay.text = "Level " + Level.ToString("N0");
        levelSourceDisplay.text = level.ToString("N0") + " levels from upgrading";
        if (freeLevels > 0)
            levelSourceDisplay.text += "\n" + freeLevels.ToString("N0") + " free levels from dropping coins";
        if (diamond.diamondMultiLevels > 0)
            levelSourceDisplay.text += "\n" + diamond.diamondMultiLevels.ToString("N0") + " levels from diamond upgrade";

        multiplierDisplay.text = NumberFormatter.FormatNumber(CoinMulti) + "x coins";
        if (UpgradeHandler.IsUpgradePurchased(1,3))
            multiplierDisplay.text += "\n" + NumberFormatter.FormatNumber(UpgradeHandler.GetEffectOfUpgrade(1,3)) + "x clickpoints";
        if (UpgradeHandler.IsUpgradePurchased(2,3))
            multiplierDisplay.text += "\n" + NumberFormatter.FormatNumber(UpgradeHandler.GetEffectOfUpgrade(2,3)) + "x experience";
        if (UpgradeHandler.IsUpgradePurchased(5,3))
            multiplierDisplay.text += "\n+" + (Level * 0.75f).ToString("N1") + "% extra coin chance";
        /*if (upgradeHandler.IsUpgradePurchased(45))
            multiplierDisplay.text += "\n" + DiamondCoinMulti.ToString("N3") + "x diamond coins";*/
    }
}
