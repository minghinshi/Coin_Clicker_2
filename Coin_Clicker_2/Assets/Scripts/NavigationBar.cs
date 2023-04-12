using UnityEngine;

public class NavigationBar : MonoBehaviour
{
    public static NavigationBar instance;
    public CanvasGroup[] panelsToNavigate;
    public GameObject mainScreen;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        panelsToNavigate = mainScreen.GetComponentsInChildren<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
