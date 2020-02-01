using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public static MenuController singleton;

    public GameObject blockPrefab;

    public GameObject slotPrefab;

    public RectTransform codingPanel;

    public RectTransform inventoryContent;

    public RectTransform instructionContent;

    public TextMeshProUGUI toggleText; // Delete when sprites for toggle are created

    private bool showing;

    private SlotController[] slots;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        showing = false;

        List<SlotController> slotList = new List<SlotController>();
        for (int i = 0; i < LevelController.singleton.numSlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, instructionContent);
            slotList.Add(slot.GetComponent<SlotController>());
        }
        slots = slotList.ToArray();

        foreach (Block block in LevelController.singleton.inventory)
        {
            GameObject codeBlock = Instantiate(blockPrefab, inventoryContent);
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
