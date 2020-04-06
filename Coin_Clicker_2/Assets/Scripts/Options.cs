using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static Options instance;

    public bool music;
    public bool sfx;
    public bool clickParticles;
    public bool bell;
    public bool collectionSound;
    public bool collectionParticles;
    public bool useLogarithm;
    public bool formatSmallNumbers;

    public Text musicText;
    public Text sfxText;
    public Text clickParticleText;
    public Text bellText;
    public Text collectionSoundText;
    public Text collectionParticlesText;
    public Text numberFormatText;
    public Text formatSmallNumbersText;

    public AudioSource musicSource;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMusic() {
        music = (!music);
        UpdateOptions();
    }

    public void ToggleSFX()
    {
        sfx = (!sfx);
        UpdateOptions();
    }

    public void ToggleParticles()
    {
        clickParticles = (!clickParticles);
        UpdateOptions();
    }

    public void ToggleBell()
    {
        bell = (!bell);
        UpdateOptions();
    }

    public void ToggleCollectionSound()
    {
        collectionSound = (!collectionSound);
        UpdateOptions();
    }

    public void ToggleCollectionParticles()
    {
        collectionParticles = (!collectionParticles);
        UpdateOptions();
    }

    public void ToggleNumberFormat()
    {
        useLogarithm = (!useLogarithm);
        UpdateOptions();
    }

    public void ToggleFormatSmallNumbers()
    {
        formatSmallNumbers = (!formatSmallNumbers);
        UpdateOptions();
    }

    void UpdateUpgradeDisplays() {
        foreach (Upgrades upgrade in FindObjectsOfType<Upgrades>()) {
            upgrade.UpdateDisplay();
        }
    }

    public void UpdateOptions() {
        if (music)
            musicText.text = "Music: On";
        else
            musicText.text = "Music: Off";
        musicSource.enabled = music;
        if (sfx)
            sfxText.text = "SFX: On";
        else
            sfxText.text = "SFX: Off";
        if (clickParticles)
            clickParticleText.text = "Click particles: On";
        else
            clickParticleText.text = "Click particles: Off";
        if (bell)
            bellText.text = "Bell sound: On";
        else
            bellText.text = "Bell sound: Off";
        if (collectionSound)
            collectionSoundText.text = "Coin collection SFX: On";
        else
            collectionSoundText.text = "Coin collection SFX: Off";
        if (collectionParticles)
            collectionParticlesText.text = "Coin collection particles: On";
        else
            collectionParticlesText.text = "Coin collection particles: Off";
        if (useLogarithm)
            numberFormatText.text = "Number format: Logarithmic";
        else
            numberFormatText.text = "Number format: Scientific";
        if (formatSmallNumbers)
            formatSmallNumbersText.text = "Format numbers when over 10^6";
        else
            formatSmallNumbersText.text = "Format numbers when over 10^66";
        UpdateUpgradeDisplays();
    }
}
