using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    PlayerController player;

    public InventoryItem itemToUnlock;
    public GameObject containedItem;

    string startHoverText;

    public AudioSource unlockSound;

    void Start()
    {
        startHoverText = nameOnHover;
        player = FindObjectOfType<PlayerController>();
    }

    override
    public void LookingAt()
    {
        if (itemToUnlock != null)
        {
            if (player._inventory.GetHeldItem() == itemToUnlock)
            {
                nameOnHover = startHoverText;
                player.openHandImage.texture = player._inventory.GetHeldIcon();
            }
            else
                nameOnHover = "Requires \"" + itemToUnlock.nameOnHover + "\"";
        }
        else
        {
            player.openHandImage.texture = player.openHand;
        }
    }

    override
    public void Interact()
    {
        if (itemToUnlock != null && player._inventory.GetHeldItem() != itemToUnlock)
            return;
        player._inventory.TryDeleteHeldItem();
        Instantiate(containedItem, transform.position, containedItem.transform.rotation);
        unlockSound.Play();
        Destroy(gameObject);
    }
}
