using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSlotController : MonoBehaviour, IDropHandler
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
        if (MenuBlockController.blockBeingDragged == null)
        {
            return;
        }

        MenuBlockController dragBlockController = MenuBlockController.blockBeingDragged.GetComponent<MenuBlockController>();
        
        if (block != null)
        {
            GameObject exBlock = block;
            exBlock.transform.SetParent(dragBlockController.previousParent);
            exBlock.transform.localScale = Vector3.one;

            if (exBlock.transform.parent == MainMenuController.singleton.inventoryContent)
            {
                MenuBlockController blockController = exBlock.GetComponent<MenuBlockController>();

                blockController.currSprite--;
                exBlock.GetComponent<Image>().sprite = blockController.sprites[blockController.currSprite];
            }
        }

        MenuBlockController.blockBeingDragged.transform.SetParent(transform);
        MenuBlockController.blockBeingDragged.transform.localScale = Vector3.one;

        if (dragBlockController.currSprite % 2 == 0)
        {
            dragBlockController.currSprite++;
        }
        MenuBlockController.blockBeingDragged.GetComponent<Image>().sprite = dragBlockController.sprites[dragBlockController.currSprite];
    }
}
