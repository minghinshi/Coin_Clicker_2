using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationBar : MonoBehaviour
{
    public static NavigationBar instance;
    public CanvasGroup[] panelsToNavigate;
    GameObject mainScreen;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainScreen = transform.parent.Find("MainScreen").gameObject;
        panelsToNavigate = mainScreen.GetComponentsInChildren<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
