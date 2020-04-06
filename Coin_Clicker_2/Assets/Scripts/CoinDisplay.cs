using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour {

    public Text display;
    public float fadeTime;
     
	// Use this for initialization
	void Start () {
        Destroy(gameObject, fadeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
