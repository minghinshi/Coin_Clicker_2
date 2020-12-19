using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Autoclicker : MonoBehaviour
{
    public static Autoclicker instance;

    private Player player;
    private Clicker clicker;
    private Multiplier multi;
    private DiamondUpgrades platinum;
    private CoinDrop drop;

    public float timeUntilClick;
    public float surgeTimeRemaining;
    public double clicksPerSec
    {
        get
        {
            double d = Level + 1;
            if (player.purchasedUpgrade[24])
                d += multi.Level * 0.1;
            if (clicker.coinIsHeld && player.purchasedUpgrade[17])
                d *= 1.15;
            if (surgeTimeRemaining > 0)
                d *= 2;
            if (player.purchasedUpgrade[22])
                d *= 1 + player.level * 0.0015;
            if (player.devMode)
                d *= 10;
            return d;
        }
    }
    public double clickOverflowBonus
    {
        get
        {
            double d = Math.Max(1, clicksPerSec/100);
            return d;
        }
    }

    public double cost
    {
        get
        {
            double d = Math.Pow(2, level) * 8;
            return d;
        }
    }
    public double bonus;
    public double BonusIncrease()
    {
        double d = 2.5e-6;
        if (player.purchasedUpgrade[23])
            d += Math.Log10(player.coins + 1) * 6.25e-8;
        return d;
    }

    public int level;
    public int Level
    {
        get
        {
            int i = level + platinum.diamondAutoLevels;
            return i;
        }
    }

    public Slider progressBar;

    public Text cpsDisplay;
    public Text costDisplay;
    public Text bonusDisplay;
    public Text surgeDisplay;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        clicker = Clicker.instance;
        multi = Multiplier.instance;
        platinum = DiamondUpgrades.instance;
        drop = CoinDrop.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.purchasedUpgrade[15])
            timeUntilClick -= Time.deltaTime * Convert.ToSingle(Math.Min(clicksPerSec,100));
        while (timeUntilClick < 0)
        {
            timeUntilClick++;
            clicker.Click(clickOverflowBonus);

            if (player.purchasedUpgrade[18])
                bonus += BonusIncrease() * clickOverflowBonus;
        }

        progressBar.value = 1 - timeUntilClick;
        if (player.purchasedUpgrade[28])
        {
            if (surgeTimeRemaining > 0)
            {
                surgeTimeRemaining -= Time.deltaTime;
                surgeDisplay.text = "Autoclicker (" + surgeTimeRemaining.ToString("N1") + "s)";
            }
            if (surgeTimeRemaining <= 0)
            {
                surgeTimeRemaining = 0;
                surgeDisplay.text = "Autoclicker";
            }
        }
        if (player.purchasedUpgrade[44])
            if (UnityEngine.Random.Range(0f, 1f) < 1 - Mathf.Pow(0.9999f, Convert.ToSingle(clickOverflowBonus)))
                drop.platinum = true;
    }

    public void Upgrade() {
        if (player.clickpoints >= cost) {
            player.clickpoints -= cost;
            level++;

            player.UpdateDisplays();
            UpdateDisplays();
        }
    }

    public void UpdateDisplays() {
        cpsDisplay.text = clicksPerSec.ToString("N1") + " clicks/sec";
        costDisplay.text = NumberFormatter.instance.FormatNumber(cost);

        if (player.purchasedUpgrade[18])
        {
            if(bonus < 100)
                bonusDisplay.text = "+" + (bonus * 100).ToString("N2") + "%";
            else
                bonusDisplay.text = (bonus+1).ToString("N2") + "x";
        }
    }
}
