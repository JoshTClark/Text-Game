using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactables;
using static TextInput;

[CreateAssetMenu(menuName = "TextGame/InputActions/Use")]
public class Use : TextInputAction
{
    public override void RespondToInput(GameController controller, OrganizedInputWordsData wordData)
    {
        if (wordData.hasNoun)
        {
            string verb = wordData.verb;
            string noun = wordData.nounFirstWord;

            List<InteractableObject> objectsInInventory = controller.interactables.objectsInInventory;

            if (controller.TestVerbDictionaryWithNoun(controller.interactables.useDictionary, wordData))
            {
                noun = wordData.fullNoun;
                InteractionDataHolder data = controller.interactables.useDictionary[noun];

                if (data.actionResponse != null)
                {
                    bool actionResult = data.actionResponse.DoActionResponse(controller, wordData);
                    if (!actionResult)
                    {
                        controller.LogStringWithReturn("Nothing happens.");
                        return;
                    }
                }
            }
            else
            {

                for (int i = 0; i < objectsInInventory.Count; i++)
                {
                    if (objectsInInventory[i].keyWords.Contains(noun))
                    {
                        controller.LogStringWithReturn("You can't use the " + noun + ".");
                        return;
                    }
                }

                controller.LogStringWithReturn("There is no " + noun + " to use.");
            }
        }
    }
}
