using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player instance;

    private Multiplier multi;
    private Autoclicker autoclicker;
    private ProgressBarHandler progressBar;
    private UpgradeHandler upgradeHandler;

    private double coins;
    public double CoinsPerClick
    {
        get {
            double d = 1;
            d *= upgradeHandler.GetTotalEffect(1, 4, 18, 32, 51);
            d *= progressBar.GetTotalMultiplier();
            d *= multi.CoinMulti;
            return d;
        }
    }
    public double BonusCoinsPerClick
    {
        get
        {
            double d = 0;
            if (IsUpgradePurchased(41))
                d += ExperiencePerClick * diamondCoins * 0.000225;
            return d;
        }
    }
    public double clickpoints;
    public double ClickpointsPerClick {
        get
        {
            double d = 1;
            d *= upgradeHandler.GetTotalEffect(2, 6, 9, 19, 33, 42);
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
    public double ExperiencePerClick
    {
        get
        {
            double d = CoinsPerClick;
            d *= upgradeHandler.GetTotalEffect(5, 7, 11, 20);
            return d;
        }
    }

    public double Coins { get => coins;
        set {
            coins = value;
            coinsDisplay.text = NumberFormatter.instance.FormatNumber(value);
        }
    }

    public double experienceNeededToLevelUp;
    public int level;

    public double diamondCoins;

    public float timeSinceLastUpgrade;

    public Text coinsDisplay;
    public Text clickpointsDisplay;
    public Text levelDisplay;
    public Text experienceDisplay;
    public Slider experienceBar;
    public Slider coinDropBar;
    public Text diamondDisplay;
    public Text winText;

    public GameObject DiamondIcon;

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
        progressBar = ProgressBarHandler.instance;
        upgradeHandler = UpgradeHandler.instance;
    }
	
    public void UpdateDisplays() {
        if(IsUpgradePurchased(0))
            clickpointsDisplay.text = NumberFormatter.instance.FormatNumber(clickpoints);
        if (IsUpgradePurchased(3))
            UpdateExperienceDisplays();
        if (IsUpgradePurchased(36))
            diamondDisplay.text = diamondCoins.ToString("N1");
    }

    void UpdateExperienceDisplays() {
        experienceBar.value = Convert.ToSingle(experience / experienceNeededToLevelUp);
        experienceDisplay.text = NumberFormatter.instance.FormatNumber(experience) + " / " + NumberFormatter.instance.FormatNumber(experienceNeededToLevelUp);
        levelDisplay.text = level.ToString();
    }

    public void UnlockLevels()
    {
        levelDisplay.transform.parent.gameObject.SetActive(true);
        experienceBar.gameObject.SetActive(true);
    }

    public void UnlockMultiplier()
    {
        MultiplierPanel.SetActive(true);
        UnlockNavigationTab(0);
    }

    public void UnlockAutoclicker() {
        AutoclickerPanel.SetActive(true);
        UnlockNavigationTab(1);
    }

    public void UnlockAutoclickerUpgrades() {
    }

    public void UnlockCoinDrop() {
        coinDropBar.gameObject.SetActive(true);
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
        UnlockNavigationTab(2);
    }

    public void UnlockProgressBars() {
        PanelSwitcher.SetActive(true);
        ProgressBarPanel.SetActive(true);
        UnlockNavigationTab(3);
    }

    void LevelUp() {
        experience -= experienceNeededToLevelUp;
        Coins += experienceNeededToLevelUp * 0.15;
        if (!(IsUpgradePurchased(43) && UnityEngine.Random.Range(0f, 1f) < (Math.Log10(diamondCoins + 1) * 0.1)))
            experienceNeededToLevelUp *= 1.15;
        level++;
        UpdateDisplays();
    }

    void UnlockNavigationTab(int number) {
        NavigationTabs[number].SetActive(true);
    }

    bool IsUpgradePurchased(int id) {
        return upgradeHandler.IsUpgradePurchased(id);
    }
}
