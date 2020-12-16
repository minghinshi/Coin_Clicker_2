using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    RectTransform rectTransform;
    Text tooltipText;
    public static Tooltip instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        tooltipText = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Input.mousePosition + new Vector3(10f,10f);
    }

    void UpdateTooltipSize() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void SetText(string Text) {
        tooltipText.text = Text;
        transform.localScale = new Vector3(1f, 1f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void ClearText() {
        tooltipText.text = "";
        transform.localScale = new Vector3(0f, 0f);
    }
}
