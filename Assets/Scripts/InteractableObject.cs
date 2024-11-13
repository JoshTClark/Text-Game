using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactable Object")]
public class InteractableObject : Interactable
{
    public List<Interaction> interactions;
}
