using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CoinDrop : MonoBehaviour
{
    public static CoinDrop instance;

    private Player player;
    private Autoclicker auto;
    private Clicker clicker;
    private Multiplier multi;
    private Options options;
    private UpgradeHandler upgradeHandler;

    public float dropCooldown;
    public GameObject coinInstance;
    public GameObject coinParticle;
    GameObject clickedCoin;

    public Slider DropTimer;

    public AudioSource coinDropDing;
    public AudioSource dropCollect;

    public GameObject DisplayPrefab;

    public Color DiamondColor;
    public Color PlatinumColor;

    public double ResourceMultiplier() {
        double d = 160;
        if (upgradeHandler.IsUpgradePurchased(26)) {
            double clicks = auto.clicksPerSec * 7.5;
            if (clicker.coinIsHeld && upgradeHandler.IsUpgradePurchased(16))
                clicks /= 1.15;
            if (auto.surgeTimeRemaining > 0)
                clicks /= 2;
            d += clicks;
        }
        return d;
    }

    public double DiamondCoinsPerDrop() {
        double d = 1 + (0.01d * dropCount);
        d *= upgradeHandler.GetTotalEffect(45, 46);
        if (isConvertEnabled)
            d *= ConvertMultiplier();
        return d;
    }

    double ConvertMultiplier() {
        return upgradeHandler.GetTotalEffect(47, 54);
    }

    public float TimePerDrop() {
        double d = 60 * upgradeHandler.GetTotalEffect(29);
        return Convert.ToSingle(d);
    }

    public int dropsLeft;
    public int TotalDrops() {
        int i = 1;
        double chance = upgradeHandler.GetTotalEffect(false, 27, 31);
        if (UnityEngine.Random.Range(0f, 1f) < chance)
            i++;
        return i;
    }

    public int dropCount;

    public bool platinum;
    public Sprite platinumSprite;

    public bool isConvertEnabled;
    public Text convertText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        auto = Autoclicker.instance;
        clicker = Clicker.instance;
        options = Options.instance;
        multi = Multiplier.instance;
        upgradeHandler = UpgradeHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (upgradeHandler.IsUpgradePurchased(25))
            UpdateDropCooldown();
        if (!isConvertEnabled)
            convertText.text = "Sacrifice disabled";
        else
            convertText.text = "Sacrifice enabled\n" + ConvertMultiplier().ToString("N2") + "x diamond coins";
    }

    void UpdateDropCooldown() {
        dropCooldown -= Time.deltaTime;
        DropTimer.value = (TimePerDrop() - dropCooldown) / TimePerDrop();
        if (dropCooldown <= 0f)
        {
            if (dropsLeft == 0)
                dropsLeft += TotalDrops();
            Drop();
            if (dropsLeft > 0)
                dropCooldown += 0.5f;
            else
                dropCooldown += TimePerDrop();
        }
    }

    public void Drop() {
        dropsLeft--;
        SpawnCoin();
        if (options.bell)
            coinDropDing.Play();
    }

    void SpawnCoin() {
        Vector3 randomPosOffset = new Vector3(UnityEngine.Random.Range(-300f, 300f), 0);
        GameObject coinObject = (GameObject)Instantiate(coinInstance, transform.position + randomPosOffset, Quaternion.identity, transform.parent);

        Button coinButton = coinObject.GetComponent<Button>();
        coinButton.onClick.AddListener(() => OnCoinClick(coinObject));

        Rigidbody2D rigidbody = coinObject.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale *= Convert.ToSingle(upgradeHandler.GetEffect(35));

        if (platinum)
        {
            coinObject.GetComponent<Image>().sprite = platinumSprite;
            platinum = false;
        }
    }

    void CollectDiamondCoin() {
        GameObject Display = Instantiate(DisplayPrefab, clickedCoin.transform.position, Quaternion.identity, clickedCoin.transform.parent);
        Text DisplayText = Display.GetComponent<CoinDisplay>().display;
        if (clickedCoin.GetComponent<Image>().sprite == platinumSprite)
        {
            player.diamondCoins += DiamondCoinsPerDrop() * 2;
            DisplayText.text = NumberFormatter.instance.FormatNumber(DiamondCoinsPerDrop() * 2);
            DisplayText.color = PlatinumColor;
        }
        else {
            player.diamondCoins += DiamondCoinsPerDrop();
            DisplayText.text = NumberFormatter.instance.FormatNumber(DiamondCoinsPerDrop());
            DisplayText.color = DiamondColor;
        }
    }

    void SpawnCoinParticles() {
        for (int i = 0; i < 25; i++)
        {
            GameObject coin = Instantiate(coinParticle, clickedCoin.transform.position, Quaternion.identity, player.ParticleHolder);
            Rigidbody2D rigidbody = coin.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(UnityEngine.Random.Range(-500f, 500f), UnityEngine.Random.Range(1000f, 1500f));
            coin.GetComponent<Image>().sprite = clickedCoin.GetComponent<Image>().sprite;
            Destroy(coin, 3f);
        }
    }

    void GiveResources() {
        player.Coins += (player.CoinsPerClick + player.BonusCoinsPerClick) * ResourceMultiplier();
        if (upgradeHandler.IsUpgradePurchased(0))
            player.clickpoints += player.ClickpointsPerClick * ResourceMultiplier();
        if (upgradeHandler.IsUpgradePurchased(3))
            player.Experience += player.ExperiencePerClick * ResourceMultiplier();
        if (upgradeHandler.IsUpgradePurchased(36))
            CollectDiamondCoin();
    }

    void ConvertCoins() {
        player.Coins = 0;
        if (upgradeHandler.IsUpgradePurchased(54))
        {
            player.clickpoints = 0;
        }
    }

    void PlayCollectionSound() {
        dropCollect.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        dropCollect.Play();
    }

    public void OnCoinClick(GameObject coinObject) {
        clickedCoin = coinObject;
        GiveResources();
        auto.surgeTimeRemaining += upgradeHandler.GetEffect(28);
        player.experienceNeededToLevelUp *= upgradeHandler.GetEffect(30);
        if (UnityEngine.Random.Range(0f,1f) < upgradeHandler.GetEffect(34))
            multi.freeLevels++;
        dropCount += Convert.ToInt32(upgradeHandler.GetEffect(40));
        if (isConvertEnabled)
            ConvertCoins();
        if (options.collectionParticles)
            SpawnCoinParticles();
        if (options.collectionSound)
            PlayCollectionSound();
        Destroy(coinObject);
    }

    public void ToggleConvert() {
        isConvertEnabled = !isConvertEnabled;
    }
}
