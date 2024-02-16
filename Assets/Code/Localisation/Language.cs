using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSurvivors.Localisation
{
    public class Language
    {
        private static Dictionary<string, string> currentLanguage = new Dictionary<string, string>();
        private static int languageCol = 1;

        public static void InitialiseLanguage(TextAsset csv, string language)
        {
            currentLanguage.Clear();
            //Gets the rows from the csv, fomated like: KEY, English, Spanish.....
            string[] rows = csv.text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
            int columns = rows[0].Split(new string[] { ",", "\n"}, System.StringSplitOptions.None).Length;

            string[] languagesRow = rows[0].Split(new string[] { "," }, System.StringSplitOptions.None);

            //Looks for the index corresponding to the selected language
            for (int i = 0; i < columns; i++)
            {
                if (languagesRow[i].Trim().ToLower() == language.Trim().ToLower())
                {
                    languageCol = i;
                    Debug.Log(i + ", " + language);
                    break;
                }
            }

            //Creates a matrix containing every cell ex:
            //KEY, English, Spanish
            //PLAY, Play, Jugar
            string[,] languageData = new string[rows.Length, columns];

            for (int i = 0; i < rows.Length; i++)
            {
                string[] row = rows[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                for (int j = 0; j < row.Length; j++)
                {
                    try
                    {
                        if (row[j] != "")
                            languageData[i, j] = row[j].Trim().Replace(';', ',');
                    }
                    catch
                    {
                        Debug.LogWarning("Couldn't load Row: " + i + " Col: " + j);
                    }
                }
            }

            //Fills the currentLanguage Dictionary with the keys from the first column, and the value
            //according to the selected language index
            for (int i = 0; i < languageData.Length; i++)
            {
                try
                {
                    string key = languageData[i, 0];
                    if (key == "" || key == null)
                    {
                        break;
                    }
                    string value = languageData[i, languageCol];
                    currentLanguage.Add(key, value);
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
                    Debug.LogError("Couldn't load Row: " + i + " Col: " + languageCol);
                }
            }
        }

        public static string Get(string key)
        {
            try
            {
                return currentLanguage[key.ToUpper().Trim()];
            }
            catch
            {
                //A '-' is used in text that should not be translated.
                if(key[0] == '-')
                {
                    return key.Remove(0,1);
                }
                else
                {
                    return "#" + key;
                }
            }
        }
    }
}
