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

    public AudioSource coinSource;

    public GameObject coinPrefab;

    public bool coinIsHeld;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        player = Player.instance;
        multi = Multiplier.instance;
        autoclicker = Autoclicker.instance;
        options = Options.instance;
    }
	
	// Update is called once per frame
	void Update () {

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

    public void Click(double overflowBonus) {

        if (!player.devMode)
        {
            if (options.clickParticles)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f)), Quaternion.identity, player.ParticleHolder);
                Rigidbody2D rigidbody = coin.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(Random.Range(-500f, 500f), Random.Range(1000f, 1250f));
                coin.GetComponent<Coin>().coinValue = (player.coinsPerClick + player.bonusCoinsPerClick) * overflowBonus;
            }

            if (options.sfx)
            {
                coinSource.pitch = Random.Range(0.9f, 1.1f);
                coinSource.Play();
            }
        }

        player.coins += (player.coinsPerClick + player.bonusCoinsPerClick) * overflowBonus;
        if(player.purchasedUpgrade[0])
            player.clickpoints += player.clickpointsPerClick * overflowBonus;
        if (player.purchasedUpgrade[3])
            player.Experience += player.experiencePerClick * overflowBonus;

        player.UpdateDisplays();
        multi.UpdateDisplays();
        autoclicker.UpdateDisplays();
    }
}
