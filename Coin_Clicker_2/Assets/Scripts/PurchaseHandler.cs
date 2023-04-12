using UnityEngine;

public class PurchaseHandler : MonoBehaviour
{
    public static PurchaseHandler instance;
    private Player player;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
    }

    public bool IsAffordable(double cost)
    {
        if (player.Coins >= cost)
        {
            player.Coins -= cost;
            return true;
        }
        return false;
    }
}
