using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SearchBarManager : MonoBehaviour
{
    public string ServerLoc = "http://192.168.8.121:5000";

    public InputField searchBar;
    public string searchBarText;
    public GameObject textSpacePrefab;
    public GameObject DisplayPrefab;

    public GameObject contentObj;

    private List<string> library = new List<string>{ "bengal", "bombay", "dilute tortoiseshell", "calico", "ayrshire cattle", "brown swiss cattle", "holstein cattle", "jersey cattle", "red dane cattle", "toy terrier", "beagle", "italian greyhound", "redbone coonhound", "shih tzu", "paplillon", "english fox hound", "capped heron", "egyptian goose", "mandarin duck", "red-tailed hawk", "scarlet ibis", "takahe","bear","butterfly","camel","caterpillar","cheetah","crab","crocodile","lion","snake","tiger","zebra" };

    public void UpdateLibrary(List<string> newLib)
    {
        library = newLib;
    }

    public void SearchInternal(string target)
    {
        List<string> animal_Names = BestFits(library, target);
        for(int i = 0; i < contentObj.transform.childCount; i++)
        {
            Destroy(contentObj.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < animal_Names.Count; i++)
        {
            GameObject newText = Instantiate(textSpacePrefab, contentObj.transform);
            Text text = newText.GetComponent<Text>();
            Button button = newText.GetComponent<Button>();
            textspaceScript tss = newText.GetComponent<textspaceScript>();

            text.text = animal_Names[i];
            tss.text = text;
            tss.sbm = this;
            button.onClick.AddListener(tss.PingPong_SearchBar);
        }
    }

    public void DisplayAnimalInfo()
    {
        StartCoroutine(SendSearch(searchBarText));
    }

    public List<string> BestFits(List<string> library, string target)
    {
        List<string> BestFitList = new List<string>();
        if (library.Contains(target)) { BestFitList.Add(target); }

        for(int i = 0; i < library.Count; i++)
        {
            if (BestFitList.Contains(library[i])) { continue; }
            if (library[i].Contains(target)) { BestFitList.Add(library[i]); }
        }

        return BestFitList;
    }

    IEnumerator SendSearch(string searchTerm)
    {
        print(this.name);

        string url = ServerLoc + "/search/search_bar/" + searchTerm;
        Debug.Log(url);
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            yield break;
        }

        string full_message = www.downloadHandler.text;
        Debug.Log(full_message);

        GameObject displayObj = Instantiate(DisplayPrefab,this.gameObject.transform.parent);
        DisplayPanelScript dps = displayObj.GetComponent<DisplayPanelScript>();
        dps.ActivateDisplay(full_message);
    }
}
