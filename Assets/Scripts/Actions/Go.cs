using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="TextGame/InputActions/Go")]
public class Go : InputAction
{
    public override void RespondToInput(GameController controller, string[] seperatedInputWords)
    {
        if (seperatedInputWords.Length > 1)
        {
            controller.navigation.AttemptToChangeRooms(seperatedInputWords[1]);
        }
        else 
        {
            controller.LogStringWithReturn(seperatedInputWords[0] + " where?");
        }
    }
}
