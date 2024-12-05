using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/ActionResponse/Activate Character")]
public class ActivateCharacterResponse : ActionResponse
{
    public Room room;
    public List<string> deactivateCharacters = new List<string>();
    public List<string> activateCharacters = new List<string>();
    public override bool DoActionResponse(GameController controller)
    {
        foreach (string deactivate in deactivateCharacters) 
        {
            controller.interactables.SetCharacterActive(deactivate, false, room);
        }

        foreach (string activate in activateCharacters)
        {
            controller.interactables.SetCharacterActive(activate, true, room);
        }

        return true;
    }
}
