using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar instance;

    private Player player;
    private CoinDrop coinDrop;
    private Clicker clicker;

    public float[] timeLeft = new float[6];
    public float[] timeNeeded = new float[6];
    public Slider[] progressBars;
    public Text[] timeDisplay;
    public Text[] multiDisplay;
    public double[] barMulti;

    public int speedMultiLevel;
    public int speedMultiLevel2;
    public float SpeedMulti() {
        float f = speedMultiLevel * speedMultiLevel2;
        if (clicker.coinIsHeld && player.purchasedUpgrade[52])
            f *= 2;
        return f;
    }

    public Text TotalMultiDisplay;
    public Text SpeedDisplay;

    public double TotalMulti() {
        return barMulti[0] * barMulti[1] * barMulti[2] * barMulti[3] * barMulti[4] * barMulti[5];
    }

    string FloatToTime(float f) {
        if (f >= 31557600f)
            return Mathf.Floor(f / 31557600f).ToString("N0") + " yr";
        else if (f >= 604800f)
            return Mathf.Floor(f / 604800).ToString("N0") + " wk";
        else if (f >= 86400f)
            return Mathf.Floor(f / 86400f).ToString("N0") + " d";
        else if (f >= 3600f)
            return Mathf.Floor(f / 3600f).ToString("N0") + " hr";
        else if (f >= 60f)
            return Mathf.Floor(f / 60f).ToString("N0") + " min";
        else if (f >= 1f)
            return Mathf.Floor(f).ToString("N0") + " s";
        else
            return Mathf.Floor(f * 1000f).ToString("N0") + " ms";
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        coinDrop = CoinDrop.instance;
        clicker = Clicker.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.purchasedUpgrade[48])
        {
            for (int i = 0; i < timeLeft.Length; i++)
            {
                timeLeft[i] -= Time.deltaTime * SpeedMulti();
                if (timeLeft[i] <= 0)
                {
                    float reps = Mathf.Ceil(-timeLeft[i] / timeNeeded[i]);
                    timeLeft[i] += reps * timeNeeded[i];
                    barMulti[i] += 0.01 * reps;
                    if (i == 1 && player.purchasedUpgrade[50]) {
                        GameObject Drop = GameObject.FindWithTag("CoinDrop");
                        if (Drop != null)
                            coinDrop.OnCoinClick(Drop);
                    }
                }
                progressBars[i].value = (timeNeeded[i] - timeLeft[i]) / timeNeeded[i];
                timeDisplay[i].text = FloatToTime(timeLeft[i]/SpeedMulti());
                multiDisplay[i].text = barMulti[i].ToString("N2") + "x";
            }
        }

        TotalMultiDisplay.text = NumberFormatter.instance.FormatNumber(TotalMulti()) + "x coins";
        SpeedDisplay.text = NumberFormatter.instance.FormatNumber(SpeedMulti()) + "x speed";
    }

    public void BuySpeedMulti() {
        if (player.diamondCoins >= 50000) {
            player.diamondCoins -= 50000;
            speedMultiLevel++;
        }
    }

    public void BuySpeedMulti2()
    {
        if (player.level >= 1100)
        {
            player.level -= 100;
            speedMultiLevel2++;
        }
    }
}
