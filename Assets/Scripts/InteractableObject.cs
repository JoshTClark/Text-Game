using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactable Object")]
public class InteractableObject : ScriptableObject
{
    public string noun = "name";
    public List<string> keyWords;
    public List<Interaction> interactions;
    [TextArea]
    public string roomDescription = "Description in room";
}
