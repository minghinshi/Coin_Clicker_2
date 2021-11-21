﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Upgrade{
    int row;
    int column;
    bool isPurchased = false;
    bool canBePurchased = true;
    bool hasAdditiveEffect = false;
    GameObject upgradeGameObject;
    Image upgradeImage;
    Button upgradeButton;
    TooltipDisplayer tooltipDisplayer;
    Func<double> effect;
    string description;

    public Upgrade(int r, int c, GameObject go) {
        row = r;
        column = c;

        upgradeGameObject = go;
        upgradeImage = upgradeGameObject.GetComponent<Image>();
        upgradeButton = upgradeGameObject.GetComponent<Button>();
        tooltipDisplayer = upgradeGameObject.GetComponent<TooltipDisplayer>();
        upgradeGameObject.SetActive(false);

        canBePurchased = (r != c);
        if (canBePurchased)
        {
            switch (row)
            {
                //Upgrades For Coins
                case 0:
                    switch (column)
                    {
                        case 1:
                            effect = () => Math.Max(Math.Pow(Player.instance.Clickpoints, 0.2), 1);
                            description = "Clickpoints boost Coin gain.";
                            break;
                        case 2:
                            effect = () => Player.instance.Level * 0.15 + 1;
                            description = "Levels boost Coin gain.";
                            break;
                        case 3:
                            effect = () => Multiplier.instance.GetMultiplier();
                            description = "Increases the multiplier Boosters give to Coins.";
                            break;
                        case 4:
                            effect = () => Autoclicker.instance.SpeedMultiplierFromPower;
                            description = "Autoclicker speed multiplier is applied to Coins.";
                            break;
                        case 5:
                            effect = () => 1 + Autoclicker.instance.SurgeDuration;
                            description = "Coin gain is boosted depending on Autoclicker Surge time remaining.";
                            break;
                    }
                    break;
                //Upgrades For Clickpoints
                case 1:
                    switch (column)
                    {
                        case 0:
                            effect = () => Math.Max(Math.Pow(Player.instance.Coins, 0.2), 1);
                            description = "Coins boost Clickpoint gain.";
                            break;
                        case 2:
                            effect = () => Player.instance.Level * 0.15 + 1;
                            description = "Levels boost Clickpoint gain.";
                            break;
                        case 3:
                            effect = () => Multiplier.instance.GetMultiplier();
                            description = "Boosters gives its multiplier to Clickpoints as well.";
                            break;
                        case 4:
                            effect = () => Autoclicker.instance.SpeedMultiplierFromPower;
                            description = "Autoclicker speed multiplier is applied to Clickpoints.";
                            break;
                        case 5:
                            effect = () => 1 + Autoclicker.instance.SurgeDuration;
                            description = "Clickpoint gain is boosted depending on Autoclicker Surge time remaining.";
                            break;
                    }
                    break;
                //Upgrades For Levels & Experience
                case 2:
                    switch (column)
                    {
                        case 0:
                            effect = () => Math.Max(Math.Pow(Player.instance.Coins, 0.1), 1);
                            description = "Coins boost Experience gain.";
                            break;
                        case 1:
                            effect = () => Math.Max(Math.Pow(Player.instance.Clickpoints, 0.1), 1);
                            description = "Clickpoints boost Experience gain.";
                            break;
                        case 3:
                            effect = () => Math.Sqrt(Multiplier.instance.GetMultiplier());
                            description = "Boosters gives some of its multiplier to Experience as well.";
                            break;
                        case 4:
                            effect = () => Math.Sqrt(Autoclicker.instance.SpeedMultiplierFromPower);
                            description = "Part of the Autoclicker speed multiplier is applied to Experience.";
                            break;
                        case 5:
                            description = "Collecting a Dropping Coin reduces the experience requirement to that of the previous level.";
                            break;
                        case 6:
                            effect = () => (Math.Log10(Player.instance.diamondCoins + 1) * 0.1);
                            description = "Diamond Coins provide a chance for the Experience requirement to not increase when you level up.";
                            hasAdditiveEffect = true;
                            break;
                    }
                    break;
                //Upgrades For Multiplier / Booster
                case 3:
                    switch (column)
                    {
                        case 0:
                            description = "Boosters become 15% more expensive per upgrade, instead of 30%.\nThis applies to all previous Boosters upgrades.";
                            break;
                        case 1:
                            effect = () => Math.Max(Player.instance.Clickpoints / 1e4, 1);
                            description = "Booster upgrades are discounted based on Clickpoints.";
                            break;
                        case 2:
                            effect = () => Player.instance.Level * 0.01 + 1;
                            description = "Levels increase the effectiveness of boosters.";
                            break;
                        case 4:
                            effect = () => Math.Pow(Autoclicker.instance.autoclickerPower + 1, 2);
                            description = "Booster upgrades are discounted based on Autoclicker Power.";
                            break;
                        case 5:
                            description = "Collecting a Dropping Coin grants a free Booster level.";
                            break;
                    }
                    break;
                //Upgrades For Autoclicker
                case 4:
                    switch (column)
                    {
                        case 0:
                            effect = () => Math.Log10(Player.instance.Coins + 1) * 0.2 + 1;
                            description = "Coins boost Autoclicker Power.";
                            break;
                        case 1:
                            description = "The Autoclicker becomes 5x as fast when the coin is held.";
                            break;
                        case 2:
                            effect = () => 1 + Player.instance.Level * 0.01;
                            description = "Levels boost Autoclicker Power.";
                            break;
                        case 3:
                            effect = () => Multiplier.instance.Level * 0.5;
                            hasAdditiveEffect = true;
                            description = "Autoclicker Level is increased by 50% of the Booster Level.\nThis does not increase the cost of Autoclicker Levels.";
                            break;
                        case 5:
                            description = "Unlock Autoclicker Surges.\nDuring an Autoclicker Surge, the Autoclicker runs 60x as fast.\nWhen a Dropping Coin is collected, 1 second is added to the Surge timer.";
                            break;
                    }
                    break;
                //Upgrades For Dropping Coins
                case 5:
                    switch (column)
                    {
                        case 0:
                            effect = () => Math.Log10(Player.instance.Coins + 1) * 0.05;
                            hasAdditiveEffect = true;
                            description = "Coins increase the number of Dropping Coins per cycle.";
                            break;
                        case 1:
                            effect = () => Math.Log10(Player.instance.Clickpoints + 1) * 0.1;
                            hasAdditiveEffect = true;
                            description = "Every time a Dropping Coin is spawned, more are spawned <color=yellow>at the same time</color>, based on Clickpoints.";
                            break;
                        case 2:
                            effect = () => 1 + Player.instance.Level * 0.002;
                            description = "Experience levels reduces the cooldown of Dropping Coins.";
                            break;
                        case 3:
                            effect = () => Math.Pow(Multiplier.instance.Level, 0.2) * 0.25;
                            hasAdditiveEffect = true;
                            description = "Booster levels increase the number of Dropping Coins per cycle.";
                            break;
                        case 4:
                            effect = () => Math.Log10(Autoclicker.instance.ClicksPerSec + 1) + 1;
                            description = "Autoclicker speed further improves the amount of resources provided by Dropping Coins.";
                            break;
                    }
                    break;
            }
            if (effect != null)
                description += "\nCurrently: <color=#407fbf>{0}{1}</color>";
        }
        else {
            upgradeImage.color = new Color(44f / 255, 62f / 255, 80f / 255);
            tooltipDisplayer.enabled = false;
        }

        upgradeButton.onClick.AddListener(delegate { PurchaseUpgrade(); });
        tooltipDisplayer.SetStringToDisplay(delegate
        {
            if (effect != null)
                return string.Format(description, (hasAdditiveEffect ? "+" : "x"), NumberFormatter.FormatNumber(effect()));
            else
                return description;
        });
    }

    public int GetTierRequired() {
        return Math.Max(row, column);
    }

    public double GetEffect() {
        return (isPurchased ? effect() : (hasAdditiveEffect ? 0 : 1));
    }

    public bool IsUpgradePurchased() {
        return isPurchased;
    }

    public void TryToSetUpgradeAsVisible(int tier) {
        if (tier >= GetTierRequired() && !upgradeGameObject.activeInHierarchy)
            upgradeGameObject.SetActive(true);
    }

    public void PurchaseUpgrade(bool ignoreCost = false) {
        if (!isPurchased && (Player.instance.Coins >= UpgradeHandler.GetCost() || ignoreCost)) {
            if(!ignoreCost)
                Player.instance.Coins -= UpgradeHandler.GetCost();
            isPurchased = true;
            UpgradeHandler.RegisterUpgrade();
            upgradeImage.color = new Color(46f / 255, 204f / 255, 113f / 255);
        }
    }
}

public class UpgradeHandler : MonoBehaviour
{
    static TierHandler tierHandler;
    static Upgrade[][] gridOfUpgrades;
    static List<Upgrade> listOfUpgrades = new List<Upgrade>();
    static int numberOfPurchasedUpgrades = 0;
    [SerializeField] GameObject upgradePrefab;
    [SerializeField] Text upgradeCostDisplay;

    public static double GetCost() {
        return 10 * Math.Pow(1.15, Math.Pow(numberOfPurchasedUpgrades + tierHandler.GetTier(), 2));
    }

    // Start is called before the first frame update
    void Start()
    {
        tierHandler = TierHandler.instance;

        //Create the big list of upgrades
        int numberOfFeatures = tierHandler.GetNumberOfFeatures();
        gridOfUpgrades = new Upgrade[numberOfFeatures][];
        for (int row = 0; row < numberOfFeatures; row++)
        {
            gridOfUpgrades[row] = new Upgrade[numberOfFeatures];
            for (int col = 0; col < numberOfFeatures; col++)
            {
                gridOfUpgrades[row][col] = new Upgrade(row, col, Instantiate(upgradePrefab, TierHandler.instance.GetFeature(row).GetTransformOfRowOfUpgrades()));
                listOfUpgrades.Add(gridOfUpgrades[row][col]);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        upgradeCostDisplay.text = NumberFormatter.FormatNumber(GetCost());
    }

    public static void SetUpgradesAsVisible(int tier) {
        foreach (Upgrade upgrade in listOfUpgrades)
            upgrade.TryToSetUpgradeAsVisible(tier);
    }

    public static double GetEffectOfUpgrade(int row, int col) {
        try { return gridOfUpgrades[row][col].GetEffect(); }
        catch (NullReferenceException) {
            Debug.Log(string.Format("No upgrade of r{0}c{1} found, returning default value of 1", row, col));
            return 1; 
        }
    }

    public static Upgrade[][] GetGridOfUpgrades() {
        return gridOfUpgrades;
    }

    public static bool IsUpgradePurchased(int row, int col)
    {
        return gridOfUpgrades[row][col].IsUpgradePurchased();
    }

    public static void RegisterUpgrade() {
        numberOfPurchasedUpgrades++;
    }
}
