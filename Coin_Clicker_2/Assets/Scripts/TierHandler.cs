using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameObjectsInTier {
    public GameObject[] gameObjects;

    public void SetGameObjectsAsVisible()
    {
        foreach (GameObject go in gameObjects)
            go.SetActive(true);
    }
}

public class TierHandler : TooltipContentHandler
{
    public static TierHandler instance;
    private PurchaseHandler purchaseHandler;
    private UpgradeHandler upgradeHandler;
    private NumberFormatter formatter;

    public GameObjectsInTier[] gameObjectsInTiers;

    public Text TierDisplay;

    public int tier = 0;
    double cost {
        get {
            return Math.Pow(2, Math.Pow(tier + 1, 2));
        }
    }
    public string[] namesOfItemsToUnlock;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        purchaseHandler = PurchaseHandler.instance;
        upgradeHandler = UpgradeHandler.instance;
        tooltip = Tooltip.instance;
        formatter = NumberFormatter.instance;

        upgradeHandler.UnlockUpgrades(1);
    }

    public override void UpdateTooltipText()
    {
        stringToDisplay = "Increase your tier to unlock content\n" +
            "and further boost your coins.\n" +
            "<color=red>This also doubles your upgrade cost!</color>\n" +
            "<color=lime>Next: Unlock {0}.</color>\n" +
            "<color=yellow>Cost: {1} coins.</color>";
        objects.Add(namesOfItemsToUnlock[tier]);
        objects.Add(formatter.FormatNumber(cost));
    }

    public void AttemptPurchase() {
        if (purchaseHandler.IsAffordable(cost)) BuyTier();
    }

    public void BuyTier() {
        gameObjectsInTiers[tier].SetGameObjectsAsVisible();
        tier++;
        TierDisplay.text = "Tier " + tier;
        upgradeHandler.UnlockUpgrades(tier);
    }
}
