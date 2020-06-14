using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public class CircleMenuSpriteSwipe : MonoBehaviour
    {
        #region FIELDS

        [SerializeField]
        private Image _iconShow = default;

        [SerializeField]
        private Image _iconHide = default;

        private bool _isShow = false;

        #endregion

        #region UNITY_METHODS

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

        #endregion
    }
}