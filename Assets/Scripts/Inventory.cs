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

    int numSlots = 10;
    float slotHeight = 50;
    float distanceBetweenSlots = 60; // Hard coded ༼ つ ◕_◕ ༽つ
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
        initialSlot.transform.position = new Vector3(400 - distanceBetweenSlots * numSlots / 2, slotHeight, 0);

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

    public void AddItem(InventoryItem newItem)
    {
        icons[selectedSlot].gameObject.SetActive(true);
        icons[selectedSlot].texture = newItem.invenctoryIcon;
        names[selectedSlot].gameObject.SetActive(true);
        names[selectedSlot].text = newItem.nameOnHover;

        items[selectedSlot] = newItem;
        items[selectedSlot].gameObject.SetActive(false);
    }

    public void HandleInventory()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            selectedSlot++;
            if (selectedSlot >= numSlots) selectedSlot = 0;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            selectedSlot--;
            if (selectedSlot < 0) selectedSlot = numSlots - 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlot = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) selectedSlot = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) selectedSlot = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) selectedSlot = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5)) selectedSlot = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6)) selectedSlot = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha7)) selectedSlot = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha8)) selectedSlot = 7;
        else if (Input.GetKeyDown(KeyCode.Alpha9)) selectedSlot = 8;
        else if (Input.GetKeyDown(KeyCode.Alpha0)) selectedSlot = 9;

        for (int i = 0; i < numSlots; i++)
        {
            if (i == selectedSlot && slotRiseValues[i] < slotRise.keys[slotRise.length - 1].time)
            {
                slotRiseValues[i] += Time.deltaTime;
                names[i].gameObject.SetActive(true);
            }
            else if (i != selectedSlot && slotRiseValues[i] > 0)
            {
                slotRiseValues[i] -= Time.deltaTime;
                names[i].gameObject.SetActive(false);
            }

            slots[i].transform.position = new Vector3(slots[i].transform.position.x, slotHeight + slotRise.Evaluate(slotRiseValues[i]), 0);
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
}
