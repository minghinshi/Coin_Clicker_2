using UnityEngine;
using System;
using System.Collections;

public class NumberFormatter : MonoBehaviour {

    public static NumberFormatter instance;
    public Options options;

    public string[] suffixes;

    private void Awake()
    {
        instance = this;
    }

    public string FormatNumber(double number) {
        if (number < 1000000)
            return number.ToString("N0");
        else if (number < 1e66 && !options.formatSmallNumbers)
        {
            int magnitude = Convert.ToInt32(Math.Floor(Math.Log10(number)));
            string suffix = suffixes[Mathf.FloorToInt((magnitude - 6) / 3f)];

            int decimalPlacesAmount = 2 - (magnitude % 3);
            string decimalPlaces = "";
            for (int i = 0; i < decimalPlacesAmount; i++)
            {
                decimalPlaces += "#";
            }

            return (number / Math.Pow(10, magnitude - magnitude % 3)).ToString("0." + decimalPlaces) + " " + suffix;
        }
        else if (options.useLogarithm)
            return Logarithmic(number);
        else
            return Scientific(number);
    }

    public string Scientific(double number) {
        return number.ToString("0.##e0");
    }

    public string Logarithmic(double number) {
        double log = Math.Log10(number);
        return "e" + log.ToString("0.##");
    }
}