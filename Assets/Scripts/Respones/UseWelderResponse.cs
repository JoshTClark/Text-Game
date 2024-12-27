using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/ActionResponse/Use Plasma Welder")]
public class UseWelderResponse : ActionResponse
{
    [TextArea(1, 100)]
    public string specialBoxText;
    [TextArea(1, 100)]
    public string redundantText;
    public override bool DoActionResponse(GameController controller, OrganizedInputWordsData wordData)
    {
        RoomNavigation nav = controller.navigation;
        Interactables interactables = controller.interactables;
        if (nav.currentRoom.name == "Ship_Storage" && interactables.IsObjectActivated(nav.currentRoom, "box"))
        {
            controller.LogStringWithReturn(specialBoxText);
        }
        else
        {
            controller.LogStringWithReturn(redundantText);
        }
        return true;
    }
}
