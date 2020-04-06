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

    public double cost {
        get {
            double d = Math.Pow(2, level) * 1000000;
            if (player.purchasedUpgrade[12])
                d = Math.Pow(1.5, level) * 1000000;
            if (player.purchasedUpgrade[14])
                d /= Math.Sqrt(player.clickpoints+1);
            return d;
        }
    }
    public double multiplier {
        get {
            double d = 1 + (Level+freeLevels) * 0.2;
            if (player.purchasedUpgrade[13])
                d *= 1 + player.level / 100d;
            if (player.purchasedUpgrade[21])
                d *= 1 + Math.Pow(Math.Log10(auto.bonus + 1), 3);
            return d;
        }
    }

    public double diamondCoinMulti {
        get
        {
            return Math.Pow(multiplier,0.05);
        }
    }

    public double coinMulti {
        get
        {
            if (player.purchasedUpgrade[10])
                return Math.Pow(multiplier, 2);
            return multiplier;
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
        costDisplay.text = NumberFormatter.instance.FormatNumber(cost);
        multiplierDisplay.text = multiplier.ToString("N1") + "x coins from clicks";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Upgrade() {
        if (player.coins >= cost) {
            player.coins -= cost;
            level++;

            player.UpdateDisplays();
            UpdateDisplays();
        }
    }

    public void UpdateDisplays() {
        costDisplay.text = NumberFormatter.instance.FormatNumber(cost);
        levelDisplay.text = "Level " + (Level+freeLevels).ToString("N0");
        levelSourceDisplay.text = level.ToString("N0") + " levels from upgrading";
        if (freeLevels > 0)
            levelSourceDisplay.text += "\n" + freeLevels.ToString("N0") + " free levels from dropping coins";
        if (diamond.diamondMultiLevels > 0)
            levelSourceDisplay.text += "\n" + diamond.diamondMultiLevels.ToString("N0") + " levels from diamond upgrade";
        string multiplierText;
        if (coinMulti >= 1000000)
            multiplierText = NumberFormatter.instance.FormatNumber(coinMulti);
        else
            multiplierText = coinMulti.ToString("N1");

        multiplierDisplay.text = multiplierText + "x coins from clicks";
        if (player.purchasedUpgrade[9])
            multiplierDisplay.text += "\n" + multiplier.ToString("N1") + "x clickpoints";
        if (player.purchasedUpgrade[11])
            multiplierDisplay.text += "\n" + multiplier.ToString("N1") + "x experience";
        if (player.purchasedUpgrade[31] && Level >= 100)
            multiplierDisplay.text += "\n" + Mathf.FloorToInt(Level / 100f).ToString("N0") + " extra coin(s)";
        if (player.purchasedUpgrade[45])
            multiplierDisplay.text += "\n" + diamondCoinMulti.ToString("N3") + "x diamond coins";
    }
}
