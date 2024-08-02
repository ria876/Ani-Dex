using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class MyScreen : MonoBehaviour
{
    private GameObject Panel;

    public void SetPanel(GameObject panel)
    {
        Panel = panel;
    }

    public virtual void onOpen() { }

    public virtual void onClose() { }

}
