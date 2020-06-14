using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public class CircleMenuController : CircleMenuBasic
    {
        #region FIELDS

        [Header("Menu")]
        [SerializeField]
        private CircleMenu _circleMenu = default;

        [Header("Buttons")]
        [SerializeField]
        private Button _btnMainMenu = default;

        [Header("Texts")]
        [SerializeField]
        private Text _txtMenu = default;

        private bool _isShow = true;

        #endregion

        #region UNITY_METHODS

        private void Start()
        {
            _btnMainMenu.onClick.AddListener(() =>
            {
                if (_isShow)
                {
                    _isShow = false;
                    SetActive(true);
                    _circleMenu.Focus(0);
                }
                else
                {
                    _isShow = true;
                    SetActive(false);
                }
            });

            _circleMenu.OnClick += OnClick;
        }

        #endregion

        #region PRIVATE_METHODS

        private void OnClick(EMenu menu)
        {
            if (_txtMenu != null)
            {
                _txtMenu.text = menu.ToString();
            }
        }

        #endregion
    }
}