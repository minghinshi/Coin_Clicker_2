using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player instance;

    private Multiplier multi;
    private Autoclicker autoclicker;
    private ProgressBar progressBar;

    public bool devMode;

    public double coins;
    public double coinsPerClick
    {
        get {
            double d = 1;
            if (purchasedUpgrade[1])
                d += clickpoints * 0.01;
            if (purchasedUpgrade[4])
                d *= 1 + (level * 0.25);
            d *= multi.coinMulti;
            if(purchasedUpgrade[18])
                d *= 1 + autoclicker.bonus;
            if (purchasedUpgrade[32] && autoclicker.surgeTimeRemaining > 0f)
                d *= 1 + (autoclicker.surgeTimeRemaining * 0.01);
            d *= progressBar.TotalMulti();
            if (purchasedUpgrade[51])
                d *= progressBar.barMulti[0];
            return d;
        }
    }
    public double bonusCoinsPerClick
    {
        get
        {
            double d = 0;
            if (purchasedUpgrade[41])
                d = experiencePerClick * diamondCoins * 0.01;
            return d;
        }
    }
    public double clickpoints;
    public double clickpointsPerClick {
        get
        {
            double d = 1;
            if (purchasedUpgrade[2])
                d += Math.Pow(Math.Max(coins,1), 0.1) - 1;
            if (purchasedUpgrade[6])
                d += level;
            if (purchasedUpgrade[9])
                d *= multi.multiplier;
            if(purchasedUpgrade[19])
                d *= 1 + autoclicker.bonus;
            if (purchasedUpgrade[33] && autoclicker.surgeTimeRemaining > 0f)
                d *= 1 + (autoclicker.surgeTimeRemaining * 0.01);
            if (purchasedUpgrade[42])
                d *= 1 + (diamondCoins * 0.01);
            return d;
        }
    }

    public double experience;
    public double Experience {
        get { return experience; }
        set
        {
            experience = value;
            if (experience >= experienceNeededToLevelUp)
                LevelUp();
        }
    }
    public double experiencePerClick
    {
        get
        {
            double d = coinsPerClick;
            if (purchasedUpgrade[5])
                d += clickpoints * 0.01;
            if(purchasedUpgrade[7])
                d *= Math.Pow(Math.Max(coins, 1), 0.1);
            if (purchasedUpgrade[11])
                d *= multi.multiplier;
            if (purchasedUpgrade[20])
                d *= 1 + autoclicker.bonus;
            return d;
        }
    }
    public double experienceNeededToLevelUp;
    public int level;

    public double diamondCoins;

    public bool[] purchasedUpgrade;

    public Text coinsDisplay;
    public Text clickpointsDisplay;
    public Text levelDisplay;
    public Text experienceDisplay;
    public Slider experienceBar;
    public Slider coinDropBar;
    public Text diamondDisplay;
    public Text winText;

    public GameObject ClickpointsIcon;
    public GameObject DiamondIcon;
    private GameObject[] upgrades;
    public int GreatestUpgradeID() {
        int i = 0;
        foreach (Upgrades upgrade in FindObjectsOfType<Upgrades>()) {
            if (upgrade.id > i)
                i = upgrade.id;
        }
        return i;
    }

    public GameObject MultiplierPanel;
    public GameObject AutoclickerPanel;
    public GameObject DiamondPanel;
    public GameObject PanelSwitcher;
    public GameObject ProgressBarPanel;
    public GameObject[] ExtraOptions;

    public GameObject DiamondAutoPurchase;
    public GameObject DiamondMultiPurchase;
    public GameObject DropPurchase;
    public GameObject ConvertToggle;
    public GameObject DiamondSpeedPurchase;
    public GameObject LevelSpeedPurchase;

    public GameObject[] NavigationTabs;

    public Transform ParticleHolder;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        multi = Multiplier.instance;
        autoclicker = Autoclicker.instance;
        progressBar = ProgressBar.instance;

        upgrades = new GameObject[GreatestUpgradeID()];
        purchasedUpgrade = new bool[GreatestUpgradeID() + 1];
        SortUpgrades();
        SaveLoad.instance.LoadGameData();
    }
	
    public void UpdateDisplays() {
        coinsDisplay.text = NumberFormatter.instance.FormatNumber(coins);
        if(purchasedUpgrade[0])
            clickpointsDisplay.text = NumberFormatter.instance.FormatNumber(clickpoints);
        if (purchasedUpgrade[3])
        {
            experienceBar.value = Convert.ToSingle(experience / experienceNeededToLevelUp);
            experienceDisplay.text = NumberFormatter.instance.FormatNumber(experience) + " / " + NumberFormatter.instance.FormatNumber(experienceNeededToLevelUp);
            levelDisplay.text = level.ToString();
        }
        if (purchasedUpgrade[36])
            diamondDisplay.text = diamondCoins.ToString("N1");
    }

    public void UnlockClickpoints() {
        ClickpointsIcon.SetActive(true);
        clickpointsDisplay.gameObject.SetActive(true);
        UnlockUpgrades(new int[] {0,1,2,14});
    }

    public void UnlockLevels()
    {
        levelDisplay.transform.parent.gameObject.SetActive(true);
        experienceBar.gameObject.SetActive(true);
        UnlockUpgrades(new int[] {3,4,5,6,7});
    }

    public void UnlockMultiplier()
    {
        MultiplierPanel.SetActive(true);
        UnlockUpgrades(new int[] {8,9,10,11,12,13,15,24});
        UnlockNavigationTab(0);
    }

    public void UnlockAutoclicker() {
        AutoclickerPanel.SetActive(true);
        UnlockNavigationTab(1);
    }

    public void UnlockAutoclickerUpgrades() {
        UnlockUpgrades(new int[] {16,17,18,19,20,21,22,23});
    }

    public void UnlockCoinDrop() {
        coinDropBar.gameObject.SetActive(true);
        UnlockUpgrades(new int[]{25,26,27,28,29,30,31,32,33,34,35});
        foreach (GameObject option in ExtraOptions)
        {
            option.SetActive(true);
        }
    }

    public void UnlockDiamond()
    {
        DiamondIcon.SetActive(true);
        diamondDisplay.gameObject.SetActive(true);
        DiamondPanel.SetActive(true);
        UnlockUpgrades(new int[] {36,37,38,39,40,41,42,43,44,45,46,47,53});
        UnlockNavigationTab(2);
    }

    public void UnlockProgressBars() {
        PanelSwitcher.SetActive(true);
        ProgressBarPanel.SetActive(true);
        UnlockUpgrades(new int[] {48,49,50,51,52});
        UnlockNavigationTab(3);
    }

    void LevelUp() {
        experience -= experienceNeededToLevelUp;
        coins += experienceNeededToLevelUp;
        if (purchasedUpgrade[43])
            experienceNeededToLevelUp *= Math.Pow(2, 1/Math.Log10(diamondCoins + 10));
        else
            experienceNeededToLevelUp *= 2;
        level++;
        UpdateDisplays();
    }

    void SortUpgrades() {
        foreach (Upgrades upgrade in FindObjectsOfType<Upgrades>()) {
            if (upgrade.id != 0)
            {
                upgrades[upgrade.id - 1] = upgrade.gameObject;
                upgrade.gameObject.SetActive(false);
            }
        }
    }

    void UnlockUpgrades(int[] allUpgradeID) {
        foreach (int upgradeID in allUpgradeID) {
            upgrades[upgradeID].SetActive(true);
        }
    }

    void UnlockNavigationTab(int number) {
        NavigationTabs[number].SetActive(true);
    }
}
