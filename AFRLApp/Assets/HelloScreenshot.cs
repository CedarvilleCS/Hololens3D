using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloScreenshot : MonoBehaviour
{ 
    private void Start()
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png");
    }
}
