using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScreenManager : MonoBehaviour
{
    public MyScreen currentScreen;

    public void SwitchScreens(MyScreen nextScreen)
    {
        GameObject nextObj = nextScreen.gameObject;
        GameObject currentObj = currentScreen.gameObject;

        currentScreen.onClose();
        currentObj.SetActive(false);

        nextObj.SetActive(true);
        nextScreen.onOpen();

        currentScreen = nextScreen;
    }

    public void ClossProgram()
    {
        Application.Quit();
    }
}
