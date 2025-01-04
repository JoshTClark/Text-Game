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
    public InteractableController interactables;
    [HideInInspector]
    public TextInput textInput;

    public TextInputAction[] inputActions;

    public string[] ignoredWords;

    private List<TextActionLog> actionLog = new List<TextActionLog>();

    private GameData gameData;

    private PlayerInput playerInput;

    private TextDataManager textDataManager;

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
        interactables = GetComponent<InteractableController>();
        playerInput = GetComponent<PlayerInput>();

        gameData = new GameData();
        textDataManager = new TextDataManager("Assets/Game Text Table - Full List.csv");
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

    public string GetText(string key) 
    {
        return textDataManager.GetText(key);
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

        string finalDescription = GetText(currentRoom.descriptionKey);

        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            InteractableObjectRoomData interactableObjectData = currentRoom.interactableObjectsInRoom[i];

            string replaceText = "<" + interactableObjectData.dataName + ">";

            if (finalDescription.Contains(replaceText))
            {
                if (interactables.IsObjectActivated(currentRoom, interactableObjectData.dataName))
                {
                    finalDescription = finalDescription.Replace(replaceText, GetText(interactableObjectData.roomDescriptionKey));
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
        if(stringToAdd == "") 
        {
            return;
        }

        LogString(stringToAdd + "\n", showInstant);
    }

    public void LogString(string stringToAdd, bool showInstant = false)
    {
        if (stringToAdd == "")
        {
            return;
        }

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
        interactables.ClearCollections();
        interactables.SetUpInteractablesInRoom(currentRoom);
    }

    public bool TestInputText(OrganizedInputWordsData wordData)
    {
        List<string> keyList = new List<string>(interactables.currentInteractableDictionary.Keys);
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
