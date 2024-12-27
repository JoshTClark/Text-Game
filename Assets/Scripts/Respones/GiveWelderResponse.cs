using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/ActionResponse/Give Plasma Welder")]
public class GiveWelderResponse : ActionResponse
{
    [TextArea(1, 100)]
    public string giveToJoanneText;
    public override bool DoActionResponse(GameController controller, OrganizedInputWordsData wordData)
    {
        RoomNavigation nav = controller.navigation;
        Interactables interactables = controller.interactables;
        if (nav.currentRoom.name == "Ship_Engine" && interactables.IsCharacterActivated(nav.currentRoom, "Joanne"))
        {
            controller.LogStringWithReturn(giveToJoanneText);
            interactables.RemoveFromInventory(wordData.fullNoun);
            interactables.SetCharacterActive("Joanne", false, nav.currentRoom);
        }
        return true;
    }
}
