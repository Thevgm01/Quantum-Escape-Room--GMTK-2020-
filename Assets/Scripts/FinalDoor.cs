using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : Door
{
    public InventoryItem blueCard;
    public InventoryItem redCard;
    public InventoryItem yellowCard;

    float fadeTracker = 0;
    public float fadeTimeInSeconds;
    public UnityEngine.UI.RawImage fadeToBlack;

    public AudioClip partialUnlockSound;

    void Start()
    {
        startHoverText = nameOnHover;
        startPos = transform.position;
        player = FindObjectOfType<PlayerController>();

        if (activationSwitch != null)
        {
            activationSwitch.activated += Interact;
        }

        source = GetComponent<AudioSource>();
        source.clip = openSound;
    }

    override
    public void LookingAt()
    {
        /*
        if (player._inventory.GetHeldItem() == null)
        {
            player.openHandImage.texture = player.openHand;
            return;
        }
        */
        nameOnHover = "Requires ";
        if (blueCard) nameOnHover += "\"" + blueCard.name + "\" ";
        if (redCard) nameOnHover += "\"" + redCard.name + "\" ";
        if (yellowCard) nameOnHover += "\"" + yellowCard.name + "\" ";

        InventoryItem held = player._inventory.GetHeldItem();
        if (held == null)
        {
            player.openHandImage.texture = player.openHand;
            return;
        }

        if (blueCard != null && held.name.Contains(blueCard.name))
            player.openHandImage.texture = blueCard.inventoryIcon;
        else if (redCard != null && held.name.Contains(redCard.name))
            player.openHandImage.texture = redCard.inventoryIcon;
        else if (yellowCard != null && held.name.Contains(yellowCard.name))
            player.openHandImage.texture = yellowCard.inventoryIcon;
    }

    override
    public void Interact()
    {
        InventoryItem held = player._inventory.GetHeldItem();
        if (held == null)
        {
            source.clip = lockedSound;
            source.Play();
            return;
        }

        source.clip = partialUnlockSound;
        if(blueCard != null && held.name.Contains(blueCard.name))
        {
            blueCard = null;
            player._inventory.TryDeleteHeldItem();
            foreach (Transform child in transform) if (child.name == "Blue Light") Destroy(child.gameObject);
            source.Play();
        }
        else if(redCard != null && held.name.Contains(redCard.name))
        {
            redCard = null;
            player._inventory.TryDeleteHeldItem();
            foreach (Transform child in transform) if (child.name == "Red Light") Destroy(child.gameObject);
            source.Play();
        }
        else if(yellowCard != null && held.name.Contains(yellowCard.name))
        {
            yellowCard = null;
            player._inventory.TryDeleteHeldItem();
            foreach (Transform child in transform) if (child.name == "Yellow Light") Destroy(child.gameObject);
            source.Play();
        }

        if (blueCard == null && redCard == null && yellowCard == null)
        {
            source.clip = openSound;
            source.Play();
            StartCoroutine("Open");
            StartCoroutine("FadeToBlack");
            player.ToggleMovement();
            player.ToggleLook();
        }
    }

    IEnumerator FadeToBlack()
    {
        while(fadeTracker < fadeTimeInSeconds)
        {
            fadeTracker += Time.deltaTime;
            fadeToBlack.color = new Color(0, 0, 0, fadeTracker / fadeTimeInSeconds);
            yield return null;
        }
    }
}
