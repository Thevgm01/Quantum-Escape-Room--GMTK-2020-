using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable
{
    public Texture inventoryIcon;
    public bool disappearAfterUse;

    override
    public void LookingAt()
    {

    }

    override
    public void Interact()
    {
        FindObjectOfType<Inventory>().AddItem(this);
    }
}
