using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationTab : MonoBehaviour
{
    public CanvasGroup targetPanel;
    public CanvasGroup[] allPanels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnToggle() {
        foreach (CanvasGroup panel in allPanels) {
            panel.alpha = 0;
            panel.blocksRaycasts = false;
            panel.interactable = false;
        }
        targetPanel.alpha = 1;
        targetPanel.blocksRaycasts = true;
        targetPanel.interactable = true;
    }
}
