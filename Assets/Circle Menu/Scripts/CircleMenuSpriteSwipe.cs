using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleMenuSpriteSwipe : MonoBehaviour
{
    [SerializeField] private Image _iconShow;
    [SerializeField] private Image _iconHide;

    private bool _isShow = false;

    private void Start()
    {
        var button = GetComponent<Button>();

        _iconHide.gameObject.SetActive(false);
        _iconShow.gameObject.SetActive(true);

        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                if (_isShow)
                {
                    _isShow = false;

                    _iconHide.gameObject.SetActive(false);
                    _iconShow.gameObject.SetActive(true);
                }
                else
                {
                    _isShow = true;

                    _iconShow.gameObject.SetActive(false);
                    _iconHide.gameObject.SetActive(true);
                }
            });
        }
    }
}