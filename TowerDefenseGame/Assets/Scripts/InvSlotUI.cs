using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {

    public int slot; //inventory slot number
	public Image slotSprite;
    public Sprite[] slotSprites;
    public Image itemSprite;
    public Text stackSizeText;
    public int type;
    public int index;
    public float stackSize;

    private void Update() {
        if (stackSize > 0) {
            var rot = itemSprite.transform.eulerAngles;
            rot.z = C.ben.Osc(1) * 10;
            itemSprite.transform.eulerAngles = rot;
        }
    }

    public void CheckIfEmpty() {
        if (stackSize == 0) {
            itemSprite.enabled = false;
            stackSizeText.enabled = false;
            type = 0;
            index = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {

        if (C.c.playerScript[0].invSelection == slot) { //double click slot
            if (C.c.playerScript[0].nearbyTable != null && type > 0) { //place on table to sell
                C.c.playerScript[0].nearbyTable.PickupItem();
                C.c.playerScript[0].inventory[slot].stack--;
                stackSize--;
                stackSizeText.text = stackSize.ToString();
                C.c.playerScript[0].nearbyTable.SetItem(type,index);
                CheckIfEmpty();
            }
            return;
        }

        C.c.playerScript[0].inventoryUI[C.c.playerScript[0].invSelection].slotSprite.sprite = slotSprites[0];
        C.c.playerScript[0].invSelection = slot;
        slotSprite.sprite = slotSprites[1];
        //Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //Debug.Log("Mouse Enter" + eventData.pointerCurrentRaycast.gameObject.name);
    }

}
