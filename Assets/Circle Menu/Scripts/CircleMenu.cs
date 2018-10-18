using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CircleMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] [Range(0f, 360f)] private float _angle;
    [SerializeField] [Range(-360f, 360f)] private float _angleOffset = 45.0f;
    [SerializeField] [Range(0.01f, 1f)] private float _spring = 0.3f;
    [SerializeField] [Range(1f, 2f)] private float _selectedScale = 1.2f;
    [SerializeField] [Range(0.1f, 0.5f)] private float _sensitivity = 0.25f;
    [SerializeField] private List<GameObject> _items;
    
    public event Action<EMenu> OnClick;
    
    private Canvas _canvas;
    private RadialLayout _radialLayout;

    private int _inner;
    private int _count;

    public int SelectedIndex
    {
        private get
        {
            if (_count == 0) return 0;
            var ret = (_count - _inner) % _count;
            return ret;
        }
        set
        {
            var cnt = _count;
            if (cnt == 0) return;
            _inner = (_inner + cnt - value) % cnt;
        }
    }

    private Vector2 _dragStart;
    private Vector2 _dragEnd;

    private Vector2 BasePos
    {
        get { return transform.position; }
    }

    private float _minAngle = 0.03f;
    private float _radius = 100.0f;
    private float _startAngle;
    private float _endAngle;

    private float GetAngle(Vector2 p1, Vector2 p2)
    {
        return -Mathf.Atan2(p1.y - p2.y, p1.x - p2.x);
    }

    private float DiffAngleDeg
    {
        get
        {
            var diffAngle = (_endAngle - _startAngle) * Mathf.Rad2Deg;
            if (diffAngle >= 180f)
            {
                diffAngle -= 360f;
            }

            if (diffAngle <= -180f)
            {
                diffAngle += 360f;
            }

            return diffAngle;
        }
    }

    private float Angle
    {
        get { return _angle; }
        set
        {
            _angle = value;
            UpdatePos(_angle);
        }
    }

    private bool _isDraging;

    private GameObject SelectedItem
    {
        get
        {
            if (_items == null) return null;
            var index = SelectedIndex;
            if (index < 0 || index >= _count) return null;
            return _items[index];
        }
    }

    private void Awake()
    {
        _canvas = gameObject.GetComponentInParent<Canvas>();
        _radialLayout = GetComponent<RadialLayout>();
        _radius = _radialLayout.Radius;
        _count = _items.Count;
    }

    private void Start()
    {
        UpdatePos(_angle);
    }

    private void OnOnClick(EMenu menu)
    {
        if (OnClick != null)
        {
            OnClick(menu);
        }
    }

    private void Update()
    {
        if (_isDraging) return;
        if (_count <= 1) return;

        var currentAngle = _angle;
        var targetAngle = 360f / _count * _inner;

        if (targetAngle >= 360f) targetAngle -= 360f;
        if (targetAngle < 0f) targetAngle += 360f;

        if (currentAngle == targetAngle || targetAngle - currentAngle == 360f) return;

        if (currentAngle < 90f && targetAngle > 270f)
        {
            currentAngle += 360f;
        }

        var diff = Mathf.Abs(targetAngle - currentAngle);

        if (diff < _minAngle)
        {
            currentAngle = targetAngle;
        }
        else
        {
            if (diff >= 180f)
            {
                targetAngle += 360f;
            }

            currentAngle = targetAngle * _spring + currentAngle * (1f - _spring);
        }

        Angle = currentAngle;
    }

    private void OnEnable()
    {
        foreach (var item in _items)
        {
            if (item == null) continue;
            item.GetComponent<CircleMenuElmnt>().OnClick += OnOnClick;
        }
    }

    private void OnDisable()
    {
        foreach (var item in _items)
        {
            if (item == null) continue;
            item.GetComponent<CircleMenuElmnt>().OnClick -= OnOnClick;
        }
    }

    public void Focus(int index)
    {
        if (index >= _count)
        {
            index %= _count;
        }

        while (index < 0)
        {
            index += _count;
        }

        SelectedIndex = index;

        var angle = 0f;

        if (_count != 0)
        {
            angle = 360f / _count * index;
        }

        Angle = angle;

        if (SelectedItem == null || _items == null) return;

        foreach (var item in _items)
        {
            if (item == null) continue;

            if (item == SelectedItem)
            {
                OnSelect(item);
            }
            else
            {
                OnInSelect(item);
            }
        }
    }

    private void OnSelect(GameObject obj)
    {
        if (obj == null) return;

        obj.transform.localScale = Vector3.one * _selectedScale;
        foreach (var btn in obj.GetComponentsInChildren<Button>(true))
        {
            btn.interactable = true;
            var elmnt = btn.GetComponent<CircleMenuElmnt>();
            elmnt.SetColor(true);
        }
    }

    private void OnInSelect(GameObject obj)
    {
        if (obj == null) return;

        obj.transform.localScale = Vector3.one;
        foreach (var btn in obj.GetComponentsInChildren<Button>(true))
        {
            btn.interactable = false;
            var elmnt = btn.GetComponent<CircleMenuElmnt>();
            elmnt.SetColor(false);
        }
    }

    private void UpdatePos(float diffAngle)
    {
        var selectedItem = SelectedItem;

        for (var i = 0; i < _count; i++)
        {
            if (_items[i] == null) continue;
            if (_count == 0) continue;

            var angle = diffAngle + (360f / _count) * i + _angleOffset;
            if (angle < 0)
            {
                angle += 360f;
            }

            var current = _items[i];

            var vPos = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
            current.transform.localPosition = vPos * _radius;

            if (_items[i] == selectedItem)
            {
                OnSelect(_items[i]);
            }
            else
            {
                OnInSelect(_items[i]);
            }
        }
    }

    #region INTERFACE

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDraging = true;

        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas.GetComponent<RectTransform>(),
            eventData.position,
            _canvas.worldCamera, out worldPoint);

        _dragStart = _dragEnd = worldPoint;
        _startAngle = GetAngle(BasePos, _dragStart);
        _endAngle = GetAngle(BasePos, _dragEnd);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas.GetComponent<RectTransform>(),
            eventData.position,
            _canvas.worldCamera, out worldPoint);

        _dragEnd = worldPoint;
        _startAngle = GetAngle(BasePos, _dragStart);
        _endAngle = GetAngle(BasePos, _dragEnd);
        UpdatePos(Angle + DiffAngleDeg);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDraging = false;

        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas.GetComponent<RectTransform>(),
            eventData.position,
            _canvas.worldCamera, out worldPoint);

        _dragEnd = worldPoint;
        _startAngle = GetAngle(BasePos, _dragStart);
        _endAngle = GetAngle(BasePos, _dragEnd);

        _angle = _angle + DiffAngleDeg;
        while (_angle > 360f)
        {
            _angle -= 360f;
        }

        while (_angle < 0f)
        {
            _angle += 360f;
        }

        UpdatePos(_angle);

        var region = 360f / (_count * 2);
        var indexCnt = (int) (_angle / region);
        var nearIndex = indexCnt / 2 + ((indexCnt % 2) == 0 ? 0 : 1);
        nearIndex = nearIndex % _count;

        _inner = nearIndex;

        if (_startAngle > _endAngle)
        {
            var result = _startAngle - _endAngle;
            if (result > _sensitivity && result < 1.0f)
            {
                _inner--;
            }
        }
        else if (_startAngle < _endAngle)
        {
            var result = _endAngle - _startAngle;
            if (result > _sensitivity && result < 1.0f)
            {
                _inner++;
            }
        }
    }

    #endregion
}