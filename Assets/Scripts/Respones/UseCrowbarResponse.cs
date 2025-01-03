using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/ActionResponse/Use Crowbar")]
public class UseCrowbarResponse : ActionResponse
{
    [TextArea(1, 100)]
    public string ableToUseText;
    [TextArea(1, 100)]
    public string allieNotInRoomText;
    [TextArea(1, 100)]
    public string redundantText;
    public override bool DoActionResponse(GameController controller, OrganizedInputWordsData wordData)
    {
        RoomNavigation nav = controller.navigation;
        Interactables interactables = controller.interactables;
        if (nav.currentRoom.name == "Ship_Storage" && interactables.IsObjectActivated(nav.currentRoom, "box"))
        {
            if (interactables.IsCharacterActivated(nav.currentRoom, "Allie1"))
            {
                interactables.SetObjectActive("box", false, nav.currentRoom);
                interactables.SetCharacterActive("allie1", false, nav.currentRoom);
                controller.LogStringWithReturn(ableToUseText);
            }
            else 
            {
                controller.LogStringWithReturn(allieNotInRoomText);
            }
        }
        else
        {
            controller.LogStringWithReturn(redundantText);
        }
        return true;
    }

}
