using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSwipe : MonoBehaviour
{
    [SerializeField] private Sprite _imgPortrait;
    [SerializeField] private Sprite _imgLandscape;

    private Image _img;

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
}