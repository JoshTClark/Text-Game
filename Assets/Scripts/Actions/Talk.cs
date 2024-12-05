using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Talk")]
public class Talk : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] seperatedInputWords)
    {
        if (seperatedInputWords.Length > 1)
        {
            controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(controller.interactables.talkDictionary, seperatedInputWords[0], seperatedInputWords[1]));
        }
    }
}
