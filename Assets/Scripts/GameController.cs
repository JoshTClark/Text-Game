using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text displayText;

    [HideInInspector]
    public RoomNavigation navigation;
    [HideInInspector]
    public Interactables interactables;
    [HideInInspector]
    public TextInput textInput;

    public TextInputAction[] inputActions;

    public string[] ignoredWords;

    private List<TextActionLog> actionLog = new List<TextActionLog>();

    private GameData gameData;

    private PlayerInput playerInput;

    // Text animation stuff
    [HideInInspector]
    public bool displayingText = false;
    private bool skipText = false;
    public int lettersPerSecond;
    private float letterTimer = 0.0f;
    private int actionLogIndex = 0;
    private int visibleCharacters = 0;
    private int textIndex = 0;


    private void Awake()
    {
        textInput = GetComponent<TextInput>();
        navigation = GetComponent<RoomNavigation>();
        interactables = GetComponent<Interactables>();
        playerInput = GetComponent<PlayerInput>();

        gameData = new GameData();
    }

    private void Start()
    {
        DisplayRoomText();
        DisplayLoggedText(); 
    }

    private void Update()
    {
        if (displayingText)
        {
            playerInput.currentActionMap.FindAction("Skip").Enable();
            textInput.CanPlayerType(false);
            letterTimer += Time.deltaTime;
            string logAsText = string.Join("\n", actionLog);
            displayText.text = logAsText;
            if (letterTimer >= 1.0f / lettersPerSecond)
            {
                if (actionLogIndex < actionLog.Count)
                {
                    TextActionLog currentLog = actionLog[actionLogIndex];
                    if (currentLog.showInstant || skipText)
                    {
                        actionLogIndex++;
                        visibleCharacters += currentLog.text.Length - textIndex;
                        textIndex = 0;
                    }
                    else if (textIndex < currentLog.text.Length)
                    {
                        visibleCharacters++;
                        textIndex++;
                    }
                    else
                    {
                        actionLogIndex++;
                        textIndex = 0;
                    }

                    letterTimer = 0.0f;
                }
                else
                {
                    displayingText = false;
                    textInput.CanPlayerType(true);
                    textIndex = 0;
                    playerInput.currentActionMap.FindAction("Skip").Disable();
                }
            }
            displayText.maxVisibleCharacters = visibleCharacters + logAsText.Length - string.Join("", actionLog).Length; ;
        }
        else
        {
            skipText = false;
        }
    }

    public void DisplayLoggedText()
    {
        displayingText = true;
    }

    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();

        UnpackRoom();
        Room currentRoom = navigation.currentRoom;

        string finalDescription = currentRoom.description;

        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            InteractableObjectRoomData interactableObjectData = currentRoom.interactableObjectsInRoom[i];

            string replaceText = "<" + interactableObjectData.objectDataName + ">";

            if (finalDescription.Contains(replaceText))
            {
                if (interactables.IsObjectActivated(currentRoom, interactableObjectData.objectDataName))
                {
                    finalDescription = finalDescription.Replace(replaceText, interactableObjectData.roomDescription);
                }
                else
                {
                    finalDescription = finalDescription.Replace(replaceText, "");
                }
            }
        }

        for (int i = 0; i < currentRoom.possibleCharacterInteractionsInRoom.Count; i++)
        {
            CharacterInteractionData characterRoomData = currentRoom.possibleCharacterInteractionsInRoom[i];

            string replaceText = "<" + characterRoomData.characterDataName + ">";

            if (finalDescription.Contains(replaceText))
            {
                if (interactables.IsCharacterActivated(currentRoom, characterRoomData.characterDataName))
                {
                    finalDescription = finalDescription.Replace(replaceText, characterRoomData.roomDescription);
                }
                else
                {
                    finalDescription = finalDescription.Replace(replaceText, "");
                }
            }
        }

        LogStringWithReturn(finalDescription);
    }

    public void LogStringWithReturn(string stringToAdd, bool showInstant = false)
    {
        LogString(stringToAdd + "\n", showInstant);
    }

    public void LogString(string stringToAdd, bool showInstant = false)
    {
        TextActionLog log = new TextActionLog();
        log.text = stringToAdd;
        log.showInstant = showInstant;

        actionLog.Add(log);
    }

    public void UnpackRoom()
    {
        PrepareInteractables(navigation.currentRoom);
        navigation.UnpackExits();
    }

    public void PrepareInteractables(Room currentRoom)
    {
        interactables.SetUpInteractablesInRoom(currentRoom);

        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i].interactableObject;

            for (int j = 0; j < interactableInRoom.interactions.Count; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                for (int k = 0; k < interactableInRoom.keyWords.Count; k++)
                {
                    if (interaction.inputAction.keywords.Contains("examine"))
                    {
                        InteractionDataHolder holder = new InteractionDataHolder();
                        holder.interactionTextResponse = interaction.textResponse;
                        holder.actionResponse = interaction.actionResponse;
                        interactables.examineDictionary.Add(interactableInRoom.keyWords[k].ToLower(), holder);
                    }
                    if (interaction.inputAction.keywords.Contains("take"))
                    {
                        InteractionDataHolder holder = new InteractionDataHolder();
                        holder.interactionTextResponse = interaction.textResponse;
                        holder.actionResponse = interaction.actionResponse;
                        interactables.takeDictionary.Add(interactableInRoom.keyWords[k].ToLower(), holder);
                    }
                    if (interaction.inputAction.keywords.Contains("open"))
                    {
                        InteractionDataHolder holder = new InteractionDataHolder();
                        holder.interactionTextResponse = interaction.textResponse;
                        holder.actionResponse = interaction.actionResponse;
                        interactables.openDictionary.Add(interactableInRoom.keyWords[k].ToLower(), holder);
                    }
                    if (interaction.inputAction.keywords.Contains("use"))
                    {
                        InteractionDataHolder holder = new InteractionDataHolder();
                        holder.interactionTextResponse = interaction.textResponse;
                        holder.actionResponse = interaction.actionResponse;
                        interactables.useDictionary.Add(interactableInRoom.keyWords[k].ToLower(), holder);
                    }
                }
            }
        }

        for (int i = 0; i < currentRoom.possibleCharacterInteractionsInRoom.Count; i++)
        {
            CharacterInteractionData characterInRoom = currentRoom.possibleCharacterInteractionsInRoom[i];

            for (int j = 0; j < characterInRoom.interactions.Count; j++)
            {
                Interaction interaction = characterInRoom.interactions[j];
                List<string> totalKeywords = new List<string>();
                for (int k = 0; k < characterInRoom.characters.Count; k++) 
                {
                    totalKeywords.AddRange(characterInRoom.characters[k].keyWords);
                }
                totalKeywords.AddRange(characterInRoom.extraKeywords);
                for (int k = 0; k < totalKeywords.Count; k++)
                {
                    if (interaction.inputAction.keywords.Contains("talk") && interactables.IsCharacterActivated(currentRoom, characterInRoom.characterDataName))
                    {
                        InteractionDataHolder holder = new InteractionDataHolder();
                        holder.interactionTextResponse = interaction.textResponse;
                        holder.actionResponse = interaction.actionResponse;
                        interactables.talkDictionary.Add(totalKeywords[k].ToLower(), holder);
                    }
                }
            }
        }
    }

    public bool TestVerbDictionaryWithNoun(Dictionary<string, InteractionDataHolder> verbDictionary, OrganizedInputWordsData wordData)
    {
        List<string> keyList = new List<string>(verbDictionary.Keys);
        string nounFirstWord = wordData.nounFirstWord.ToLower();
        if (keyList.Contains(nounFirstWord))
        {
            wordData.fullNoun = wordData.nounFirstWord;
            return true;
        }
        else 
        {
            foreach (string key in keyList) 
            {
                char[] delimiterCharacters = { ' ' };
                List<string> seperatedKey = new List<string>(key.Split(delimiterCharacters));
                Debug.Log(key);
                if (seperatedKey.Count <= 1) 
                {
                    continue;
                }
                if (seperatedKey[0] == nounFirstWord)
                {
                    Debug.Log("checking key for further words");
                    int keyIndex = 0;
                    for (int i = wordData.nounStartIndex; i < wordData.seperatedInput.Length; i++) 
                    {
                        Debug.Log("checking input-" + wordData.seperatedInput[i] + " and key-" + seperatedKey[keyIndex]);
                        if (wordData.seperatedInput[i] == seperatedKey[keyIndex])
                        {
                            keyIndex++;
                            if (keyIndex >= seperatedKey.Count)
                            {
                                wordData.fullNoun = key;
                                return true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        return false;
    }

    public void OnSkip()
    {
        if (displayingText)
        {
            Debug.Log("Skipping Text");
            skipText = true;
        }
    }

    public void ClearCollectionsForNewRoom()
    {
        navigation.ClearExits();
        interactables.ClearCollections();
    }
}
