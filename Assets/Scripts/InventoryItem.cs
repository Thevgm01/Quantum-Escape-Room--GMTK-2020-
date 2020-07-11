using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable
{
    override
    public void Interact(GameObject interactor)
    {
        interactor.GetComponent<Inventory>().AddItem(gameObject);
    }
}
