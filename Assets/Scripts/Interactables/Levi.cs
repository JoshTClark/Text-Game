using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/Interactables/Levi")]
public class Levi : Interactable
{
    public string helmTalkKey;
    public override bool Talk(InteractionData interactionData)
    {
        string currentRoom = interactionData.controller.navigation.currentRoom.name;
        if (currentRoom == "Ship_Helm")
        {
            interactionData.controller.LogStringWithReturn(interactionData.controller.GetText(helmTalkKey));
            return true;
        }

        return false;
    }
}
