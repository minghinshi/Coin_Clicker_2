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

    public float dropCooldown;
    public GameObject coinInstance;
    public GameObject coinParticle;

    public Slider DropTimer;

    public AudioSource coinDropDing;
    public AudioSource dropCollect;

    public GameObject DisplayPrefab;

    public Color DiamondColor;
    public Color PlatinumColor;

    public double CoinDropMultiplier() {
        double d = 160;
        if (player.purchasedUpgrade[26]) {
            double clicks = auto.clicksPerSec * 7.5;
            if (clicker.coinIsHeld && player.purchasedUpgrade[16])
                clicks /= 1.15;
            if (auto.surgeTimeRemaining > 0)
                clicks /= 2;
            d += clicks;
        }
        return d;
    }

    public double CoinsPerDrop() {
        double d = 1 + (0.01d * dropCount);
        if (player.purchasedUpgrade[45])
            d *= multi.DiamondCoinMulti;
        if (player.purchasedUpgrade[46])
            d *= 1 + (player.level * 0.001);
        if (player.purchasedUpgrade[47] && convertCoins)
            d *= ConvertMultiplier();
        return d;
    }

    public double ConvertMultiplier() {
        double d = 1 + (Math.Log10(player.coins) * 0.0225);
        if (player.purchasedUpgrade[54])
            d *= 1 + (Math.Log10(player.clickpoints) * 0.03);
        return d;
    }

    public float TimePerDrop() {
        float f = 60f;
        if (player.devMode)
            f /= 10f;
        if (player.purchasedUpgrade[29])
            f /= 1 + player.level / 600f;
        return f;
    }

    public int dropsLeft;
    public int TotalDrops() {
        int i = 1;
        float chance = 0f;
        if (player.purchasedUpgrade[27]) {
            chance += Convert.ToSingle(Math.Log10(player.clickpoints + 1) * 0.03);
        }
        if (player.purchasedUpgrade[31])
            chance += multi.Level * 0.0075f;
        if (UnityEngine.Random.Range(0f, 1f) < chance)
            i++;
        return i;
    }

    public int dropCount;

    public bool platinum;
    public Sprite platinumSprite;

    public bool convertCoins;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (player.purchasedUpgrade[25])
        {
            dropCooldown -= Time.deltaTime;
            DropTimer.value = (TimePerDrop() - dropCooldown) / TimePerDrop();
            if (dropCooldown <= 0f)
            {
                if(dropsLeft == 0)
                    dropsLeft += TotalDrops();
                dropsLeft--;
                Drop();
                if (dropsLeft > 0)
                    dropCooldown += 1f;
                else
                    dropCooldown += TimePerDrop();
            }
        }
        if (!convertCoins)
            convertText.text = "Sacrifice disabled";
        else
            convertText.text = "Sacrifice enabled\n" + ConvertMultiplier().ToString("N2") + "x diamond coins";
    }

    public void Drop() {
        Vector3 randomPosOffset = new Vector3(UnityEngine.Random.Range(-300f, 300f), 0);
        GameObject coinObject = (GameObject)Instantiate(coinInstance, transform.position + randomPosOffset, Quaternion.identity, transform.parent);

        Button coinButton = coinObject.GetComponent<Button>();
        coinButton.onClick.AddListener(() => OnCoinClick(coinObject));

        Rigidbody2D rigidbody = coinObject.GetComponent<Rigidbody2D>();
        if (player.purchasedUpgrade[35])
            rigidbody.gravityScale /= Convert.ToSingle(Math.Log10(player.coins + 1) * 0.025 + 1);

        if (options.bell)
            coinDropDing.Play();
        if (platinum)
        {
            coinObject.GetComponent<Image>().sprite = platinumSprite;
            platinum = false;
        }
    }

    public void OnCoinClick(GameObject coinObject) {
        if (player.purchasedUpgrade[28])
            auto.surgeTimeRemaining += 12f;
        if (player.purchasedUpgrade[30])
            player.experienceNeededToLevelUp *= 0.985;
        if (player.purchasedUpgrade[34] && UnityEngine.Random.Range(0f,1f) < 0.07f)
            multi.freeLevels++;

        if (player.purchasedUpgrade[36])
        {
            GameObject Display = Instantiate(DisplayPrefab, coinObject.transform.position, Quaternion.identity, coinObject.transform.parent);
            Text DisplayText = Display.GetComponent<CoinDisplay>().display;
            player.diamondCoins += CoinsPerDrop();
            DisplayText.text = NumberFormatter.instance.FormatNumber(CoinsPerDrop());
            DisplayText.color = DiamondColor;

            if (coinObject.GetComponent<Image>().sprite == platinumSprite)
            {
                player.diamondCoins += CoinsPerDrop();
                DisplayText.text = NumberFormatter.instance.FormatNumber(CoinsPerDrop() * 2);
                DisplayText.color = PlatinumColor;
            }
        }

        if (player.purchasedUpgrade[40])
            dropCount += 1;

        player.coins += (player.coinsPerClick + player.bonusCoinsPerClick) * CoinDropMultiplier();
        if (player.purchasedUpgrade[0])
            player.clickpoints += player.clickpointsPerClick * CoinDropMultiplier();
        if (player.purchasedUpgrade[3])
            player.Experience += player.experiencePerClick * CoinDropMultiplier();

        if (convertCoins)
        {
            player.coins = 0;
            if (player.purchasedUpgrade[54]) {
                player.clickpoints = 0;
            }
        }

        if (options.collectionParticles)
        {
            for (int i = 0; i < 25; i++)
            {
                GameObject coin = Instantiate(coinParticle, coinObject.transform.position, Quaternion.identity, player.ParticleHolder);
                Rigidbody2D rigidbody = coin.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(UnityEngine.Random.Range(-500f, 500f), UnityEngine.Random.Range(1000f, 1500f));
                coin.GetComponent<Image>().sprite = coinObject.GetComponent<Image>().sprite;
                Destroy(coin, 3f);
            }
        }

        dropCollect.pitch = UnityEngine.Random.Range(0.9f, 1.1f);

        if(options.collectionSound)
            dropCollect.Play();

        Destroy(coinObject);
    }

    public void ToggleConvert() {
        convertCoins = !convertCoins;
    }
}
