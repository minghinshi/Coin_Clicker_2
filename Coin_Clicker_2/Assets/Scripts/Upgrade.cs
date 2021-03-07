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
            case 8:
                player.UnlockMultiplier();
                break;
            case 9:
            case 10:
            case 11:
                multi.UpdateDisplays();
                break;
            case 18:
                autoclicker.bonusDisplay.gameObject.SetActive(true);
                break;
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
    public double GetEffect() {
        switch (id) {
            //Coin Upgrades
            case 102:
                return Math.Log10(player.Clickpoints + 1) + 1;
            case 103:
                return Math.Sqrt(player.Level + 1);
            case 18:
                return 1 + autoclicker.bonus;
            case 32:
                return 1 + (autoclicker.surgeTimeRemaining * 0.05);
            case 41:
                return player.ExperiencePerClick * player.diamondCoins * 0.000225;
            case 51:
                return progressBar.barMulti[0];

            //Clickpoint Upgrades
            case 201:
                return Math.Log10(player.Coins + 1) + 1;
            case 203:
                return Math.Sqrt(player.Level + 1);
            case 9:
                return ((multi.GetMultiplier() - 1) * 0.2 + 1);
            case 19:
                return 1 + autoclicker.bonus;
            case 33:
                return 1 + (autoclicker.surgeTimeRemaining * 0.05);
            case 42:
                return 1 + (player.diamondCoins * 0.0015);

            //Experience Upgrades
            case 301:
                return Math.Log10(player.Coins + 1)/2 + 1;
            case 302:
                return Math.Log10(player.Clickpoints + 1)/2 + 1;
            case 11:
                return ((multi.GetMultiplier() - 1) * 0.2 + 1);
            case 20:
                return 1 + autoclicker.bonus;

            //Multiplier Upgrades
            case 10:
                return (multi.GetMultiplier() - 1) * 0.2 + 1;
            case 12:
                return -0.35;
            case 13:
                return 1 + player.Level * 0.0025;
            case 14:
                return 1 / Math.Pow(player.Clickpoints, 0.15);
            case 21:
                return 1 + autoclicker.bonus;

            //Autoclicker Upgrades
            case 17:
                return (clicker.coinIsHeld ? 1.15 : 1);
            case 22:
                return 1 + player.Level * 0.0015;
            case 23:
                return Math.Log10(player.Coins + 1) * 6.25e-8;
            case 24:
                return multi.Level * 0.1;

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

    public string GetFormattedEffect() {
        return NumberFormatter.instance.FormatNumber(GetEffect());
    }

    public override void UpdateTooltipText()
    {
        switch (id)
        {
            //Coin Upgrades
            case 102:
                stringToDisplay = "Clickpoints boost coins.\n" +
                        "<color=#90ee90>Currently: {0}x</color>";
                objects.Add(GetFormattedEffect());
                break;

            case 103:
                stringToDisplay = "Levels boost coins.\n" +
                        "<color=#90ee90>Currently: {0}x</color>";
                objects.Add(GetFormattedEffect());
                break;

            //Clickpoint Upgrades
            case 201:
                stringToDisplay = "Coins boost clickpoints.\n" +
                        "<color=#90ee90>Currently: {0}x</color>";
                objects.Add(GetFormattedEffect());
                break;

            case 203:
                stringToDisplay = "Levels boost clickpoints.\n" +
                        "<color=#90ee90>Currently: {0}x</color>";
                objects.Add(GetFormattedEffect());
                break;

            //Experience Upgrades
            case 301:
                stringToDisplay = "Coins boost experience.\n" +
                        "<color=#90ee90>Currently: {0}x</color>";
                objects.Add(GetFormattedEffect());
                break;

            case 302:
                stringToDisplay = "Clickpoints boost experience.\n" +
                        "<color=#90ee90>Currently: {0}x</color>";
                objects.Add(GetFormattedEffect());
                break;

            default:
            stringToDisplay = "<color=red>A tooltip should be here but it is missing.\nPlease report this bug.</color>";
            break;
        }
        stringToDisplay += "\nUpgrade ID: " + id;
    }
}
