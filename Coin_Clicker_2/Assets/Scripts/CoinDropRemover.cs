using UnityEngine;

public class CoinDropRemover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -62.5f)
        {
            Destroy(gameObject);
        }
    }
}
