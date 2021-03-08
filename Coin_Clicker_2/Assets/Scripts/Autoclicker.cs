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

    public float timeUntilClick;
    public double surgeTimeRemaining;
    public double clicksPerSec
    {
        get
        {
            double d = Level + 1;
            d += upgradeHandler.GetTotalEffect(true, 24);
            d *= upgradeHandler.GetTotalEffect(17, 22);
            if (surgeTimeRemaining > 0)
                d *= 2;
            return d;
        }
    }
    public double clickOverflowBonus
    {
        get
        {
            double d = Math.Max(1, clicksPerSec / 50);
            return d;
        }
    }

    public double cost
    {
        get
        {
            double d = Math.Pow(2, level + 1);
            return d;
        }
    }
    public double bonus;
    public double BonusIncrease()
    {
        double d = Math.Pow(2, -bonus);
        d += upgradeHandler.GetTotalEffect(true, 23);
        return d;
    }

    public int level;
    public int Level
    {
        get
        {
            int i = level + platinum.diamondAutoLevels;
            return i;
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
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilClick -= Time.deltaTime * Convert.ToSingle(Math.Min(clicksPerSec, 50));
        while (timeUntilClick < 0) Autoclick();

        progressBar.value = 1 - timeUntilClick;
        if (upgradeHandler.IsUpgradePurchased(28)) UpdateSurgeDuration();
        if (upgradeHandler.IsUpgradePurchased(44))
            if (UnityEngine.Random.Range(0f, 1f) < 1 - Mathf.Pow(0.9999f, Convert.ToSingle(clickOverflowBonus)))
                drop.platinum = true;
    }

    void Autoclick() {
        timeUntilClick++;
        clicker.Click(clickOverflowBonus);
 
        if (tierHandler.tier >= 4)
            bonus += BonusIncrease() * clickOverflowBonus;
    }

    void UpdateSurgeDuration() {
        if (surgeTimeRemaining > 0)
        {
            surgeTimeRemaining -= Time.deltaTime;
            surgeDisplay.text = "Autoclicker (" + surgeTimeRemaining.ToString("N1") + "s)";
        }
        if (surgeTimeRemaining < 0)
        {
            surgeTimeRemaining = 0;
            surgeDisplay.text = "Autoclicker";
        }
    }

    public void Upgrade() {
        if (player.Clickpoints >= cost) {
            player.Clickpoints -= cost;
            level++;

            player.UpdateDisplays();
            UpdateDisplays();
        }
    }

    public void UpdateDisplays() {
        statDisplay.text = clicksPerSec.ToString("N1") + " clicks/sec";
        costDisplay.text = NumberFormatter.instance.FormatNumber(cost);

        if (tierHandler.tier >= 4)
            statDisplay.text += "\n" + NumberFormatter.instance.FormatNumber(bonus) + " Power";
    }
}
