using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Icons Data", menuName = "Settings/Icons Data")]
public class IconData : ScriptableObject
{
    [SerializeField] private Sprite physicalIcon;
    [SerializeField] private Sprite etherialIcon;
    [SerializeField] private Sprite fireIcon;
    [SerializeField] private Sprite waterIcon;
    [SerializeField] private Sprite airIcon;
    [SerializeField] private Sprite earthIcon;

    public Sprite GetIcon(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.None => null,
            ElementType.Physical => physicalIcon,
            ElementType.Etherial => etherialIcon,
            ElementType.Fire => fireIcon,
            ElementType.Water => waterIcon,
            ElementType.Air => airIcon,
            ElementType.Earth => earthIcon,
            _ => throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null)
        };
    }

}
