using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotController : MonoBehaviour, IDropHandler
{
    public GameObject block
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (BlockController.blockBeingDragged == null)
        {
            return;
        }

        BlockController dragBlockController = BlockController.blockBeingDragged.GetComponent<BlockController>();

        if (block != null)
        {
            GameObject exBlock = block;
            exBlock.transform.SetParent(dragBlockController.previousParent);
            exBlock.transform.localScale = Vector3.one;

            if (exBlock.transform.parent == MenuController.singleton.inventoryContent)
            {
                Destroy(exBlock.GetComponent<Button>());

                BlockController blockController = exBlock.GetComponent<BlockController>();

                blockController.currSprite--;
                exBlock.GetComponent<Image>().sprite = blockController.sprites[blockController.currSprite];
            }
        }

        BlockController.blockBeingDragged.transform.SetParent(transform);
        BlockController.blockBeingDragged.transform.localScale = Vector3.one;
        if (BlockController.blockBeingDragged.GetComponent<Button>() == null)
        {
            Button remove = BlockController.blockBeingDragged.AddComponent<Button>();
            remove.onClick.AddListener(() => dragBlockController.RemoveBlock());
        }

        if (dragBlockController.currSprite % 2 == 0)
        {
            dragBlockController.currSprite++;
        }
        BlockController.blockBeingDragged.GetComponent<Image>().sprite = dragBlockController.sprites[dragBlockController.currSprite];
    }
}
