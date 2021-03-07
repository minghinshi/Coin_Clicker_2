using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UpgradeHandler : MonoBehaviour
{
    public static UpgradeHandler instance;
    private TierHandler tierHandler;
    private NumberFormatter formatter;
    public List<Upgrade> upgrades;
    public List<Upgrade> purchasedUpgrades;

    public Text upgradeCostDisplay;

    public double GetCost() {
        return Math.Pow(16, purchasedUpgrades.Count + tierHandler.tier);
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tierHandler = TierHandler.instance;
        formatter = NumberFormatter.instance;
        FillListOfUpgrades();
        SetAdditive(5, 12, 23, 24, 27, 28, 31, 34, 40, 41);
    }

    // Update is called once per frame
    void Update()
    {
        upgradeCostDisplay.text = formatter.FormatNumber(GetCost());
    }

    void SetAdditive(params int[] ids)
    {
        foreach (int id in ids)
            upgrades[id].isAdditive = true;
    }

    void FillListOfUpgrades()
    {
        Upgrade[] foundUpgrades = FindObjectsOfType<Upgrade>();
        upgrades = new List<Upgrade>(foundUpgrades);
        foreach (Upgrade foundUpgrade in foundUpgrades)
            upgrades[foundUpgrade.id] = foundUpgrade;
    }

    public List<Upgrade> GetListOfUpgradesFromID(params int[] ids)
    {
        List<Upgrade> selectedUpgrades = new List<Upgrade>();
        foreach (int id in ids)
            selectedUpgrades.Add(upgrades[id]);
        return selectedUpgrades;
    }

    public double GetEffect(int id) {
        return upgrades[id].GetEffect();
    }

    public double GetTotalEffect(bool isAdditive, List<Upgrade> upgrades)
    {
        double totalEffect = (isAdditive ? 0 : 1);
        foreach (Upgrade upgrade in upgrades)
        {
            if (isAdditive)
                totalEffect += upgrade.GetEffect();
            else
                totalEffect *= upgrade.GetEffect();
        }
        return totalEffect;
    }

    public double GetTotalEffect(List<Upgrade> upgrades) {
        return GetTotalEffect(false, upgrades);
    }

    public double GetTotalEffect(bool isAdditive, params int[] ids)
    {
        return GetTotalEffect(isAdditive, GetListOfUpgradesFromID(ids));
    }

    public double GetTotalEffect(params int[] ids)
    {
        return GetTotalEffect(GetListOfUpgradesFromID(ids));
    }

    public bool IsUpgradePurchased(int id)
    {
        return upgrades[id].isPurchased;
    }
}
