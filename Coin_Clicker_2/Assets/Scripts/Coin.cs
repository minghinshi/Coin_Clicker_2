using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour {

    public double coinValue;

    public GameObject DisplayPrefab;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y <= -25f) {
            GameObject Display = Instantiate(DisplayPrefab, transform.position, Quaternion.identity, transform.parent);
            Display.GetComponent<CoinDisplay>().display.text = NumberFormatter.instance.FormatNumber(coinValue);

            Destroy(gameObject);
        }
	}
}
