using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactables;
using static TextInput;

[CreateAssetMenu(menuName = "TextGame/InputActions/Examine")]
public class Examine : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
        if (wordData.hasNoun)
        {
            string verb = wordData.verb;
            string noun = wordData.nounFirstWord;
            if (controller.TestVerbDictionaryWithNoun(controller.interactables.examineDictionary, wordData))
            {
                noun = wordData.fullNoun;
                InteractionDataHolder data = controller.interactables.examineDictionary[noun];
                if (data.actionResponse != null) 
                {
                    data.actionResponse.DoActionResponse(controller, wordData);
                    Debug.Log("did action response");
                }

                controller.LogStringWithReturn(data.interactionTextResponse);
            }
            else
            {
                controller.LogStringWithReturn("You can't " + verb + " " + noun);
            }
        }
    }
}
