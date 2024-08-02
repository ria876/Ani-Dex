using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPanelScript : MonoBehaviour
{
    public List<Text> FeildsList = new List<Text>();


    public void ActivateDisplay(string message)
    {
        UpdateText(TransilateAnimalInfo(message));
    }

    public Dictionary<string,string> TransilateAnimalInfo(string message)
    {
        Dictionary<string, string> info = new Dictionary<string, string>();

        string trimed_message = message.TrimStart('{').TrimStart('\n').TrimEnd('}').TrimEnd('\n');

        foreach(string pair in trimed_message.Split("<pair>"))
        {
            if (!pair.Contains("</>")) { continue; }
            string[] vals = pair.Split("</>");
            info[vals[0]] = vals[1];
        }

        return info;
    }

    public void UpdateText(Dictionary<string,string> info)
    {
        int i = 0;
        foreach(KeyValuePair<string,string> pair in info)
        {
            FeildsList[i].text = pair.Key + ": " + pair.Value;
            if (i < FeildsList.Count - 1) { i++;}
            else { break; }
        }

        if(i < FeildsList.Count && i >= info.Count)
        {
            for(int j = i; i < FeildsList.Count; j++)
            {
                FeildsList[j].text = "";
            }
        }
    }

    public void ClosePanel()
    {
        print("Man i'm...");
        Destroy(this.gameObject);
    }
}
