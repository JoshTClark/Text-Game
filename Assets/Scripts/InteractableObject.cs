using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactable Object")]
public class InteractableObject : ScriptableObject
{
    public string objectName = "Name";
    public List<string> keyWords;
    public List<Interaction> interactions;
}
