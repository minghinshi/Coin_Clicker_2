using UnityEngine;

public class Coin : MonoBehaviour
{

    public double coinValue;

    public GameObject DisplayPrefab;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 200f)
        {
            GameObject Display = Instantiate(DisplayPrefab, transform.position, Quaternion.identity, transform.parent);
            Display.GetComponent<CoinDisplay>().display.text = NumberFormatter.FormatNumber(coinValue);

            Destroy(gameObject);
        }
    }
}
