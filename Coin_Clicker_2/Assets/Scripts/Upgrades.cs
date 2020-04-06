﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeCurrencies { coins, clickpoints, diamondCoins }

public class Upgrades : MonoBehaviour {

    private Player player;
    private Multiplier multi;
    private Autoclicker autoclicker;

    public int id;
    public double cost;
    public UpgradeCurrencies currency;

    public Text costDisplay;
    public Image upgradeBox;

	// Use this for initialization
	void Start () {
        player = Player.instance;
        multi = Multiplier.instance;
        autoclicker = Autoclicker.instance;

        costDisplay.text = NumberFormatter.instance.FormatNumber(cost);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Purchase() {
        if (player.coins >= cost && currency == UpgradeCurrencies.coins) {
            player.coins -= cost;
            Upgrade();
        }
        if (player.clickpoints >= cost && currency == UpgradeCurrencies.clickpoints)
        {
            player.clickpoints -= cost;
            Upgrade();
        }
        if (player.diamondCoins >= cost && currency == UpgradeCurrencies.diamondCoins)
        {
            player.diamondCoins -= cost;
            Upgrade();
        }
    }

    public void Upgrade() {
        switch (id) {
            case 0:
                player.UnlockClickpoints();
                break;
            case 3:
                player.UnlockLevels();
                break;
            case 8:
                player.UnlockMultiplier();
                break;
            case 9:
                multi.UpdateDisplays();
                break;
            case 10:
                multi.UpdateDisplays();
                break;
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
        player.purchasedUpgrade[id] = true;
        if(upgradeBox)
            upgradeBox.color = new Color(0.3f, 1f, 0.3f);
        player.UpdateDisplays();
        gameObject.SetActive(false);
    }

    public void UpdateDisplay() {
        costDisplay.text = NumberFormatter.instance.FormatNumber(cost);
    }
}