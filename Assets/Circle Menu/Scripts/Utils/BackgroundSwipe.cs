using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu.Utils
{
    public class BackgroundSwipe : MonoBehaviour
    {
        #region FIELDS

        [SerializeField]
        private Sprite _imgPortrait = default;

        [SerializeField]
        private Sprite _imgLandscape = default;

        private Image _img;

        #endregion

        #region UNITY_METHODS

        private void Start()
        {
            _img = GetComponent<Image>();
        }

        private void OnEnable()
        {
            DeviceOrientationManager.OnDeviceOrientationChange += SetOrientation;
        }

        private void OnDisable()
        {
            DeviceOrientationManager.OnDeviceOrientationChange -= SetOrientation;
        }

        #endregion

        #region PRIVATE_METHODS

        private void SetOrientation(DeviceOrientation orientation)
        {
            StopAllCoroutines();

            switch (orientation)
            {
                case DeviceOrientation.Portrait:
                    Portrait();
                    break;
                case DeviceOrientation.PortraitUpsideDown:
                    Portrait();
                    break;
                case DeviceOrientation.LandscapeLeft:
                    Landscape();
                    break;
                case DeviceOrientation.LandscapeRight:
                    Landscape();
                    break;
                case DeviceOrientation.FaceUp:
                    break;
            }
        }

        private void Portrait()
        {
            _img.sprite = _imgPortrait;
        }

        private void Landscape()
        {
            _img.sprite = _imgLandscape;
        }

        #endregion
    }
}