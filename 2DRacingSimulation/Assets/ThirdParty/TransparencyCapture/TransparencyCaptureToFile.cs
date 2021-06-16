using System.Collections;
using UnityEditor;
using UnityEngine;

namespace RacingSimulation.ThirdParty
{
    public class TransparencyCaptureToFile : MonoBehaviour
    {
        public IEnumerator Capture(string path)
        {

            yield return new WaitForEndOfFrame();
            //After Unity4,you have to do this function after WaitForEndOfFrame in Coroutine
            //Or you will get the error:"ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame"
            TransparencyCapture.CaptureScreenshot(path);

            yield return new WaitForEndOfFrame();
            AssetDatabase.Refresh();
            DestroyImmediate(this.gameObject);
        }
    }
}