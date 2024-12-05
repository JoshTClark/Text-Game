using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactables;

[CreateAssetMenu(menuName = "TextGame/InputActions/Use")]
public class Use : TextInputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            string verb = separatedInputWords[0];
            string noun = separatedInputWords[1];

            List<InteractableObject> objectsInInventory = controller.interactables.objectsInInventory;

            if (controller.TestVerbDictionaryWithNoun(controller.interactables.useDictionary, verb, noun))
            {
                InteractionDataHolder data = controller.interactables.useDictionary[noun];

                if (data.actionResponse != null)
                {
                    bool actionResult = data.actionResponse.DoActionResponse(controller);
                    if (!actionResult)
                    {
                        controller.LogStringWithReturn("Nothing happens.");
                        return;
                    }
                }
            }

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
