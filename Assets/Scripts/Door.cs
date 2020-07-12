using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool locked;

    Vector3 startPos;
    float openAmount = 0;
    public AnimationCurve opening;
    float distanceToPlayerStartClosing = 10;

    PlayerController player;

    public InventoryItem itemToUnlock;
    public Interactable activationSwitch;

    string startHoverText;

    void Start()
    {
        startHoverText = nameOnHover;
        startPos = transform.position;
        player = FindObjectOfType<PlayerController>();
    }

    override
    public void LookingAt()
    {
        if (player._inventory.GetHeldItem() == itemToUnlock)
            nameOnHover = startHoverText;
        else
            nameOnHover = "Requires \"" + itemToUnlock.nameOnHover + "\"";
    }

    override
    public void Interact()
    {
        if (itemToUnlock != null && player._inventory.GetHeldItem() != itemToUnlock)
            return;
        player._inventory.TryDeleteHeldItem();
        StartCoroutine("Open");
    }

    IEnumerator Open()
    {
        canInteract = false;
        while (openAmount < opening.keys[opening.length - 1].time)
        {
            openAmount += Time.deltaTime;
            transform.position = startPos + new Vector3(0, opening.Evaluate(openAmount), 0);
            yield return null;
        }
        /*
        while (Vector3.Distance(transform.position, player.transform.position) <= distanceToPlayerStartClosing)
        {
            yield return new WaitForSeconds(0.3f);
        }
        while (openAmount > 0)
        {
            openAmount -= Time.deltaTime;
            transform.position = startPos + new Vector3(0, opening.Evaluate(openAmount), 0);
            yield return null;
        }
        openAmount = 0;
        transform.position = startPos;
        canInteract = !locked;
        yield break;
        */
    }
}
