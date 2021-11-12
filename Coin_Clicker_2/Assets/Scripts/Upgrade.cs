/*
using System;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : TooltipContentHandler
{
    private Player player;
    private Multiplier multi;
    private Autoclicker autoclicker;
    private ProgressBarHandler progressBar;
    private UpgradeHandler upgradeHandler;
    private Clicker clicker;
    private PurchaseHandler purchaseHandler;
    private NumberFormatter numberFormatter;
    private Image image;

    public int id;
    public int tierRequired;
    public bool isPurchased;
    public bool isAdditive = false;

	public void SetReferences () {
        player = Player.instance;
        multi = Multiplier.instance;
        autoclicker = Autoclicker.instance;
        progressBar = ProgressBarHandler.instance;
        upgradeHandler = UpgradeHandler.instance;
        clicker = Clicker.instance;
        purchaseHandler = PurchaseHandler.instance;
        numberFormatter = NumberFormatter.instance;
        tooltip = Tooltip.instance;
        image = GetComponent<Image>();
	}


    public void AttemptPurchase() {
        if (!isPurchased)
            if (purchaseHandler.IsAffordable(upgradeHandler.GetCost()))
                PurchaseUpgrade();
    }

    public void PurchaseUpgrade() {
        switch (id) {
            case 25:
                player.UnlockCoinDrop();
                break;
            case 36:
                player.UnlockDiamond();
                break;
            case 37:
                player.DiamondAutoPurchase.SetActive(true);
                break;
            case 38:
                player.DiamondMultiPurchase.SetActive(true);
                break;
            case 39:
                player.DropPurchase.SetActive(true);
                break;
            case 47:
                player.ConvertToggle.SetActive(true);
                break;
            case 48:
                player.UnlockProgressBars();
                break;
            case 49:
                player.DiamondSpeedPurchase.SetActive(true);
                break;
            case 53:
                player.LevelSpeedPurchase.SetActive(true);
                player.winText.gameObject.SetActive(true);
                break;
        }
        isPurchased = true;
        upgradeHandler.purchasedUpgrades.Add(this);
        player.UpdateDisplays();
        image.color = new Color(0f, .8f, 1f);
    }

    //Returns a value that is used in calculations to increase/decrease a number.
    public double Effect
    {
        get
        {
            switch (id)
            {
                //Coin Upgrades
                case 102:
                    return Math.Log10(player.Clickpoints + 1) / 2 + 1;
                case 103:
                    return player.Level * 0.1 + 1;
                case 104:
                    return Math.Sqrt(multi.GetMultiplier());
                case 105:
                    return autoclicker.SpeedMultiplier;
                case 32:
                    return 1 + (autoclicker.SurgeDuration * 0.05);
                case 41:
                    return player.ExperiencePerClick * player.diamondCoins * 0.000225;
                case 51:
                    return progressBar.barMulti[0];

                //Clickpoint Upgrades
                case 201:
                    return Math.Log10(player.Coins + 1) / 2 + 1;
                case 203:
                    return player.Level * 0.1 + 1;
                case 204:
                    return Math.Sqrt(multi.GetMultiplier());
                case 205:
                    return autoclicker.SpeedMultiplier;
                case 33:
                    return 1 + (autoclicker.SurgeDuration * 0.05);
                case 42:
                    return 1 + (player.diamondCoins * 0.0015);

                //Experience Upgrades
                case 301:
                    return Math.Log10(player.Coins + 1) / 4 + 1;
                case 302:
                    return Math.Log10(player.Clickpoints + 1) / 4 + 1;
                case 304:
                    return Math.Sqrt(multi.GetMultiplier());
                case 305:
                    return autoclicker.SpeedMultiplier;

                //Multiplier Upgrades
                case 402:
                    return 1 + player.Clickpoints / 1000;
                case 403:
                    return player.Level * 0.02 + 1;
                case 405:
                    return autoclicker.autoclickerPower + 1;

                //Autoclicker Upgrades
                case 501:
                    return Math.Pow(player.Coins, 0.1);
                case 502:
                    return (clicker.coinIsHeld ? 1.5 : 1);
                case 503:
                    return 1 + player.Level * 0.02;
                case 504:
                    return Math.Floor(multi.Level * 0.1);

                //Dropping Coin Upgrades
                case 27:
                    return Math.Log10(player.Clickpoints + 1) * 0.03;
                case 28:
                    return 12;
                case 29:
                    return 1 / (1 + player.Level / 600f);
                case 30:
                    return 0.985;
                case 31:
                    return multi.Level * 0.0075f;
                case 34:
                    return 0.07;
                case 35:
                    return 1 / (Math.Log10(player.Coins + 1) * 0.025 + 1);
                case 40:
                    return 1;

                //Diamond Coin Upgrades
                case 45:
                    return multi.DiamondCoinMulti;
                case 46:
                    return 1 + (player.Level * 0.001);
                case 47:
                    return 1 + (Math.Log10(player.Coins) * 0.0225);
                case 54:
                    return 1 + (Math.Log10(player.Clickpoints) * 0.03);


                default:
                    return 1;
            }
        }
    }

    public string GetFormattedEffect() {
        return NumberFormatter.instance.FormatNumber(Effect);
    }

    public override void UpdateTooltipText()
    {
        switch (id)
        {
            //Coin Upgrades
            case 102:
                stringToDisplay = "Clickpoints boost coins.";
                DisplayEffect();
                break;

            case 103:
                stringToDisplay = "Levels boost coins.";
                DisplayEffect();
                break;

            case 104:
                stringToDisplay = "Boosters gives an additional boost to coins.";
                DisplayEffect();
                break;

            case 105:
                stringToDisplay = "Apply the autoclicker speed multiplier to coins.";
                DisplayEffect();
                break;

            //Clickpoint Upgrades
            case 201:
                stringToDisplay = "Coins boost clickpoints.";
                DisplayEffect();
                break;

            case 203:
                stringToDisplay = "Levels boost clickpoints.";
                DisplayEffect();
                break;

            case 204:
                stringToDisplay = "Boosters gives an additional boost to clickpoints.";
                DisplayEffect();
                break;

            case 205:
                stringToDisplay = "Apply the autoclicker speed multiplier to clickpoints.";
                DisplayEffect();
                break;

            //Experience Upgrades
            case 301:
                stringToDisplay = "Coins boost experience.";
                DisplayEffect();
                break;

            case 302:
                stringToDisplay = "Clickpoints boost experience.";
                DisplayEffect();
                break;

            case 304:
                stringToDisplay = "Boosters gives an additional boost to experience.";
                DisplayEffect();
                break;

            case 305:
                stringToDisplay = "Apply the autoclicker speed multiplier to experience.";
                DisplayEffect();
                break;

            //Booster Upgrades
            case 401:
                stringToDisplay = "Reduce booster cost scaling by <color=#90ee90>0.5</color>.";
                break;

            case 402:
                stringToDisplay = "Clickpoints reduces booster cost.";
                DisplayEffect();
                break;

            case 403:
                stringToDisplay = "Levels increase the effectiveness of boosters.";
                DisplayEffect();
                break;

            case 405:
                stringToDisplay = "Autoclicker power further reduces booster cost.";
                DisplayEffect();
                break;

            //Autoclicker upgrades
            case 501:
                stringToDisplay = "Coins boost autoclicker power.";
                DisplayEffect();
                break;

            case 502:
                stringToDisplay = "Holding down the coin boosts autoclicker speed by <color=#90ee90>1.5x</color>.";
                break;

            case 503:
                stringToDisplay = "Levels boost autoclicker speed.";
                DisplayEffect();
                break;

            case 504:
                stringToDisplay = "Get a free autoclicker level every 10 multiplier levels.\n" +
                    "Currently: +{0}";
                objects.Add((int)Effect);
                break;

            default:
            stringToDisplay = "<color=red>A tooltip should be here but it is missing.\nPlease report this bug.</color>";
            break;
        }
        stringToDisplay += "\nUpgrade ID: " + id;
    }

    void DisplayEffect() {
        objects.Add(GetFormattedEffect());
        stringToDisplay += "\n<color=#90ee90>Currently: {0}x</color>";
    }
}
*/