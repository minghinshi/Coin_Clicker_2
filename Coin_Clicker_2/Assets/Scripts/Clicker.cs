using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public static Clicker instance;

    private Player player;
    private Multiplier multi;
    private Autoclicker autoclicker;
    private Options options;
    private TierHandler tierHandler;

    public AudioSource coinSource;

    public GameObject coinPrefab;

    public bool coinIsHeld;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        player = Player.instance;
        multi = Multiplier.instance;
        autoclicker = Autoclicker.instance;
        options = Options.instance;
        tierHandler = TierHandler.instance;
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        coinIsHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        coinIsHeld = false;
        Click(1);
    }

    void SpawnCoinParticle(double clicksPerTick) {
        GameObject coin = Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f)), Quaternion.identity, player.ParticleHolder);
        Rigidbody2D rigidbody = coin.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(Random.Range(-500f, 500f), Random.Range(850f, 1000f));
        coin.GetComponent<Coin>().coinValue = (player.CoinsPerClick + player.BonusCoinsPerClick) * clicksPerTick;
    }

    void PlayClickSoundEffect() {
        coinSource.pitch = Random.Range(0.9f, 1.1f);
        coinSource.Play();
    }

    public void Click(double clicksPerTick) {
        if (options.clickParticles) SpawnCoinParticle(clicksPerTick);
        if (options.sfx) PlayClickSoundEffect();

        player.Coins += (player.CoinsPerClick + player.BonusCoinsPerClick) * clicksPerTick;
        if (tierHandler.GetTier() >= 1)
            player.Clickpoints += player.ClickpointsPerClick * clicksPerTick;
        if (tierHandler.GetTier() >= 2)
            player.Experience += player.ExperiencePerClick * clicksPerTick;

        player.UpdateDisplays();
        multi.UpdateDisplays();
        autoclicker.UpdateDisplays();
    }
}
