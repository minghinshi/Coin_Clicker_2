using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinDrop : MonoBehaviour
{
    public static CoinDrop instance;

    private Player player;
    private Options options;

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

    public double ResourceMultiplier()
    {
        double d = Autoclicker.instance.BaseClicksPerSec * 3600
            * UpgradeHandler.GetEffectOfUpgrade(5, 4);
        return d;
    }

    public double DiamondCoinsPerDrop()
    {
        double d = 1 + (0.01d * dropCount);
        /*d *= upgradeHandler.GetTotalEffect(45, 46);*/
        if (isConvertEnabled)
            d *= ConvertMultiplier();
        return d;
    }

    double ConvertMultiplier()
    {
        return 1; /*upgradeHandler.GetTotalEffect(47, 54);*/
    }

    public float TimePerDrop()
    {
        double d = 60 * UpgradeHandler.GetEffectOfUpgrade(5, 2);
        return Convert.ToSingle(d);
    }

    public int dropsLeft;
    public int TotalDrops()
    {
        double p = 1 + UpgradeHandler.GetEffectOfUpgrade(5, 0) + UpgradeHandler.GetEffectOfUpgrade(5, 3);
        return ConvertProbabilityToInt(p);
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
        options = Options.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (TierHandler.instance.GetTier() >= 5)
            UpdateDropCooldown();
        if (!isConvertEnabled)
            convertText.text = "Sacrifice disabled";
        else
            convertText.text = "Sacrifice enabled\n" + ConvertMultiplier().ToString("N2") + "x diamond coins";
    }

    void UpdateDropCooldown()
    {
        dropCooldown -= Time.deltaTime;
        DropTimer.value = (TimePerDrop() - dropCooldown) / TimePerDrop();
        if (dropCooldown <= 0f)
        {
            if (dropsLeft == 0)
                dropsLeft += TotalDrops();
            Drop();
            if (dropsLeft > 0)
                dropCooldown += 1f;
            else
                dropCooldown += TimePerDrop();
        }
    }

    public void Drop()
    {
        dropsLeft--;
        for (int i = 0; i < 1 + ConvertProbabilityToInt(UpgradeHandler.GetEffectOfUpgrade(5, 1)); i++)
            SpawnCoin();
        if (options.bell)
            coinDropDing.Play();
    }

    void SpawnCoin()
    {
        Vector3 randomPosOffset = new Vector3(UnityEngine.Random.Range(-300f, 300f), 0);
        GameObject coinObject = (GameObject)Instantiate(coinInstance, transform.position + randomPosOffset, Quaternion.identity, transform.parent);

        Button coinButton = coinObject.GetComponent<Button>();
        coinButton.onClick.AddListener(() => OnCoinClick(coinObject));

        if (platinum)
        {
            coinObject.GetComponent<Image>().sprite = platinumSprite;
            platinum = false;
        }
    }

    void CollectDiamondCoin()
    {
        GameObject Display = Instantiate(DisplayPrefab, clickedCoin.transform.position, Quaternion.identity, clickedCoin.transform.parent);
        Text DisplayText = Display.GetComponent<CoinDisplay>().display;
        if (clickedCoin.GetComponent<Image>().sprite == platinumSprite)
        {
            player.diamondCoins += DiamondCoinsPerDrop() * 2;
            DisplayText.text = NumberFormatter.FormatNumber(DiamondCoinsPerDrop() * 2);
            DisplayText.color = PlatinumColor;
        }
        else
        {
            player.diamondCoins += DiamondCoinsPerDrop();
            DisplayText.text = NumberFormatter.FormatNumber(DiamondCoinsPerDrop());
            DisplayText.color = DiamondColor;
        }
    }

    void SpawnCoinParticles()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject coin = Instantiate(coinParticle, clickedCoin.transform.position, Quaternion.identity, player.ParticleHolder);
            Rigidbody2D rigidbody = coin.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(UnityEngine.Random.Range(-500f, 500f), UnityEngine.Random.Range(1000f, 1500f));
            coin.GetComponent<Image>().sprite = clickedCoin.GetComponent<Image>().sprite;
            Destroy(coin, 3f);
        }
    }

    void GiveResources()
    {
        player.Coins += (player.CoinsPerClick + player.BonusCoinsPerClick) * ResourceMultiplier();
        player.Clickpoints += player.ClickpointsPerClick * ResourceMultiplier();
        player.Experience += player.ExperiencePerClick * ResourceMultiplier();
        if (TierHandler.instance.GetTier() >= 6)
            CollectDiamondCoin();
    }

    void ConvertCoins()
    {
        player.Coins = 0;
        /*if (upgradeHandler.IsUpgradePurchased(54))
            player.Clickpoints = 0;*/
    }

    void PlayCollectionSound()
    {
        dropCollect.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        dropCollect.Play();
    }

    public void OnCoinClick(GameObject coinObject)
    {
        clickedCoin = coinObject;
        GiveResources();
        if (UpgradeHandler.IsUpgradePurchased(4, 5))
            Autoclicker.instance.SurgeDuration += 1;
        if (UpgradeHandler.IsUpgradePurchased(2, 5))
            player.experienceNeededToLevelUp /= 1.15;
        if (UpgradeHandler.IsUpgradePurchased(3, 5))
            Multiplier.instance.freeLevels++;
        //dropCount += Convert.ToInt32(upgradeHandler.GetEffect(40));
        if (isConvertEnabled)
            ConvertCoins();
        if (options.collectionParticles)
            SpawnCoinParticles();
        if (options.collectionSound)
            PlayCollectionSound();
        Destroy(coinObject);
    }

    public void ToggleConvert()
    {
        isConvertEnabled = !isConvertEnabled;
    }

    public int ConvertProbabilityToInt(double probability) {
        double roundUpProbability = probability - (int)probability;
        return (int)probability + (UnityEngine.Random.Range(0f, 1f) < roundUpProbability ? 1 : 0);
    }
}
