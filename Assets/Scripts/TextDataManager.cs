using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TextDataManager
{
    private Dictionary<string, string> allText = new Dictionary<string, string>();
    public TextDataManager(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            using (CsvReader reader = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
                reader.Read();
                reader.ReadHeader();
                while (reader.Read())
                {
                    Debug.Log(reader.GetField("Key") + " - " + reader.GetField("Text"));
                    allText.Add(reader.GetField("Key"), reader.GetField("Text"));
                }
            }
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
