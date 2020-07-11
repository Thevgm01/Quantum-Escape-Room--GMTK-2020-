using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    GameObject[] slots;
    int numSlots = 10;
    float slotHeight = 50;
    float distanceBetweenSlots = 60; // Hard coded ༼ つ ◕_◕ ༽つ
    public GameObject initialSlot;

    int selectedSlot = 0;

    public AnimationCurve slotRise;
    float[] slotRiseValues;

    void Start()
    {
        // Reference resolution is 800x600
        initialSlot.transform.position = new Vector3(400 - distanceBetweenSlots * numSlots / 2, slotHeight, 0);

        slots = new GameObject[numSlots];
        slots[0] = initialSlot;

        slotRiseValues = new float[numSlots];

        for (int i = 1; i < numSlots; i++)
        {
            slots[i] = Instantiate(initialSlot, initialSlot.transform.parent);
            slots[i].transform.position = initialSlot.transform.position + new Vector3(distanceBetweenSlots * i, 0, 0);
        }
    }

    public void AddItem(GameObject newItem)
    {
        slots[selectedSlot].GetComponent<UnityEngine.UI.RawImage>().material = newItem.GetComponent<Material>();
        //var obj = Instantiate(new GameObject(), slots[selectedSlot].transform);
        //obj.AddComponent<Material>().SetTexture("tex", newItem.GetComponent<Material>().mainTexture);
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
                slotRiseValues[i] += Time.deltaTime;
            else if (i != selectedSlot && slotRiseValues[i] > 0)
                slotRiseValues[i] -= Time.deltaTime;

            slots[i].transform.position = new Vector3(slots[i].transform.position.x, slotHeight + slotRise.Evaluate(slotRiseValues[i]), 0);
        }
    }
}
