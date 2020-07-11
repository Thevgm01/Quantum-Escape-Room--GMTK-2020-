using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable
{
    public Texture invenctoryIcon;

    override
    public void Interact(PlayerController interactor)
    {
        interactor.GetComponent<Inventory>().AddItem(this);
    }
}
