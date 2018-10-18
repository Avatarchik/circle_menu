using System;
using System.Collections;
using UnityEngine;

public class DeviceOrientationManager : MonoBehaviour
{
    public static event Action<DeviceOrientation> OnDeviceOrientationChange;
    private static DeviceOrientation _deviceOrientation;
    private static float CheckDelay = 0.1f;

    private void Start()
    {
        StartCoroutine(CheckForChange());
    }

    private static IEnumerator CheckForChange()
    {
        _deviceOrientation = Input.deviceOrientation;

        while (true)
        {
            if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.Portrait)
            {
                if (_deviceOrientation != Input.deviceOrientation)
                {
                    _deviceOrientation = Input.deviceOrientation;

                    if (OnDeviceOrientationChange != null)
                        OnDeviceOrientationChange(_deviceOrientation);
                }
            }

            yield return new WaitForSeconds(CheckDelay);
        }
    }

#if UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            OnDeviceOrientationChange(DeviceOrientation.LandscapeLeft);
        }

        if (Input.GetKeyDown("p"))
        {
            OnDeviceOrientationChange(DeviceOrientation.Portrait);
        }
    }
#endif
}