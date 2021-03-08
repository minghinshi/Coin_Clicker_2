using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UpgradeHandler : MonoBehaviour
{
    public static UpgradeHandler instance;
    [SerializeField]
    private TierHandler tierHandler;
    private NumberFormatter formatter;
    public Dictionary<int, Upgrade> upgrades = new Dictionary<int, Upgrade>();
    public List<Upgrade> purchasedUpgrades = new List<Upgrade>();
    public GameObject upgradePrefab;
    public GameObject[] upgradeRows;

    public Text upgradeCostDisplay;

    public double GetCost() {
        return Math.Pow(2, purchasedUpgrades.Count + tierHandler.tier + 1);
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
        CreateUpgrades();
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

    void CreateUpgrades() {
        int gridSize = upgradeRows.Length;
        for (int row = 0; row < gridSize; row++)
            for (int col = 0; col < gridSize; col++)
                CreateUpgrade(row, col);
    }

    void CreateUpgrade(int row, int col) {
        GameObject upgradeObject = Instantiate(upgradePrefab, upgradeRows[row].transform);
        Upgrade upgrade = upgradeObject.GetComponent<Upgrade>();
        if (row == col)
            DisableUpgrade(upgrade);
        else
            InitializeUpgrade(upgrade, row, col);
    }

    void InitializeUpgrade(Upgrade upgrade, int row, int col) {
        upgrade.id = (row + 1) * 100 + (col + 1);
        upgrade.tierRequired = Math.Max(row, col) + 1;
        upgrade.SetReferences();
        upgrades.Add(upgrade.id, upgrade);
        upgrade.gameObject.SetActive(false);
    }

    void DisableUpgrade(Upgrade upgrade) {
        Image image = upgrade.GetComponent<Image>();
        image.color = new Color(.2f, .2f, .2f);
        upgrade.enabled = false;
    }

    public List<Upgrade> GetListOfUpgradesFromID(params int[] ids)
    {
        List<Upgrade> selectedUpgrades = new List<Upgrade>();
        foreach (int id in ids)
        {
            try { selectedUpgrades.Add(upgrades[id]); }
            catch (KeyNotFoundException) { }
        }
        return selectedUpgrades;
    }

    public double GetEffect(int id) {
        try {
            if (!upgrades[id].isPurchased)
            {
                if (upgrades[id].isAdditive)
                    return 0;
                return 1;
            }
            return upgrades[id].GetEffect();
        }
        catch (KeyNotFoundException) { return 1; }
    }

    public double GetTotalEffect(bool isAdditive, List<Upgrade> upgrades)
    {
        double totalEffect = (isAdditive ? 0 : 1);
        foreach (Upgrade upgrade in upgrades)
        {
            if (isAdditive)
                totalEffect += GetEffect(upgrade.id);
            else
                totalEffect *= GetEffect(upgrade.id);
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
        try {return upgrades[id].isPurchased; }
        catch (KeyNotFoundException) { return false; }
    }

    public void UnlockUpgrades(int tier) {
        foreach (KeyValuePair<int, Upgrade> upgradePair in upgrades) {
            Upgrade upgrade = upgradePair.Value;
            if (upgrade.tierRequired == tier + 1)
                upgrade.gameObject.SetActive(true);
        }
    }
}
