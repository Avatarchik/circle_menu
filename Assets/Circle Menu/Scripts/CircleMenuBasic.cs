using CircleMenu.Utils;
using UnityEngine;

namespace CircleMenu
{
    public class CircleMenuBasic : MonoBehaviour
    {
        #region FIELDS

        [Header("Root")]
        [SerializeField]
        private GameObject _root = default;

        private CanvasGroup _canvasGroup;

        #endregion

        #region UNITY_METHODS

        private void Awake()
        {
            _canvasGroup = _root.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0.0f;
            SetActive(false);
        }

        #endregion

        #region PUBLIC_METHODS

        protected void SetActive(bool value)
        {
            _root.SetActive(value);
            StopAllCoroutines();

            StartCoroutine(value
                ? Fade.In(_canvasGroup, 1.0f, 0.5f)
                : Fade.Out(_canvasGroup, 0.0f, 0.5f));
        }

        #endregion
    }
}