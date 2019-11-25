using System;
using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public enum EMenu
    {
        Default = 0,
        GitHub = 1,
        Instagram = 2,
        Facebook = 3,
        Skype = 4,
        Snapchat = 5,
        Viber = 6,
        Whatsapp = 7,
        Soundcloud = 8,
        Linkedin = 9,
        GooglePlus = 10,
        GoogleDrive = 11,
        Twitter = 12
    }

    public class CircleMenuElmnt : MonoBehaviour
    {
        [SerializeField] private Image _imgIcon = default;
        [SerializeField] private EMenu _menu = default;

        public event Action<EMenu> OnClick;

        private void Start()
        {
            var button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() => { OnClick?.Invoke(_menu); });
            }
        }

        public void SetColor(bool active)
        {
            _imgIcon.color = active ? Color.white : Color.gray;
        }
    }
}