using UnityEngine;
using System.Collections.Generic;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;

    public float timeUntilSave;

    public Player player;
    public Multiplier multiplier;
    public Autoclicker autoclicker;
    public CoinDrop coinDrop;
    public DiamondUpgrades platinum;
    public ProgressBarHandler progressBar;
    public Options options;
    private UpgradeHandler upgradeHandler;
    private TierHandler tierHandler;

    private void Awake()
    {
        instance = this;
    }

    void Start() {
        player = Player.instance;
        upgradeHandler = UpgradeHandler.instance;
        tierHandler = TierHandler.instance;
        LoadGameData();
    }

    private void Update()
    {
        timeUntilSave -= Time.deltaTime;
        if (timeUntilSave <= 0f)
            SaveGameData();
    }

    public static string Load(string key, string defaultData) {
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefs.GetString(key);
        }
        else {
            PlayerPrefs.SetString(key, defaultData);
            return defaultData;
        }
    }

    public static void Save(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public void SaveGameData() {
        //Upgrades
        foreach (KeyValuePair<int, Upgrade> upgrade in upgradeHandler.upgrades)
        {
            int id = upgrade.Key;
            Save("Upgrade" + id + "Purchased", upgrade.Value.isPurchased.ToString());
        }

        //Main Stats (Related to gameplay)
        Save("Coins", player.Coins.ToString("R"));
        Save("Level", player.Level.ToString());
        Save("Experience", player.Experience.ToString("R"));
        Save("ExperienceReq", player.experienceNeededToLevelUp.ToString("R"));
        Save("MultiLevel", multiplier.level.ToString());
        Save("Clickpoints", player.Clickpoints.ToString("R"));
        Save("AutoclickerLevel", autoclicker.autoclickerLevel.ToString());
        Save("AutoclickerBonus", autoclicker.autoclickerPower.ToString("R"));
        Save("AutoclickerSurge", autoclicker.SurgeDuration.ToString("R"));
        Save("DropCooldown", coinDrop.TimePerDrop().ToString("R"));
        Save("FreeLevels", multiplier.freeLevels.ToString());
        Save("PlatinumCoins", player.diamondCoins.ToString("R"));
        Save("PlatinumAuto", platinum.diamondAutoLevels.ToString());
        Save("PlatinumMulti", platinum.diamondMultiLevels.ToString());
        Save("PlatinumDrop", platinum.dropPurchases.ToString());
        Save("DropCount", coinDrop.dropCount.ToString());
        Save("Tier", tierHandler.tier.ToString());

        //Progress Bar Stats
        for (int i = 0; i < 6; i++)
        {
            Save("ProgressBar" + i + "Multi", progressBar.barMulti[i].ToString("R"));
            Save("ProgressBar" + i + "Time", progressBar.timeLeft[i].ToString("R"));
        }

        Save("SpeedMultiLevel", progressBar.speedMultiLevel.ToString());
        Save("SpeedMultiLevel2", progressBar.speedMultiLevel2.ToString());

        //Options
        Save("Music", options.music.ToString());
        Save("SFX", options.sfx.ToString());
        Save("ClickParticles", options.clickParticles.ToString());
        Save("Bell", options.bell.ToString());
        Save("CollectionSound", options.collectionSound.ToString());
        Save("CollectionParticles", options.collectionParticles.ToString());
        Save("UseLogarithm", options.useLogarithm.ToString());
        Save("FormatSmallNumbers", options.formatSmallNumbers.ToString());

        //Optional Stats (Does not affect gameplay)
        //Insert Stats Here...

        timeUntilSave = 60f;
    }

    public void LoadGameData() {
        //Main Stats (Related to gameplay)
        player.Coins = double.Parse(Load("Coins", "0"));
        player.Clickpoints = double.Parse(Load("Clickpoints", "0"));
        player.Experience = double.Parse(Load("Experience", "0"));
        player.experienceNeededToLevelUp = double.Parse(Load("ExperienceReq", "16"));
        multiplier.level = int.Parse(Load("MultiLevel", "0"));
        player.Level = int.Parse(Load("Level", "0"));
        autoclicker.autoclickerLevel = int.Parse(Load("AutoclickerLevel", "1"));
        autoclicker.autoclickerPower = double.Parse(Load("AutoclickerBonus", "0"));
        autoclicker.SurgeDuration = float.Parse(Load("AutoclickerSurge", "0"));
        coinDrop.dropCooldown = float.Parse(Load("DropCooldown", "60"));
        multiplier.freeLevels = int.Parse(Load("FreeLevels", "0"));
        player.diamondCoins = double.Parse(Load("PlatinumCoins", "0"));
        platinum.diamondAutoLevels = int.Parse(Load("PlatinumAuto", "0"));
        platinum.diamondMultiLevels = int.Parse(Load("PlatinumMulti", "0"));
        platinum.dropPurchases = int.Parse(Load("PlatinumDrop", "0"));
        coinDrop.dropCount = int.Parse(Load("DropCount", "0"));

        //Tier
        int targetTier = int.Parse(Load("Tier", "0"));
        while (tierHandler.tier < targetTier) {
            tierHandler.BuyTier();
        }

        //Upgrades
        foreach (KeyValuePair<int, Upgrade> upgrade in upgradeHandler.upgrades)
        {
            bool purchased = bool.Parse(Load("Upgrade" + upgrade.Key + "Purchased", "FALSE"));
            if (purchased)
            {
                upgrade.Value.PurchaseUpgrade();
            }
        }

        //Progress Bar Stats
        for (int i = 0; i < 6; i++)
        {
            progressBar.barMulti[i] = double.Parse(Load("ProgressBar" + i + "Multi", "1"));
            progressBar.timeLeft[i] = float.Parse(Load("ProgressBar" + i + "Time", Mathf.Pow(100,i).ToString("R")));
        }
        progressBar.speedMultiLevel = int.Parse(Load("SpeedMultiLevel", "1"));
        progressBar.speedMultiLevel2 = int.Parse(Load("SpeedMultiLevel2", "1"));

        //Options
        options.music = bool.Parse(Load("Music", "FALSE"));
        options.sfx = bool.Parse(Load("SFX", "FALSE"));
        options.clickParticles = bool.Parse(Load("ClickParticles", "TRUE"));
        options.bell = bool.Parse(Load("Bell", "FALSE"));
        options.collectionSound = bool.Parse(Load("CollectionSound", "FALSE"));
        options.collectionParticles = bool.Parse(Load("CollectionParticles", "TRUE"));
        options.useLogarithm = bool.Parse(Load("UseLogarithm", "FALSE"));
        options.formatSmallNumbers = bool.Parse(Load("FormatSmallNumbers", "FALSE"));
        options.UpdateOptions();

        //Optional Stats (Does not affect gameplay)
        //Insert Stats Here...
    }
}


