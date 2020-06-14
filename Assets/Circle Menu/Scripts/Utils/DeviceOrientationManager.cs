using System;
using System.Collections;
using UnityEngine;

namespace CircleMenu.Utils
{
    public class DeviceOrientationManager : MonoBehaviour
    {
        #region FIELDS

        public static event Action<DeviceOrientation> OnDeviceOrientationChange;
        private static DeviceOrientation _deviceOrientation;
        private const float CHECK_DELAY = 0.1f;

        #endregion

        #region UNITY_METHODS

        private void Start()
        {
            StartCoroutine(CheckForChange());
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

        #endregion

        #region PRIVATE_METHODS

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

                        OnDeviceOrientationChange?.Invoke(_deviceOrientation);
                    }
                }

                yield return new WaitForSeconds(CHECK_DELAY);
            }
        }

        #endregion
    }
}