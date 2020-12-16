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
            double d = Math.Pow(2, level) * 1000;
            if (player.purchasedUpgrade[12])
                d = Math.Pow(1.65, level) * 1000;
            if (player.purchasedUpgrade[14])
                d /= Math.Pow(player.clickpoints, 0.15);
            return d;
        }
    }

    public double GetMultiplier()
    {
        double d = 1 + ((Level + freeLevels) * 0.15);
        if (player.purchasedUpgrade[13])
            d *= 1 + player.level * 0.0025;
        if (player.purchasedUpgrade[21])
            d *= 1 + auto.bonus;
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
            if (player.purchasedUpgrade[10])
                return GetMultiplier() * ((GetMultiplier() - 1) * 0.2 + 1);
            return GetMultiplier();
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
        costDisplay.text = NumberFormatter.instance.FormatNumber(Cost);
        multiplierDisplay.text = GetMultiplier().ToString("N1") + "x coins from clicks";
    }

    public void Upgrade() {
        if (player.coins >= Cost) {
            player.coins -= Cost;
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
        if (player.purchasedUpgrade[9])
            multiplierDisplay.text += "\n" + ((GetMultiplier() - 1) * 0.2 + 1).ToString("N2") + "x clickpoints";
        if (player.purchasedUpgrade[11])
            multiplierDisplay.text += "\n" + ((GetMultiplier() - 1) * 0.2 + 1).ToString("N2") + "x experience";
        if (player.purchasedUpgrade[31])
            multiplierDisplay.text += "\n+" + (Level * 0.75f).ToString("N1") + "% extra coin chance";
        if (player.purchasedUpgrade[45])
            multiplierDisplay.text += "\n" + DiamondCoinMulti.ToString("N3") + "x diamond coins";
    }
}
