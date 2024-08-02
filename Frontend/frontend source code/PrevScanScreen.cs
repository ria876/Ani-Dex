using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PrevScanScreen : MyScreen
{
    public string images_file_path;
    public GameObject content;
    public GameObject PhotoPrefab;


    public override void onOpen()
    {
        if (!Directory.Exists(images_file_path)) { Debug.LogError("Image folder can't be found!"); }

        string[] files = Directory.GetFiles(images_file_path);
        foreach(string path in files)
        {
            if (!path.EndsWith(".png")) { continue; }
            GameObject photoObj = Instantiate(PhotoPrefab, content.transform);
            PhotoIconScript photo = photoObj.GetComponent<PhotoIconScript>();
            photo.image_path = path;
            photo.UpdateImage();
        }
    }

    public override void onClose()
    {
        for(int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
