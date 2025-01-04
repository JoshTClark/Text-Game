using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TextDataManager
{
    private Dictionary<string, string> allText = new Dictionary<string, string>();
    public TextDataManager(TextAsset textFile)
    {
        string text = textFile.text;
        Regex regex = new Regex("(.*)\t(.*)");
        string[] splitText = regex.Split(text);
        for (int i = 0; i < splitText.Length; i+=3)
        {
            string str = splitText[i];
            Debug.Log(str);
        }
    }

    public string GetText(string key)
    {
        if (allText.ContainsKey(key))
        {
            return allText[key];
        }
        else
        {
            Debug.LogError("Could not find text key - " + key);
            return key;
        }
    }
}
