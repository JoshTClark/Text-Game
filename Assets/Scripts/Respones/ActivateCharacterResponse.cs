using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/ActionResponse/Activate Character")]
public class ActivateCharacterResponse : ActionResponse
{
    [SerializeField]
    private List<CharacterActivationData> characters = new List<CharacterActivationData>();
    public override bool DoActionResponse(GameController controller)
    {
        foreach (CharacterActivationData c in characters) 
        {
            controller.interactables.SetCharacterActive(c.characterName, c.setActive, c.room);
        }

        return true;
    }

    [Serializable]
    private class CharacterActivationData
    {
        public string characterName = "";
        public bool setActive = false;
        public Room room;
    }
}
