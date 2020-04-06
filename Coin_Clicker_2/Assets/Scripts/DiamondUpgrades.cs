using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondUpgrades : MonoBehaviour
{
    public static DiamondUpgrades instance;

    public Player player;
    public CoinDrop coinDrop;

    public double autoPrice {
        get
        {
            if (diamondAutoLevels >= 999)
                return (diamondAutoLevels * 0.1 + 0.1) * 10;
            return diamondAutoLevels * 0.1 + 0.1;
        }
    }

    public double multiPrice
    {
        get
        {
            if (diamondMultiLevels >= 999)
                return (diamondMultiLevels * 0.1 + 0.1) * 10;
            return diamondMultiLevels * 0.1 + 0.1;
        }
    }

    public double dropPrice
    {
        get
        {
            if (dropPurchases >= 999)
                return (dropPurchases * 0.1 + 0.1) * 10;
            return dropPurchases * 0.1 + 0.1;
        }
    }

    public int diamondAutoLevels;
    public int diamondMultiLevels;
    public int dropPurchases;

    public Text AutoPriceDisplay;
    public Text MultiPriceDisplay;
    public Text DropPriceDisplay;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        coinDrop = CoinDrop.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.purchasedUpgrade[37])
            AutoPriceDisplay.text = autoPrice.ToString("#,##0.#");
        if (player.purchasedUpgrade[38])
            MultiPriceDisplay.text = multiPrice.ToString("#,##0.#");
        if (player.purchasedUpgrade[39])
            DropPriceDisplay.text = dropPrice.ToString("#,##0.#");
    }

    public void BuyAuto() {
        if (player.diamondCoins >= autoPrice) {
            player.diamondCoins -= autoPrice;
            diamondAutoLevels++;
        }
    }

    public void BuyMulti()
    {
        if (player.diamondCoins >= multiPrice)
        {
            player.diamondCoins -= multiPrice;
            diamondMultiLevels++;
        }
    }

    public void BuyDrop()
    {
        if (player.diamondCoins >= dropPrice)
        {
            player.diamondCoins -= dropPrice;
            dropPurchases++;
            coinDrop.Drop();
        }
    }
}
