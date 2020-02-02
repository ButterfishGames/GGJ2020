using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController singleton;

    public GameObject blockPrefab;

    public GameObject slotPrefab;

    public RectTransform codingPanel;

    public RectTransform inventoryContent;

    public RectTransform instructionContent;

    public Image toggleImg;

    public Sprite openMenuSprite, closeMenuSprite;

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

        for(int i = 0; i < LevelController.singleton.brokenBlocks.Length; i++)
        {
            SlotController slot = slots[LevelController.singleton.brokenBlocks[i].slot];
            if (slot.block == null)
            {
                GameObject codeBlock = Instantiate(blockPrefab, slot.transform);
                BlockController blockController = codeBlock.GetComponent<BlockController>();
                blockController.block = LevelController.singleton.brokenBlocks[i].block;
                blockController.broken = true;
                blockController.InitUI();
            }
            else
            {
                Debug.Log("ERROR: Repeated slot for broken blocks");
            }
        }

        foreach (Block block in LevelController.singleton.inventory)
        {
            GameObject codeBlock = Instantiate(blockPrefab, inventoryContent);
            BlockController blockController = codeBlock.GetComponent<BlockController>();
            blockController.block = block;
            blockController.InitUI();
        }
    }

    public void ToggleMenu()
    {
        if (showing)
        {
            codingPanel.anchoredPosition = new Vector2(384, 0);
            toggleImg.sprite = openMenuSprite;
            showing = false;
        }
        else
        {
            codingPanel.anchoredPosition = new Vector2(-384, 0);
            toggleImg.sprite = closeMenuSprite;
            showing = true;
        }
    }

    public void RunScript()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player.executing)
        {
            return;
        }

        List<int> actions = new List<int>();
        List<int> values = new List<int>();

        foreach (SlotController slot in slots)
        {
            if (slot.block != null)
            {
                Block block = slot.block.GetComponent<BlockController>().block;
                actions.Add((int)block.blockType);
                values.Add(block.val);
            }
        }

        player.SetCommands(actions.ToArray(), values.ToArray());

        ToggleMenu();
    }
}
