using UnityEngine;
using System;
using System.Collections;

public class NumberFormatter : MonoBehaviour {

    static string[] suffixes = {"k", "M", "B", "T", "q", "Q", "s", "S", "O", "N", "D", "UD", "DD", "TD", "qD", "QD", "sD", "SD", "OD", "ND"};

    public static string FormatNumber(double number) {
        if (number < 1000)
            return number.ToString("N2");
        else if (number < 1e66 && !Options.instance.formatSmallNumbers)
        {
            int magnitude = Convert.ToInt32(Math.Floor(Math.Log10(number)));
            string suffix = suffixes[Mathf.FloorToInt((magnitude - 3) / 3f)];

            int decimalPlacesAmount = 3 - (magnitude % 3);
            string decimalPlaces = "";
            for (int i = 0; i < decimalPlacesAmount; i++)
            {
                decimalPlaces += "0";
            }

            return (number / Math.Pow(10, magnitude - magnitude % 3)).ToString("0." + decimalPlaces) + " " + suffix;
        }
        else if (Options.instance.useLogarithm)
            return Logarithmic(number);
        else
            return Scientific(number);
    }

    static string Scientific(double number) {
        return number.ToString("0.##e0");
    }

    static string Logarithmic(double number) {
        double log = Math.Log10(number);
        return "e" + log.ToString("0.##");
    }
}