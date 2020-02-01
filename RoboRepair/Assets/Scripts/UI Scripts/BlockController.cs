using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BlockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject blockBeingDragged;
    public bool broken;
    public Block block;

    [Tooltip("0-1 is Move, 2-3 is Turn, 4-5 is Shoot, 6-7 is Repair, 8-9 is Wait, 10 is Broken")]
    public Sprite[] sprites;

    public int currSprite;

    public RuntimeAnimatorController brokenController;

    public Transform previousParent;

    public void OnBeginDrag (PointerEventData eventData)
    {
        if (broken)
        {
            return;
        }

        previousParent = transform.parent;

        blockBeingDragged = gameObject;
        transform.SetParent(MenuController.singleton.codingPanel);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (currSprite % 2 == 1)
        {
            currSprite--;
        }
        GetComponent<Image>().sprite = sprites[currSprite];
    }

    public void OnDrag (PointerEventData eventData)
    {
        if (broken)
        {
            return;
        }

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        if (broken)
        {
            return;
        }

        blockBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == MenuController.singleton.codingPanel)
        {
            transform.SetParent(MenuController.singleton.inventoryContent);
            if (GetComponent<Button>() != null)
            {
                Destroy(GetComponent<Button>());
            }

            if (currSprite % 2 == 1)
            {
                currSprite--;
            }
            GetComponent<Image>().sprite = sprites[currSprite];

            transform.localScale = Vector3.one;
        }
    }

    public void RemoveBlock()
    {
        transform.SetParent(MenuController.singleton.inventoryContent);

        if (currSprite % 2 == 1)
        {
            currSprite--;
        }
        GetComponent<Image>().sprite = sprites[currSprite];

        transform.localScale = Vector3.one;

        Destroy(GetComponent<Button>());
    }

    public void InitUI()
    {
        string text = "";
        switch (block.blockType)
        {
            case Block.BlockType.move:
                text = "Move " + block.val;
                currSprite = 0;
                break;

            case Block.BlockType.turn:
                text = "Turn ";
                if (block.val > 0)
                {
                    text += "R ";
                }
                else
                {
                    text += "L ";
                }
                text += Mathf.Abs(block.val) + "°";
                currSprite = 2;
                break;

            case Block.BlockType.shoot:
                text = "Shoot";
                currSprite = 4;
                break;

            case Block.BlockType.repair:
                text = "Repair";
                currSprite = 6;
                break;

            case Block.BlockType.wait:
                text = "Wait " + block.val + "s";
                currSprite = 8;
                break;
        }

        GetComponentInChildren<TextMeshProUGUI>().text = text;
        if (broken)
        {
            currSprite = 10;
            Animator animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = brokenController;
        }

        GetComponent<Image>().sprite = sprites[currSprite];
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
