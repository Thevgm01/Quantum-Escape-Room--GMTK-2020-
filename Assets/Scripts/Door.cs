using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool locked;

    protected Vector3 startPos;
    float openAmount = 0;
    public AnimationCurve opening;
    float distanceToPlayerStartClosing = 10;

    protected PlayerController player;

    public InventoryItem itemToUnlock;
    public Interactable activationSwitch;

    protected string startHoverText;

    protected AudioSource source;
    public AudioClip openSound;
    public AudioClip lockedSound;

    void Start()
    {
        startHoverText = nameOnHover;
        startPos = transform.position;
        player = FindObjectOfType<PlayerController>();

        if(activationSwitch != null)
        {
            activationSwitch.activated += Interact;
        }

        source = GetComponent<AudioSource>();
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
        if (itemToUnlock != null)
        {
            if (player._inventory.GetHeldItem() != itemToUnlock)
            {
                source.clip = lockedSound;
                source.Play();
                return;
            }
            else player._inventory.TryDeleteHeldItem();
        }
        source.clip = openSound;
        source.Play();
        StartCoroutine("Open");
    }

    protected IEnumerator Open()
    {
        canInteract = false;
        while (openAmount < opening.keys[opening.length - 1].time)
        {
            openAmount += Time.deltaTime;
            transform.position = startPos + new Vector3(0, opening.Evaluate(openAmount), 0);
            yield return null;
        }
        if (activationSwitch != null)
        {
            activationSwitch.activated -= Interact;
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
