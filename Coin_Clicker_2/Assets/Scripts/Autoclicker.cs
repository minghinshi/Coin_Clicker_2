using System;
using UnityEngine;
using UnityEngine.UI;

public class Autoclicker : MonoBehaviour
{
    public static Autoclicker instance;

    private Player player;
    private Clicker clicker;
    private DiamondUpgrades platinum;
    private TierHandler tierHandler;

    public float timeUntilClick;
    private double surgeDuration;
    public double BaseClicksPerSec
    {
        get
        {
            double d = 1 + 0.15 * TotalLevel;
            d *= SpeedMultiplierFromPower;
            return d;
        }
    }

    public double ClicksPerSec
    {
        get
        {
            double d = BaseClicksPerSec;
            if (UpgradeHandler.IsUpgradePurchased(4, 1) && Clicker.instance.coinIsHeld)
                d *= 5;
            if (SurgeDuration > 0)
                d *= 20;
            return d;
        }
    }
    public double ClicksPerTick
    {
        get
        {
            double d = Math.Max(1, ClicksPerSec / 50);
            return d;
        }
    }

    public double Cost
    {
        get
        {
            double d = 10 * Math.Pow(1.15, autoclickerLevel);
            return d;
        }
    }
    public double autoclickerPower;
    public double PowerIncrease
    {
        get
        {
            double d = 1
                * UpgradeHandler.GetEffectOfUpgrade(4, 0)
                * UpgradeHandler.GetEffectOfUpgrade(4, 2);
            return d;
        }
    }
    public double SpeedMultiplierFromPower {
        get
        {
            return Math.Pow(1 + autoclickerPower * 0.001, 0.5);
        }
    }

    public int autoclickerLevel;
    public int TotalLevel
    {
        get
        {
            int i = autoclickerLevel + platinum.diamondAutoLevels + (int)UpgradeHandler.GetEffectOfUpgrade(4, 3);
            return i;
        }
    }

    public double SurgeDuration {
        get => surgeDuration;
        set {
            surgeDuration = value;
            if(surgeDuration > 0)
                surgeDisplay.text = "Autoclicker (" + surgeDuration.ToString("N1") + "s)";
            if (surgeDuration < 0) {
                surgeDuration = 0;
                surgeDisplay.text = "Autoclicker";
            }
        }
    }

    public Slider progressBar;

    public Text statDisplay;
    public Text costDisplay;
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
        platinum = DiamondUpgrades.instance;
        tierHandler = TierHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilClick -= Time.deltaTime * Convert.ToSingle(Math.Min(ClicksPerSec, 50));
        while (timeUntilClick < 0)
            Autoclick();

        progressBar.value = 1 - timeUntilClick;
        if (UpgradeHandler.IsUpgradePurchased(5, 4))
            UpdateSurgeDuration();

        /*if (upgradeHandler.IsUpgradePurchased(44))
            if (UnityEngine.Random.Range(0f, 1f) < 1 - Mathf.Pow(0.9999f, Convert.ToSingle(ClickOverflowBonus)))
                drop.platinum = true;*/
    }

    void Autoclick() {
        timeUntilClick++;
        clicker.Click(ClicksPerTick);
 
        if (tierHandler.GetTier() >= 4)
            autoclickerPower += PowerIncrease* ClicksPerTick;
    }

    void UpdateSurgeDuration() {
        if (SurgeDuration > 0)
            SurgeDuration -= Time.deltaTime;
    }

    public void Upgrade() {
        while (player.Clickpoints >= Cost) {
            player.Clickpoints -= Cost;
            autoclickerLevel++;
            if (!Input.GetKey(KeyCode.LeftShift))
                break;
        }
        player.UpdateDisplays();
        UpdateDisplays();
    }

    public void UpdateDisplays() {
        statDisplay.text = NumberFormatter.FormatNumber(ClicksPerSec) + " clicks/s";
        costDisplay.text = NumberFormatter.FormatNumber(Cost);

        if (tierHandler.GetTier() >= 4)
            statDisplay.text += String.Format("\n<color=#90ee90>{0}</color> Power\n=> <color=#90ee90>{1}x</color> Speed", NumberFormatter.FormatNumber(autoclickerPower), NumberFormatter.FormatNumber(SpeedMultiplierFromPower));
    }
}
