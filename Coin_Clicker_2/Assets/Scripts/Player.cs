using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player instance;

    private Multiplier multi;
    private Autoclicker autoclicker;
    private ProgressBarHandler progressBar;
    private UpgradeHandler upgradeHandler;

    [SerializeField]
    private double coins;
    public double CoinsPerClick
    {
        get {
            double d = 0.1;
            d *= upgradeHandler.GetTotalEffect(102, 103, 18, 32, 51);
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
    private double clickpoints;
    public double ClickpointsPerClick {
        get
        {
            double d = 0.1;
            d *= upgradeHandler.GetTotalEffect(201, 203, 204, 19, 33, 42);
            return d;
        }
    }

    public double experience;
    public double Experience {
        get => experience;
        set
        {
            experience = value;
            if (experience >= experienceNeededToLevelUp)
                LevelUp();
            UpdateExperienceDisplays();
        }
    }
    public double ExperiencePerClick
    {
        get
        {
            double d = CoinsPerClick;
            d *= upgradeHandler.GetTotalEffect(301, 302, 304, 20);
            return d;
        }
    }

    public double Coins {
        get => coins;
        set {
            coins = value;
            coinsDisplay.text = NumberFormatter.instance.FormatNumber(value);
        }
    }

    public double Clickpoints {
        get => clickpoints;
        set {
            clickpoints = value;
            clickpointsDisplay.text = NumberFormatter.instance.FormatNumber(Clickpoints);
        }
    }

    public int Level {
        get => level;
        set
        {
            level = value;
            levelDisplay.text = level.ToString();
        }
    }

    public double experienceNeededToLevelUp;
    private int level;

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
        if (IsUpgradePurchased(3))
            UpdateExperienceDisplays();
        if (IsUpgradePurchased(36))
            diamondDisplay.text = diamondCoins.ToString("N1");
    }

    void UpdateExperienceDisplays() {
        experienceBar.value = Convert.ToSingle(experience / experienceNeededToLevelUp);
        experienceDisplay.text = NumberFormatter.instance.FormatNumber(experience) + " / " + NumberFormatter.instance.FormatNumber(experienceNeededToLevelUp);
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
        //UnlockNavigationTab(2);
    }

    public void UnlockProgressBars() {
        PanelSwitcher.SetActive(true);
        ProgressBarPanel.SetActive(true);
        //UnlockNavigationTab(3);
    }

    void LevelUp() {
        experience -= experienceNeededToLevelUp;
        Coins += experienceNeededToLevelUp;
        if (!(IsUpgradePurchased(43) && UnityEngine.Random.Range(0f, 1f) < (Math.Log10(diamondCoins + 1) * 0.1)))
            experienceNeededToLevelUp *= 2;
        Level++;
    }

    bool IsUpgradePurchased(int id) {
        return upgradeHandler.IsUpgradePurchased(id);
    }
}
