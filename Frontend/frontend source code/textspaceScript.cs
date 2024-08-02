using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textspaceScript : MonoBehaviour
{
    public SearchBarManager sbm;
    public Text text;

    public void PingPong_SearchBar()
    {
        sbm.searchBarText = text.text.ToLower();
        sbm.DisplayAnimalInfo();
    }
}
