using UnityEngine;
using System.Collections;

using System.IO;
using System.Text.RegularExpressions;

public class ReadFile // : MonoBehaviour
{
    public struct ColorData
    {
        public string name;
        public Entry.Color col;
    }

    private ColorData[] _colors = {
                                  new ColorData() { name = "blue", col = Entry.Color.BLUE },
                                  new ColorData() { name = "black", col = Entry.Color.BLACK },
                                  new ColorData() { name = "green", col = Entry.Color.GREEN },
                                  new ColorData() { name = "yellow", col = Entry.Color.YELLOW },
                                  new ColorData() { name = "orange", col = Entry.Color.ORANGE },
                                  new ColorData() { name = "red", col = Entry.Color.RED },
                                  new ColorData() { name = "white", col = Entry.Color.WHITE }
                              };

    bool checkColor(string line, string s, Entry.Color c, int index)
    {
        if (line == s)
        {
            GameController.Instance.countries[index].setColor(c);
            return true;
        }
        return false;
    }

    public void Awake()
    {
        Debug.Log("DEBUT AWAKE READFILE");
      //  DontDestroyOnLoad(this.gameObject);

        TextAsset levelFile = (TextAsset)Resources.Load("countries", typeof(TextAsset));

        if (levelFile == null)
        {
            Debug.LogError("fail fichier");
        }

        StringReader readertmp = new StringReader(levelFile.text);
        if (readertmp == null)
        {
            Debug.LogError("map not found or not readable");
        }
        int len = 0;
        for (string line = readertmp.ReadLine(); line != null; line = readertmp.ReadLine())
            ++len;
        
        StringReader reader = new StringReader(levelFile.text);
        if (reader == null)
        {
            Debug.LogError("map not found or not readable");
        }

        GameController.Instance.countries = new GameController.Country[len];
        int i = 0;
        GameController.Instance.nbCountries = len;

        Debug.Log("FOR");
        for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
            line = line.Trim().ToLower();
            line = Regex.Replace(line, @"\s{2,}", " ");
            string[] sp = line.Split(' ');

            char[] name = sp[0].ToCharArray();
            name[0] = char.ToUpper(name[0]); 
            
            GameController.Instance.countries[i] = new GameController.Country(new string(name));
            foreach (string s in sp)
            {
                foreach (ColorData cd in _colors ) {
                    if (checkColor(s, cd.name, cd.col, i) == true)
                        break;
                }

            }
            ++i;
        }


    }
}
