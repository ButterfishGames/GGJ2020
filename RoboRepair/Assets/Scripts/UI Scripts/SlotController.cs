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
        if (block == null && BlockController.blockBeingDragged != null)
        {
            BlockController dragBlockController = BlockController.blockBeingDragged.GetComponent<BlockController>();
            BlockController.blockBeingDragged.transform.SetParent(transform);
            if (BlockController.blockBeingDragged.GetComponent<Button>() == null)
            {
                Button remove = BlockController.blockBeingDragged.AddComponent<Button>();
                remove.onClick.AddListener(() => dragBlockController.RemoveBlock());
            }
        }
    }
}
