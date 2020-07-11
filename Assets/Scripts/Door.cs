using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    Vector3 startPos;
    float openAmount = 0;
    public AnimationCurve opening;
    float distanceToPlayerStartClosing = 10;

    Transform player;

    public InventoryItem itemToUnlock;

    void Start()
    {
        startPos = transform.position;
        if(itemToUnlock != null)
        {
            nameOnHover = "Requires \"" + itemToUnlock.nameOnHover + "\"";
        }
    }

    override
    public void Interact(PlayerController interactor)
    {
        if (itemToUnlock != null && interactor._inventory.GetHeldItem() != itemToUnlock)
            return;
        player = interactor.transform;
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
        canInteract = true;
        yield break;
    }
}
