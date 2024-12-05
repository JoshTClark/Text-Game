using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="TextGame/InputActions/Go")]
public class Go : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            controller.navigation.AttemptToChangeRooms(separatedInputWords[1]);
        }
        else 
        {
            controller.LogStringWithReturn(separatedInputWords[0] + " where?");
        }
    }
}
