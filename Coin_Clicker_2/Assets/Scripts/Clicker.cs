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
    private UpgradeHandler upgradeHandler;
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
        upgradeHandler = UpgradeHandler.instance;
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

    void SpawnCoinParticle(double overflowBonus) {
        GameObject coin = Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f)), Quaternion.identity, player.ParticleHolder);
        Rigidbody2D rigidbody = coin.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(Random.Range(-500f, 500f), Random.Range(850f, 1000f));
        coin.GetComponent<Coin>().coinValue = (player.CoinsPerClick + player.BonusCoinsPerClick) * overflowBonus;
    }

    void PlayClickSoundEffect() {
        coinSource.pitch = Random.Range(0.9f, 1.1f);
        coinSource.Play();
    }

    public void Click(double overflowBonus) {
        if (options.clickParticles) SpawnCoinParticle(overflowBonus);
        if (options.sfx) PlayClickSoundEffect();

        player.Coins += (player.CoinsPerClick + player.BonusCoinsPerClick) * overflowBonus;
        if (tierHandler.tier >= 1)
            player.Clickpoints += player.ClickpointsPerClick * overflowBonus;
        if (tierHandler.tier >= 2)
            player.Experience += player.ExperiencePerClick * overflowBonus;

        player.UpdateDisplays();
        multi.UpdateDisplays();
        autoclicker.UpdateDisplays();
    }
}
