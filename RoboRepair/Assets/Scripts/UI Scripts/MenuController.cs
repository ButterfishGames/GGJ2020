using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public RectTransform codingPanel;

    public RectTransform inventoryContent;

    public RectTransform instructionContent;

    public TextMeshProUGUI toggleText; // Delete when sprites for toggle are created

    private bool showing;

    // Start is called before the first frame update
    void Start()
    {
        showing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

    public void ToggleMenu()
    {
        if (showing)
        {
            codingPanel.anchoredPosition = new Vector2(384, 0);
            toggleText.text = "<"; // Delete when sprites for toggle are created
            showing = false;
        }
        else
        {
            codingPanel.anchoredPosition = new Vector2(-384, 0);
            toggleText.text = ">"; // Delete when sprites for toggle are created
            showing = true;
        }
    }
}
