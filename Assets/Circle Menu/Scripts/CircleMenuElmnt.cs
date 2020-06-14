using System;
using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public class CircleMenuElmnt : MonoBehaviour
    {
        #region FIELDS

        [SerializeField]
        private Image _imgIcon = default;

        [SerializeField]
        private EMenu _menu = default;

        public event Action<EMenu> OnClick;

        #endregion

        #region UNITY_METHODS

        private void Start()
        {
            var button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() => { OnClick?.Invoke(_menu); });
            }
        }

        #endregion

        #region PUBLIC_METHODS

        public void SetColor(bool active)
        {
            _imgIcon.color = active ? Color.white : Color.gray;
        }

        #endregion
    }
}