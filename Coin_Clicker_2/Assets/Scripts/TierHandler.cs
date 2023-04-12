using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Feature
{
    [SerializeField] string nameOfFeatureToUnlock;
    [SerializeField] GameObject rowOfUpgrades;
    [SerializeField] GameObject[] elementsToShow;

    public Feature()
    {

    }

    public Transform GetTransformOfRowOfUpgrades()
    {
        return rowOfUpgrades.transform;
    }

    public void ActivateFeature()
    {
        foreach (GameObject gameObject in elementsToShow)
            gameObject.SetActive(true);
        rowOfUpgrades.SetActive(true);
    }

    public String GetTierName()
    {
        return nameOfFeatureToUnlock;
    }
}

public class TierHandler : MonoBehaviour
{
    public static TierHandler instance;
    PurchaseHandler purchaseHandler;
    TooltipDisplayer tooltipDisplayer;

    [SerializeField] Feature[] features;
    [SerializeField] Text TierDisplay;
    [SerializeField] RectTransform upgradesRectTransform;

    int tier = 0;
    double cost
    {
        get
        {
            return UpgradeHandler.GetCostOfNthUpgrade(tier * (tier + 2));
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        purchaseHandler = PurchaseHandler.instance;
        tooltipDisplayer = GetComponent<TooltipDisplayer>();

        tooltipDisplayer.SetStringToDisplay(delegate
        {
            return string.Format(
                "Increase your tier to unlock content\n" +
                "and further boost your coins.\n" +
                "<color=red>This also increases your upgrade cost!</color>\n" +
                "<color=lime>Next: Unlock {0}.</color>\n" +
                "<color=yellow>Cost: {1} coins.</color>", features[tier + 1].GetTierName(), NumberFormatter.FormatNumber(cost));
        });
    }

    public void AttemptPurchase()
    {
        if (purchaseHandler.IsAffordable(cost)) BuyTier();
    }

    public void BuyTier()
    {
        tier++;
        features[tier].ActivateFeature();
        TierDisplay.text = "Tier " + tier;
        UpgradeHandler.SetUpgradesAsVisible(tier);
        LayoutRebuilder.ForceRebuildLayoutImmediate(upgradesRectTransform);
    }

    public Feature GetFeature(int rowNumber)
    {
        return features[rowNumber];
    }

    public int GetNumberOfFeatures()
    {
        return features.Length;
    }

    public int GetTier()
    {
        return tier;
    }
}
