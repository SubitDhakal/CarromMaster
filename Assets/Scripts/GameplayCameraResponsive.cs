using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCameraResponsive : MonoBehaviour
{

    private void Start()
    {
        //float orthoSize = rink.bounds.size.x * 0.5f * Screen.height / Screen.width;

        float y = Screen.height;
        float x = Screen.width;

        Debug.Log(x + " " + y);

        float cameraOrthoSize = GetComponent<Camera>().orthographicSize;

        float orthoSize = y/x * cameraOrthoSize * 0.5f;

        if(orthoSize >= cameraOrthoSize)
            GetComponent<Camera>().orthographicSize = orthoSize;
        else
        {
            orthoSize = y / x *cameraOrthoSize * (1+0.056f) * 0.5f;
            GetComponent<Camera>().orthographicSize = orthoSize;
        }
    }
}
