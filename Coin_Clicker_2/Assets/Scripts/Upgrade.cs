using System;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{

    private Player player;
    private Multiplier multi;
    private Autoclicker autoclicker;
    private ProgressBarHandler progressBar;
    private UpgradeHandler upgradeHandler;
    private Clicker clicker;
    private PurchaseHandler purchaseHandler;

    public int id;
    public bool isPurchased;
    public bool isAdditive = false;

	// Use this for initialization
	void Start () {
        player = Player.instance;
        multi = Multiplier.instance;
        autoclicker = Autoclicker.instance;
        progressBar = ProgressBarHandler.instance;
        upgradeHandler = UpgradeHandler.instance;
        clicker = Clicker.instance;
        purchaseHandler = PurchaseHandler.instance;
	}


    public void AttemptPurchase() {
        if (purchaseHandler.IsAffordable(upgradeHandler.GetCost())) PurchaseUpgrade();
    }

    public void PurchaseUpgrade() {
        switch (id) {
            case 3:
                player.UnlockLevels();
                break;
            case 8:
                player.UnlockMultiplier();
                break;
            case 9:
            case 10:
            case 11:
                multi.UpdateDisplays();
                break;
            case 15:
                player.UnlockAutoclicker();
                break;
            case 16:
                player.UnlockAutoclickerUpgrades();
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
    }

    //Returns a value that is used in calculations to increase/decrease a number.
    public double GetEffect() {
        if (!isPurchased)
        {
            if (isAdditive)
                return 0;
            return 1;
        }
        switch (id) {
            //Coin Upgrades
            case 1:
                return 1 + Math.Log10(player.clickpoints + 1) * 0.1;
            case 4:
                return 1 + player.level * 0.03;
            case 18:
                return 1 + autoclicker.bonus;
            case 32:
                return 1 + (autoclicker.surgeTimeRemaining * 0.05);
            case 41:
                return player.ExperiencePerClick * player.diamondCoins * 0.000225;
            case 51:
                return progressBar.barMulti[0];

            //Clickpoint Upgrades
            case 2:
                return 1 + Math.Log10(player.Coins + 1) * 0.1;
            case 6:
                return 1 + player.level * 0.015;
            case 9:
                return ((multi.GetMultiplier() - 1) * 0.2 + 1);
            case 19:
                return 1 + autoclicker.bonus;
            case 33:
                return 1 + (autoclicker.surgeTimeRemaining * 0.05);
            case 42:
                return 1 + (player.diamondCoins * 0.0015);

            //Experience Upgrades
            case 5:
                return player.clickpoints / 10000;
            case 7:
                return 1 + Math.Log10(player.Coins + 1) * 0.05;
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
                return 1 + player.level * 0.0025;
            case 14:
                return 1 / Math.Pow(player.clickpoints, 0.15);
            case 21:
                return 1 + autoclicker.bonus;

            //Autoclicker Upgrades
            case 17:
                return (clicker.coinIsHeld ? 1.15 : 1);
            case 22:
                return 1 + player.level * 0.0015;
            case 23:
                return Math.Log10(player.Coins + 1) * 6.25e-8;
            case 24:
                return multi.Level * 0.1;

            //Dropping Coin Upgrades
            case 27:
                return Math.Log10(player.clickpoints + 1) * 0.03;
            case 28:
                return 12;
            case 29:
                return 1 / (1 + player.level / 600f);
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
                return 1 + (player.level * 0.001);
            case 47:
                return 1 + (Math.Log10(player.Coins) * 0.0225);
            case 54:
                return 1 + (Math.Log10(player.clickpoints) * 0.03);


            default:
                return 1;
        }
    }
}
