using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Interactable : MonoBehaviour
{
    public Action activated = delegate { };
    public abstract void LookingAt();
    public abstract void Interact();
    public bool canInteract;
    public string nameOnHover;
}
