#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Collections;
using System.IO;

public enum CaptureMethod
{
    AppCapture_Asynch,
    AppCapture_Synch,
    ReadPixels_Asynch,
    ReadPixels_Synch,
    RenderToTex_Asynch,
    RenderToTex_Synch
}

public class Screenshot : MonoBehaviour
{
    public CaptureMethod captureMethod;
    public KeyCode captureButton = KeyCode.Home;

    public int Supersize = 1;
    public Rect Dimensions = new Rect(0f, 0f, 1920f, 1080f);

    public string m_sRootLocation = string.Empty;
    public const string SCREENSHOT_FOLDER_LOCATION = "Screenshots";
    private const string FILE_TYPE = ".png";
    public int m_iShotNumber;
    private string m_sDatetime;

    private void OnEnable()
    {
        m_sRootLocation = Application.dataPath + "/../";
        FileManager.CheckFolderLocation(m_sRootLocation + SCREENSHOT_FOLDER_LOCATION);
    }

    public void Update()
    {
#if UNITY_EDITOR
        if (true == EditorApplication.isPlaying)
        {
            if (true == Input.GetKeyDown(captureButton))
            {
                TakeShot();
            }
        }
#endif
    }

    public void TakeShot()
    {
        m_sDatetime = DateTime.Now.ToString("HH:mm:ss");
        m_sDatetime = m_sDatetime.Replace(":", "-");

        SaveScreenshot(m_sRootLocation + SCREENSHOT_FOLDER_LOCATION + Path.DirectorySeparatorChar + m_iShotNumber + " " + m_sDatetime + FILE_TYPE);

        m_iShotNumber++;
    }

    public void SaveScreenshot(string filePath)
    {
        if (captureMethod == CaptureMethod.AppCapture_Asynch)
        {
            ScreenCapture.CaptureScreenshot(filePath, Supersize);
        }
        else if (captureMethod == CaptureMethod.AppCapture_Synch)
        {
            Texture2D texture = GetScreenshot(CaptureMethod.AppCapture_Synch);
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }
        else if (captureMethod == CaptureMethod.ReadPixels_Asynch)
        {
            StartCoroutine(SaveScreenshot_ReadPixelsAsynch(filePath));
        }
        else if (captureMethod == CaptureMethod.ReadPixels_Synch)
        {
            Texture2D texture = GetScreenshot(CaptureMethod.ReadPixels_Synch);

            byte[] bytes = texture.EncodeToPNG();

            //Save our test image (could also upload to WWW)
            File.WriteAllBytes(filePath, bytes);

            //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            Destroy(texture);
        }
        else if (captureMethod == CaptureMethod.RenderToTex_Asynch)
        {
            StartCoroutine(SaveScreenshot_RenderToTexAsynch(filePath));
        }
        else
        {
            Texture2D screenShot = GetScreenshot(CaptureMethod.RenderToTex_Synch);
            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }
    }

    private IEnumerator SaveScreenshot_ReadPixelsAsynch(string filePath)
    {
        //Wait for graphics to render
        yield return new WaitForEndOfFrame();

        //Create a texture to pass to encoding
        Texture2D texture = new Texture2D((int)Dimensions.width, (int)Dimensions.height, TextureFormat.RGB24, false);

        //Put buffer into texture
        texture.ReadPixels(Dimensions, 0, 0);

        //Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        byte[] bytes = texture.EncodeToPNG();

        //Save our test image (could also upload to WWW)
        File.WriteAllBytes(filePath, bytes);

        //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        Destroy(texture);
    }

    private IEnumerator SaveScreenshot_RenderToTexAsynch(string filePath)
    {
        //Wait for graphics to render
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture((int)Dimensions.width, (int)Dimensions.height, 24);
        Texture2D screenShot = new Texture2D((int)Dimensions.width, (int)Dimensions.height, TextureFormat.RGB24, false);

        //Camera.main.targetTexture = rt;
        //Camera.main.Render();

        //Render from all!
        foreach (Camera cam in Camera.allCameras)
        {
            cam.targetTexture = rt;
            cam.Render();
            cam.targetTexture = null;
        }

        RenderTexture.active = rt;
        screenShot.ReadPixels(Dimensions, 0, 0);
        if (null == Camera.main)
        {
            Debug.LogError("No Main Camera");
        }
        else
        {
            Camera.main.targetTexture = null;
            RenderTexture.active = null; //Added to avoid errors
            Destroy(rt);

            //Split the process up
            yield return 0;

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }
    }

    private static int tempFileCount = 0;
    ///<summary>Must use a Synch capture type to work.</summary>
    public Texture2D GetScreenshot(CaptureMethod method)
    {
        if (method == CaptureMethod.AppCapture_Synch)
        {
            string tempFilePath = System.Environment.GetEnvironmentVariable("TEMP") + "/screenshotBuffer" + tempFileCount + ".png";
            tempFileCount++;
            ScreenCapture.CaptureScreenshot(tempFilePath, Supersize);
            WWW www = new WWW("file:///" + tempFilePath.Replace(Path.DirectorySeparatorChar.ToString(), "/"));

            Texture2D texture = new Texture2D((int)Dimensions.width, (int)Dimensions.height, TextureFormat.RGB24, false);
            while (!www.isDone) { }
            www.LoadImageIntoTexture((Texture2D)texture);
            File.Delete(tempFilePath); //Can delete now

            return texture;
        }
        else if (method == CaptureMethod.ReadPixels_Synch)
        {
            //Create a texture to pass to encoding
            Texture2D texture = new Texture2D((int)Dimensions.width, (int)Dimensions.height, TextureFormat.RGB24, false);

            //Put buffer into texture
            texture.ReadPixels(Dimensions, 0, 0); //Unity complains about this line's call being made "while not inside drawing frame", but it works just fine.*

            return texture;
        }
        else if (method == CaptureMethod.RenderToTex_Synch)
        {
            RenderTexture rt = new RenderTexture((int)Dimensions.width, (int)Dimensions.height, 24);
            Texture2D screenShot = new Texture2D((int)Dimensions.width, (int)Dimensions.height, TextureFormat.RGB24, false);

            //Camera.main.targetTexture = rt;
            //Camera.main.Render();

            //Render from all!
            foreach (Camera cam in Camera.allCameras)
            {
                cam.targetTexture = rt;
                cam.Render();
                cam.targetTexture = null;
            }

            RenderTexture.active = rt;
            screenShot.ReadPixels(Dimensions, 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null; //Added to avoid errors
            Destroy(rt);

            return screenShot;
        }
        else
            return null;
    }
}