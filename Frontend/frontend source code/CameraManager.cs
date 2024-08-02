using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CameraManager : MonoBehaviour
{
    private WebCamTexture currentCam;
    private Texture defaultBackground;

    public string SavePhotoPath;

    public RawImage background;
    public Button switchCameraButton;
    public Button captureCamera;
    public ScanScreenScript sss;


    public bool isCameraOpen = false;

    private int cameraIndex;

    // Start is called before the first frame update
    void Start()
    {
        switchCameraButton.onClick.AddListener(SwitchCamera);
        captureCamera.onClick.AddListener(CaptureCamera);
    }

    public void OpenCamera()
    {
        if (isCameraOpen) { return; }

        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            //camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                currentCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                cameraIndex = i;
            }
        }
        currentCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        cameraIndex = 0;

        if (currentCam == null)
        {
            Debug.Log("Unable to find back or front camera");
            return;
        }

        currentCam.Play();
        isCameraOpen = true;
        background.texture = currentCam;
    }

    public void SwitchCamera()
    {
        if (!isCameraOpen) { Debug.LogWarning("Camera is not registered as on."); return; }

        WebCamDevice[] devices = WebCamTexture.devices;
        cameraIndex = (cameraIndex + 1) % (devices.Length);
        currentCam.Stop();
        currentCam = new WebCamTexture(devices[cameraIndex].name, Screen.width, Screen.height);
        currentCam.Play();
        background.texture = currentCam;
    }

    public void StopCamera()
    {
        currentCam.Stop();
        background.texture = null;
        isCameraOpen = false;
    }

    public void CaptureCamera()
    {
        if (!isCameraOpen) { Debug.LogWarning("Camera is not registered on. must be on to capture"); }
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();

        Texture2D photo = new Texture2D(currentCam.width, currentCam.height);
        photo.SetPixels(currentCam.GetPixels());
        photo.Apply();
        Resize(photo, 250, 250);
        

        byte[] bytes = photo.EncodeToPNG();
        string FILE_PATH = SavePhotoPath + "\\photo" + Time.time + ".png";
        for(int i = 0; i < 1000; i++)
        {
            if (!File.Exists(FILE_PATH)) { break; }
            FILE_PATH = SavePhotoPath + "\\photo" + Time.time + "_" + Random.Range(1,1000) + "_" + Random.Range(1, 1000) + ".png";
        }

        File.WriteAllBytes(FILE_PATH, bytes);

        sss.SendImage(bytes);
    }

    private static void Resize(Texture2D texture, int newWidth, int newHeight)
    {
        RenderTexture tmp = RenderTexture.GetTemporary(newWidth, newHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        RenderTexture.active = tmp;
        Graphics.Blit(texture, tmp);
        texture.Reinitialize(newWidth, newHeight, texture.format, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.ReadPixels(new Rect(Vector2.zero, new Vector2(newWidth, newHeight)), 0, 0);
        texture.Apply();
        RenderTexture.ReleaseTemporary(tmp);
    }
}
