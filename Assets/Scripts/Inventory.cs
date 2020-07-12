using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject initialSlot;

    GameObject[] slots;
    RawImage[] icons;
    Text[] names;
    InventoryItem[] items;

    int numSlots = 4;
    float slotHeight = 60;
    float distanceBetweenSlots = 70; // Hard coded ༼ つ ◕_◕ ༽つ
    public AnimationCurve slotRise;
    float[] slotRiseValues;

    int selectedSlot = 0;

    void Start()
    {
        slots = new GameObject[numSlots];
        icons = new RawImage[numSlots];
        names = new Text[numSlots];
        items = new InventoryItem[numSlots];

        slots[0] = initialSlot;
        // Reference resolution is 800x600
        initialSlot.transform.position = new Vector3(initialSlot.transform.position.x - distanceBetweenSlots * (numSlots - 1) / 2, slotHeight, 0);

        slotRiseValues = new float[numSlots];

        for (int i = 0; i < numSlots; i++)
        {
            if (i > 0)
            {
                slots[i] = Instantiate(initialSlot, initialSlot.transform.parent);
                slots[i].transform.position = initialSlot.transform.position + new Vector3(distanceBetweenSlots * i, 0, 0);
            }
            foreach(Transform child in slots[i].transform)
            {
                if (child.name == "Item") icons[i] = child.GetComponent<RawImage>();
                else if (child.name == "Text") names[i] = child.GetComponent<Text>();
                child.gameObject.SetActive(false);
            }
        }
    }

    public void HandleInventory()
    {
        int startSlot = selectedSlot;
        if (Input.mouseScrollDelta.y > 0)
        {
            do
            {
                selectedSlot++;
                if (selectedSlot >= numSlots) selectedSlot = 0;
            } while (items[selectedSlot] == null && selectedSlot != startSlot);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            do
            {
                selectedSlot--;
                if (selectedSlot < 0) selectedSlot = numSlots - 1;
            } while (items[selectedSlot] == null && selectedSlot != startSlot);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) selectedSlot = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) selectedSlot = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) selectedSlot = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) selectedSlot = 3;
        //else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) selectedSlot = 4;
        //else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) selectedSlot = 5;
        //else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) selectedSlot = 6;
        //else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) selectedSlot = 7;
        //else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) selectedSlot = 8;
        //else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) selectedSlot = 9;

        for (int i = 0; i < numSlots; i++)
        {
            if (i == selectedSlot && items[i] != null && slotRiseValues[i] < slotRise.keys[slotRise.length - 1].time)
            {
                slotRiseValues[i] += Time.deltaTime;
                names[i].gameObject.SetActive(true);
            }
            else if ((i != selectedSlot || items[i] == null) && slotRiseValues[i] > 0)
            {
                slotRiseValues[i] -= Time.deltaTime;
                names[i].gameObject.SetActive(false);
            }

            slots[i].transform.position = new Vector3(slots[i].transform.position.x, slotHeight + slotRise.Evaluate(slotRiseValues[i]) * Screen.height / 14f, 0);
        }
    }

    public InventoryItem GetHeldItem()
    {
        return items[selectedSlot];
    }

    public Texture GetHeldIcon()
    {
        return icons[selectedSlot].texture;
    }

    public void AddItem(InventoryItem newItem)
    {
        for(selectedSlot = 0; selectedSlot < numSlots; selectedSlot++)
            if (items[selectedSlot] == null) break;

        icons[selectedSlot].gameObject.SetActive(true);
        icons[selectedSlot].texture = newItem.inventoryIcon;
        names[selectedSlot].gameObject.SetActive(true);
        names[selectedSlot].text = newItem.nameOnHover;

        items[selectedSlot] = newItem;
        items[selectedSlot].gameObject.SetActive(false);
    }

    public void TryDeleteHeldItem()
    {
        if (items[selectedSlot].disappearAfterUse)
        {
            items[selectedSlot] = null;
            icons[selectedSlot].gameObject.SetActive(false);
            names[selectedSlot].text = "";
        }
    }
}
