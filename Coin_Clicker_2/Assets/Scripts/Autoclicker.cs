using System;
using UnityEngine;
using UnityEngine.UI;

public class Autoclicker : MonoBehaviour
{
    public static Autoclicker instance;

    private Player player;
    private Clicker clicker;
    private Multiplier multi;
    private DiamondUpgrades platinum;
    private CoinDrop drop;
    private UpgradeHandler upgradeHandler;
    private TierHandler tierHandler;
    private NumberFormatter formatter;

    public float timeUntilClick;
    private double surgeDuration;
    public double ClicksPerSec
    {
        get
        {
            double d = TotalLevel;
            d *= upgradeHandler.GetTotalEffect(502, 503);
            d *= SpeedMultiplier;
            if (SurgeDuration > 0)
                d *= 2;
            return d;
        }
    }
    public double ClickOverflowBonus
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
            double d = Math.Pow(2, autoclickerLevel + 1);
            return d;
        }
    }
    public double autoclickerPower;
    public double PowerIncrease
    {
        get
        {
            double d = 0.01;
            d *= upgradeHandler.GetEffect(501);
            return d;
        }
    }
    public double SpeedMultiplier {
        get
        {
            return Math.Pow(autoclickerPower + 1, 0.2);
        }
    }

    public int autoclickerLevel;
    public int TotalLevel
    {
        get
        {
            int i = autoclickerLevel + platinum.diamondAutoLevels + (int)upgradeHandler.GetEffect(504);
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
        multi = Multiplier.instance;
        platinum = DiamondUpgrades.instance;
        drop = CoinDrop.instance;
        upgradeHandler = UpgradeHandler.instance;
        tierHandler = TierHandler.instance;
        formatter = NumberFormatter.instance;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilClick -= Time.deltaTime * Convert.ToSingle(Math.Min(ClicksPerSec, 50));
        while (timeUntilClick < 0)
            Autoclick();

        progressBar.value = 1 - timeUntilClick;
        if (upgradeHandler.IsUpgradePurchased(28)) UpdateSurgeDuration();
        if (upgradeHandler.IsUpgradePurchased(44))
            if (UnityEngine.Random.Range(0f, 1f) < 1 - Mathf.Pow(0.9999f, Convert.ToSingle(ClickOverflowBonus)))
                drop.platinum = true;
    }

    void Autoclick() {
        timeUntilClick++;
        clicker.Click(ClickOverflowBonus);
 
        if (tierHandler.tier >= 4)
            autoclickerPower += PowerIncrease* ClickOverflowBonus;
    }

    void UpdateSurgeDuration() {
        if (SurgeDuration > 0)
            SurgeDuration -= Time.deltaTime;
    }

    public void Upgrade() {
        if (player.Clickpoints >= Cost) {
            player.Clickpoints -= Cost;
            autoclickerLevel++;

            player.UpdateDisplays();
            UpdateDisplays();
        }
    }

    public void UpdateDisplays() {
        statDisplay.text = ClicksPerSec.ToString("N1") + " clicks/sec";
        costDisplay.text = NumberFormatter.instance.FormatNumber(Cost);

        if (tierHandler.tier >= 4)
            statDisplay.text += String.Format("\n<color=#90ee90>{0}</color> Power\n=> <color=#90ee90>{1}x</color> Speed", formatter.FormatNumber(autoclickerPower), formatter.FormatNumber(SpeedMultiplier));
    }
}
