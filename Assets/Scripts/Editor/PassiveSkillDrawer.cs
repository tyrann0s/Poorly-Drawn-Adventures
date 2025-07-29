using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Mobs.Skills;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(PassiveSkill), true)]
public class PassivekillDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Получаем все типы, наследующиеся от PassiveSkill       
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(PassiveSkill).IsAssignableFrom(p) && !p.IsAbstract)
            .ToArray();

        // Создаем массив имен для выпадающего списка
        var typeNames = new string[] { "None" }.Concat(types.Select(t => t.Name)).ToArray();
        
        // Определяем текущий выбранный индекс
        int selectedIndex = 0;
        if (property.managedReferenceValue != null)
        {
            var currentType = property.managedReferenceValue.GetType();
            selectedIndex = Array.IndexOf(types, currentType) + 1;
        }

        // Показываем выпадающий список
        var newIndex = EditorGUI.Popup(position, label.text, selectedIndex, typeNames);
        
        // Если выбор изменился
        if (newIndex != selectedIndex)
        {
            if (newIndex == 0)
            {
                property.managedReferenceValue = null;
            }
            else
            {
                var selectedType = types[newIndex - 1];
                property.managedReferenceValue = Activator.CreateInstance(selectedType);
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif