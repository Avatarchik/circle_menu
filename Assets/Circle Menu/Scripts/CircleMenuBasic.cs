using UnityEngine;

public class CircleMenuBasic : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject _root;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = _root.AddComponent<CanvasGroup>();
        _canvasGroup.alpha = 0.0f;
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        _root.SetActive(value);
        StopAllCoroutines();

        StartCoroutine(value
            ? Utils.FadeIn(_canvasGroup, 1.0f, 0.5f)
            : Utils.FadeOut(_canvasGroup, 0.0f, 0.5f));
    }
}