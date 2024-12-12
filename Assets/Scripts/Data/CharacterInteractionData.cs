using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInteractionData
{
    public string characterDataName = "Name used to activate/deactivate character";
    public List<Character> characters;
    public List<string> extraKeywords;
    public bool activatedAtStart;
    [TextArea]
    public string roomDescription = "Description in room";
    public List<Interaction> interactions;
}