using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable
{
    public string objectName = "Name";
    public List<string> keyWords;

    public virtual bool Examine(InteractionData interactionData) { return false; }
    public virtual bool Give(InteractionData interactionData) { return false; }
    public virtual bool Open(InteractionData interactionData) { return false; }
    public virtual bool Take(InteractionData interactionData) { return false; }
    public virtual bool Talk(InteractionData interactionData) { return false; }
    public virtual bool Use(InteractionData interactionData) { return false; }
}
