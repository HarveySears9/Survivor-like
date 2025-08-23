using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShots : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        { // capture screen shot on left mouse button down

            string folderPath = "C:\\Users\\0e\\Desktop\\Shark Surge Screenshots"; // the path of your project folder

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created

            var screenshotName =
                                    "Screenshot_" +
                                    System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + // puts the current time right into the screenshot name
                                    ".png"; // put youre favorite data format here
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName)); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            Debug.Log(folderPath + screenshotName); // You get instant feedback in the console
        }
    }
}
