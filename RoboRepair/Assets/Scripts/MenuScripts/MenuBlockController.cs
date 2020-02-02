using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuBlockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject blockBeingDragged;

    public enum MenuBlockType
    {
        play,
        credits,
        quit
    }

    public MenuBlockType blockType;

    [Tooltip("0-1 is Play, 2-3 is Credits, 4-5 is Quit")]
    public Sprite[] sprites;

    public int currSprite;

    public Transform previousParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        blockBeingDragged = gameObject;
        transform.SetParent(MainMenuController.singleton.canvas);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (currSprite % 2 == 1)
        {
            currSprite--;
        }
        GetComponent<Image>().sprite = sprites[currSprite];
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        blockBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == MainMenuController.singleton.canvas)
        {
            transform.SetParent(MainMenuController.singleton.inventoryContent);

            if (currSprite % 2 == 1)
            {
                currSprite--;
            }
            GetComponent<Image>().sprite = sprites[currSprite];
        }
    }
}