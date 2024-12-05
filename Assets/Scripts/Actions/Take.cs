using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Take")]
public class Take : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] seperatedInputWords)
    {
        Dictionary<string, string> takeDictionary = controller.interactables.Take(seperatedInputWords);

        if (takeDictionary != null)
        {
            controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(takeDictionary, seperatedInputWords[0], seperatedInputWords[1]));
        }
    }
}
