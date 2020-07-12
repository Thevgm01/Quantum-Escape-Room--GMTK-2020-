using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable
{
    public Texture inventoryIcon;
    public bool disappearAfterUse;
    public AudioClip sound;

    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    override
    public void LookingAt()
    {
        player.openHandImage.texture = player.openHand;
    }

    override
    public void Interact()
    {
        FindObjectOfType<Inventory>().AddItem(this);
    }
}
