using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScanScreenScript : MyScreen
{
    public string ServerLoc = "http://192.168.8.121:5000";
    public CameraManager camM;
    public string[] info_dumps;
    public int animal_info_index = 0;

    public GameObject DisplayPrefab;
    public GameObject ScanDSPrefab;

    public GameObject currentDsp;

    public override void onOpen()
    {
        camM.OpenCamera();
    }

    public override void onClose()
    {
        camM.StopCamera();
    }

    public void SendImage(byte[] bytes)
    {
        StartCoroutine(UploadImage(bytes));
    }

    public void SendImage_v2(byte[] bytes)
    {
        print("I did the thing");
        StartCoroutine(UploadImage_v2(bytes));
    }

    IEnumerator UploadImage(byte[] bytes)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", bytes, "image.png", "image/png");

        string url = ServerLoc + "/search/image";
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            www.Dispose();
            yield break;
        }

        Debug.Log(www.downloadHandler.text);

        GameObject displayObj = Instantiate(DisplayPrefab, this.transform);
        DisplayPanelScript dps = displayObj.GetComponent<DisplayPanelScript>();
        dps.ActivateDisplay(www.downloadHandler.text);

    }

    IEnumerator UploadImage_v2(byte[] bytes)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", bytes, "image.png", "image/png");

        string url = ServerLoc + "/search/image/multi_guess";
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            www.Dispose();
            yield break;
        }

        Debug.Log(www.downloadHandler.text);

        GameObject displayObj = Instantiate(ScanDSPrefab, this.transform);
        DisplayPanelScript dps = displayObj.GetComponent<DisplayPanelScript>();
        DisplayExtraScript dpsX = displayObj.GetComponentInChildren<DisplayExtraScript>();
        
        info_dumps = www.downloadHandler.text.Split("<guess>");
        animal_info_index = 0;

        dpsX.switchButton.onClick.AddListener(SwitchDisplay);

        dps.ActivateDisplay(info_dumps[0]);
        currentDsp = displayObj;
    }

    public void SwitchDisplay()
    {
        print(info_dumps.Length);
        animal_info_index = (animal_info_index + 1)%info_dumps.Length;
        currentDsp.GetComponent<DisplayPanelScript>().ActivateDisplay(info_dumps[animal_info_index]);
    }


}
