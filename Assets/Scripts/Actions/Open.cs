using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Open")]
public class Open : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
        if (wordData.hasNoun)
        {
            string verb = wordData.verb;
            string noun = wordData.nounFirstWord;
            if (controller.TestVerbDictionaryWithNoun(controller.interactables.openDictionary, wordData))
            {
                noun = wordData.fullNoun;
                InteractionDataHolder data = controller.interactables.openDictionary[noun];
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
