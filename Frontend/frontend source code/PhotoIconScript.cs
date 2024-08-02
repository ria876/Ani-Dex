using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoIconScript : MonoBehaviour
{
    public RawImage selfImg;
    public Button selfButton;
    public string image_path;

    public void UpdateImage()
    {
        byte[] imageData = File.ReadAllBytes(image_path);

        Texture2D raw = new Texture2D(95,95);
        raw.LoadImage(imageData);

        selfImg.texture = raw;
    }
}
