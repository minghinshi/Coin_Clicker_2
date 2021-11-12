using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player instance;

    private Multiplier multi;
    private ProgressBarHandler progressBar;

    [SerializeField]
    private double coins;
    public double CoinsPerClick
    {
        get {
            double d = 1;
            d *= UpgradeHandler.GetEffectOfUpgrade(0, 1)
                * UpgradeHandler.GetEffectOfUpgrade(0, 2)
                * UpgradeHandler.GetEffectOfUpgrade(0, 4)
                * progressBar.GetTotalMultiplier()
                * multi.CoinMulti;
            return d;
        }
    }
    public double BonusCoinsPerClick
    {
        get
        {
            double d = 0;
            /*if (IsUpgradePurchased(41))
                d += ExperiencePerClick * diamondCoins * 0.000225;*/
            return d;
        }
    }
    private double clickpoints;
    public double ClickpointsPerClick {
        get
        {
            double d = 1;
            d *= UpgradeHandler.GetEffectOfUpgrade(1, 0)
                * UpgradeHandler.GetEffectOfUpgrade(1, 2)
                * UpgradeHandler.GetEffectOfUpgrade(1, 3)
                * UpgradeHandler.GetEffectOfUpgrade(1, 4);
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
            d *= UpgradeHandler.GetEffectOfUpgrade(2, 0)
                * UpgradeHandler.GetEffectOfUpgrade(2, 1)
                * UpgradeHandler.GetEffectOfUpgrade(2, 3)
                * UpgradeHandler.GetEffectOfUpgrade(2, 4);
            return d;
        }
    }

    public double Coins {
        get => coins;
        set {
            coins = value;
            coinsDisplay.text = NumberFormatter.FormatNumber(value);
        }
    }

    public double Clickpoints {
        get => clickpoints;
        set {
            clickpoints = value;
            clickpointsDisplay.text = NumberFormatter.FormatNumber(Clickpoints);
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
        progressBar = ProgressBarHandler.instance;
    }
	
    public void UpdateDisplays() {
        /*if (IsUpgradePurchased(3))
            UpdateExperienceDisplays();*/
        /*if (IsUpgradePurchased(36))
            diamondDisplay.text = diamondCoins.ToString("N1");*/
    }

    void UpdateExperienceDisplays() {
        experienceBar.value = Convert.ToSingle(experience / experienceNeededToLevelUp);
        experienceDisplay.text = NumberFormatter.FormatNumber(experience) + " / " + NumberFormatter.FormatNumber(experienceNeededToLevelUp);
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
        if (!(UnityEngine.Random.Range(0f, 1f) < UpgradeHandler.GetEffectOfUpgrade(2, 5)))
            experienceNeededToLevelUp *= 1.15;
        Level++;
    }
}
