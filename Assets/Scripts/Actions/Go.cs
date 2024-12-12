using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TextInput;

[CreateAssetMenu(menuName = "TextGame/InputActions/Go")]
public class Go : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
        if (wordData.isValid)
        {
            string noun = wordData.nounFirstWord;
            controller.navigation.AttemptToChangeRooms(noun);
        }
        else
        {
            controller.LogStringWithReturn("where?");
        }
    }
}
