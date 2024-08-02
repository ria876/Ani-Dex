using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

public class TestHTTPMessages: MonoBehaviour
{
    public string ServerLoc = "http://192.168.8.121:5000";
    public string image_path;
    public string searchTerm;
    public Text Report;

    public void HelloWorldGetTest()
    {
        StartCoroutine(GetNatrualResponce(ServerLoc, "/hello"));   
    }

    public void UploadImageTest()
    {
        StartCoroutine(UploadImage(image_path));
    }

    public void SearchBarTest()
    {
        StartCoroutine(SearchBarUpload(searchTerm));
    }

    IEnumerator GetNatrualResponce(string host, string requestRoute)
    {
        string url = host + requestRoute;
        Debug.Log(url);
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            yield break;
        }

        foreach(KeyValuePair<string,string> pair in www.GetResponseHeaders())
        {
            Debug.Log(pair.Key + ":" + pair.Value);
        }

        Debug.Log(www.downloadHandler.text);
    }

    IEnumerator UploadImage(string client_file_path)
    {
        byte[] imageData = File.ReadAllBytes(client_file_path);
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, "image.png", "image/png");

        string url = ServerLoc + "/testing/image_upload";
        UnityWebRequest www = UnityWebRequest.Post(url,form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            www.Dispose();
            yield break;
        }

        Debug.Log(www.downloadHandler.text);
        www.Dispose();
    }

    IEnumerator SearchBarUpload(string searchTerm)
    {
        string url = ServerLoc + "/testing/search_bar/" + searchTerm;
        Debug.Log(url);
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Report.text = www.error;
            yield break;
        }

        string full_message = www.downloadHandler.text;

        //if (!full_message.StartsWith("{\ninfo")) { Debug.Log(full_message); yield break; }

        full_message = full_message.Replace("\n", "");
        full_message = full_message.TrimStart('{');
        full_message = full_message.TrimEnd('}');

        string[] animal_info = full_message.Substring(9).TrimStart('{').TrimEnd('}').Split('\"');

        Dictionary<string, string> message_info = new Dictionary<string, string>();
        for(int i = 0; i + 4 < animal_info.Length; i = i + 4)
        {
            message_info[animal_info[i + 1]] = animal_info[i + 3];
        }

        foreach(KeyValuePair<string,string> pair in message_info)
        {
            Debug.Log(pair.Key + ":" + pair.Value);
        }
    }
}
