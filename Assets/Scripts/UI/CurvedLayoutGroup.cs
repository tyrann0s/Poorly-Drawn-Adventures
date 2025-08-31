using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Раскладывает дочерние RectTransform по дуге.
/// Устанавливайте на родительский объект вместо Horizontal/Vertical Layout Group.
/// </summary>
[AddComponentMenu("UI/Curved Layout Group")]
public class CurvedLayoutGroup : LayoutGroup
{
    [Header("Arc")]
    [Tooltip("Радиус дуги в пикселях от центра контейнера до детей.")]
    public float radius = 200f;

    [Tooltip("Начальный угол дуги (в градусах, 0 = вправо, 90 = вверх).")]
    public float startAngle = -30f;

    [Tooltip("Общий угол дуги (в градусах). Положительное значение — по часовой стрелке.")]
    public float arcAngle = 60f;

    [Tooltip("Дополнительный угол между соседними элементами поверх автоматического шага.")]
    public float extraAnglePerItem = 0f;

    [Header("Children")]
    [Tooltip("Поворачивать ли детей так, чтобы они касались дуги (по касательной).")]
    public bool orientAlongTangent = false;

    [Tooltip("Отступ между элементами вдоль дуги в градусах (добавляется к автоматике при вычислении шага).")]
    public float spacingDegrees = 0f;

    [Tooltip("Игнорировать неактивные объекты при раскладке.")]
    public bool ignoreInactive = true;

    // Чтобы Unity показывал «Driven by…» и не сохранял управляемые значения
    private DrivenRectTransformTracker _tracker;

    // --- LayoutGroup overrides ---

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        // Ширина/высота контроллера тут обычно не вычисляется для кривых раскладок,
        // оставим preferred равными нулю и дадим контейнеру жить своей жизнью.
    }

    public override void CalculateLayoutInputVertical() { }

    public override void SetLayoutHorizontal()
    {
        DoLayout();
    }

    public override void SetLayoutVertical()
    {
        // Вторая фаза верстки — повторяем раскладку для совместимости с порядком вызовов системы лэйаута.
        DoLayout();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetDirty();
    }

    protected override void OnDisable()
    {
        _tracker.Clear();
        base.OnDisable();
    }

    protected override void OnTransformChildrenChanged()
    {
        base.OnTransformChildrenChanged();
        SetDirty();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        SetDirty();
    }

    private void DoLayout()
    {
        _tracker.Clear();

        var children = GetChildren();
        int count = children.Count;
        if (count == 0) return;

        // Корректируем дугу под количество элементов:
        // если элементов 1 — ставим в середину дуги; иначе распределяем равномерно.
        float usedArc = arcAngle;
        float step = (count > 1)
            ? (usedArc - spacingDegrees * (count - 1)) / (count - 1)
            : 0f;

        // Опорная точка — центр нашего контейнера (с учётом padding и alignment).
        Vector2 center = GetAlignmentPivot(); // в локальных координатах нашего RectTransform

        for (int i = 0; i < count; i++)
        {
            var child = children[i];
            if (!child) continue;

            // Устанавливаем, какие свойства мы «драйвим»
            _tracker.Add(this, child, DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Rotation);

            float angle = startAngle + i * (step + spacingDegrees) + extraAnglePerItem * i;
            float rad = angle * Mathf.Deg2Rad;

            // Позиция на окружности
            Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            child.anchorMin = child.anchorMax = new Vector2(0.5f, 0.5f); // проще управлять anchoredPosition
            child.pivot = child.pivot; // не трогаем — пусть останется как есть
            child.anchoredPosition = center + offset;

            if (orientAlongTangent)
            {
                // Тангенс к окружности = угол + 90°
                float t = angle + 90f;
                child.localRotation = Quaternion.Euler(0f, 0f, t);
            }
            else
            {
                child.localRotation = Quaternion.identity;
            }
        }
    }

    // Собираем подходящих детей по тем же правилам, что и у штатных LayoutGroup
    private List<RectTransform> GetChildren()
    {
        var list = new List<RectTransform>();
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            var t = rectTransform.GetChild(i) as RectTransform;
            if (!t) continue;
            if (ignoreInactive && !t.gameObject.activeInHierarchy) continue;
            if (!t.gameObject.activeSelf && ignoreInactive) continue;
            list.Add(t);
        }
        return list;
    }

    // Вычисление базовой точки с учетом padding + alignment (по аналогии с LayoutGroup.GetStartOffset)
    private Vector2 GetAlignmentPivot()
    {
        // Размер нашей панели
        var size = rectTransform.rect.size;

        // Горизонталь
        float x = padding.left - padding.right;
        float w = size.x - padding.horizontal;
        float ax = 0.5f;
        switch (childAlignment)
        {
            case TextAnchor.UpperLeft:
            case TextAnchor.MiddleLeft:
            case TextAnchor.LowerLeft: ax = 0f; break;
            case TextAnchor.UpperCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.LowerCenter: ax = 0.5f; break;
            case TextAnchor.UpperRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.LowerRight: ax = 1f; break;
        }
        float cx = Mathf.Lerp(-w * 0.5f, w * 0.5f, ax);

        // Вертикаль (в UI вверх — положительный Y)
        float ay = 0.5f;
        switch (childAlignment)
        {
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight: ay = 1f; break;
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight: ay = 0.5f; break;
            case TextAnchor.LowerLeft:
            case TextAnchor.LowerCenter:
            case TextAnchor.LowerRight: ay = 0f; break;
        }
        float h = size.y - padding.vertical;
        float cy = Mathf.Lerp(-h * 0.5f, h * 0.5f, ay);

        return new Vector2(cx, cy);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetDirty();
    }
#endif

    protected void SetDirty()
    {
        if (!IsActive()) return;
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }
}
