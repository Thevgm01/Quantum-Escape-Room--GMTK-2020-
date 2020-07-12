using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    PlayerController player;

    public InventoryItem itemToUnlock;
    public GameObject containedItem;

    string startHoverText;

    void Start()
    {
        startHoverText = nameOnHover;
        player = FindObjectOfType<PlayerController>();
    }

    override
    public void LookingAt()
    {
        if(player._inventory.GetHeldItem() == itemToUnlock)
            nameOnHover = startHoverText;
        else
            nameOnHover = "Requires \"" + itemToUnlock.nameOnHover + "\"";
    }

    override
    public void Interact()
    {
        if (itemToUnlock != null && player._inventory.GetHeldItem() != itemToUnlock)
            return;
        Instantiate(containedItem, transform.position, containedItem.transform.rotation);
        Destroy(gameObject);
    }
}
