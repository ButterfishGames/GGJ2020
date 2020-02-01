using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject blockBeingDragged;
    public Block block;

    public void OnBeginDrag (PointerEventData eventData)
    {
        blockBeingDragged = gameObject;
        transform.SetParent(MenuController.singleton.codingPanel);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag (PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        blockBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == MenuController.singleton.codingPanel)
        {
            transform.SetParent(MenuController.singleton.inventoryContent);
        }
    }

    public void RemoveBlock()
    {
        transform.SetParent(MenuController.singleton.inventoryContent);
        Destroy(GetComponent<Button>());
    }
}

[System.Serializable]
public struct Block
{
    public enum BlockType
    {
        move,
        turn,
        shoot,
        repair,
        wait
    }

    public BlockType blockType;

    public int val;
}
