using System;
using UnityEngine;

public class NumberFormatter : MonoBehaviour
{
    static readonly string[] suffixes = { "k", "M", "B", "T", "q", "Q", "s", "S", "O", "N", "D", "UD", "DD", "TD", "qD", "QD", "sD", "SD", "OD", "ND" };

    public static string FormatNumber(double number, int decimalPlaces = 0)
    {
        if (number < 1000000)
            return Unformatted(number, decimalPlaces);
        else if (number < 1e66 && !Options.instance.formatSmallNumbers)
            return Standard(number);
        else if (Options.instance.useLogarithm)
            return Logarithmic(number);
        else
            return Scientific(number);
    }

    private static string Unformatted(double number, int decimalPlaces)
    {
        return number.ToString("N" + decimalPlaces.ToString());
    }

    private static string Standard(double number)
    {
        int magnitude = Convert.ToInt32(Math.Floor(Math.Log10(number)));
        string suffix = suffixes[Mathf.FloorToInt((magnitude - 3) / 3f)];
        return (number / Math.Pow(10, magnitude - magnitude % 3)).ToString("N3") + suffix;
    }

    static string Scientific(double number)
    {
        return number.ToString("0.##e0");
    }

    static string Logarithmic(double number)
    {
        double log = Math.Log10(number);
        return "e" + log.ToString("0.##");
    }
}