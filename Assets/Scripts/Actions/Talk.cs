using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextGame/InputActions/Talk")]
public class Talk : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
        if (wordData.hasNoun)
        {
            string verb = wordData.verb;
            string noun = wordData.nounFirstWord;
            if (controller.TestVerbDictionaryWithNoun(controller.interactables.talkDictionary, wordData))
            {
                noun = wordData.fullNoun;
                InteractionDataHolder data = controller.interactables.talkDictionary[noun];
                if (data.actionResponse != null)
                {
                    data.actionResponse.DoActionResponse(controller, wordData);
                }

                controller.LogStringWithReturn(data.interactionTextResponse);
            }
            else
            {
                controller.LogStringWithReturn("You can't " + verb + " to " + noun);
            }
        }
    }
}
