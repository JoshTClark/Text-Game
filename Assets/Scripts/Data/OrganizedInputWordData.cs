using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizedInputWordsData
{
    public string rawInput;
    public string[] seperatedInput;
    public string verb;
    public string nounFirstWord;
    public int nounStartIndex;
    public bool isValid;
    public string fullNoun;
    public OrganizedInputWordsData(string input, GameController controller)
    {
        rawInput = input;
        isValid = rawInput.Length > 1;
        if (isValid)
        {
            char[] delimiterCharacters = { ' ' };
            seperatedInput = input.Split(delimiterCharacters);
            verb = seperatedInput[0];
            string[] ignoredWords = controller.ignoredWords;
            for (int i = 1; i < seperatedInput.Length; i++)
            {
                string inputWord = seperatedInput[i].ToLower();
                bool isValid = true;
                for (int x = 0; x < ignoredWords.Length; x++)
                {
                    if (inputWord == ignoredWords[x].ToLower())
                    {
                        isValid = false;
                        break;
                    }
                }
                if (isValid) 
                {
                    nounStartIndex = i;
                    nounFirstWord = seperatedInput[i];
                    break;
                }
            }
        }
    }
}
