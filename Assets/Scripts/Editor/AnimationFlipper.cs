using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

public class AnimationFlipper : EditorWindow
{
    [MenuItem("Tools/Animation Clip Inverter")]
    public static void ShowWindow()
    {
        GetWindow<AnimationFlipper>("Animation Clip Inverter");
    }

    private AnimationClip clip;
    private Vector2 scrollPosition;
    
    // Опции для инверсии
    private bool invertX = true;
    private bool invertY = false;
    private bool invertZ = false;
    
    // Опции для типов позиций
    private bool affectLocalPosition = true;
    private bool affectWorldPosition = false;
    
    // Предварительный просмотр
    private bool showPreview = false;
    private List<string> affectedPaths = new List<string>();

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.LabelField("Animation Clip Inverter", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // Выбор клипа
        EditorGUILayout.LabelField("Animation Clip:", EditorStyles.label);
        AnimationClip newClip = (AnimationClip)EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false);
        
        if (newClip != clip)
        {
            clip = newClip;
            UpdatePreview();
        }
        
        if (clip == null)
        {
            EditorGUILayout.HelpBox("Выберите Animation Clip для продолжения", MessageType.Info);
            EditorGUILayout.EndScrollView();
            return;
        }
        
        EditorGUILayout.Space();
        
        // Опции инверсии осей
        EditorGUILayout.LabelField("Инвертировать оси:", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        
        bool newInvertX = EditorGUILayout.Toggle("Position X", invertX);
        bool newInvertY = EditorGUILayout.Toggle("Position Y", invertY);
        bool newInvertZ = EditorGUILayout.Toggle("Position Z", invertZ);
        
        if (newInvertX != invertX || newInvertY != invertY || newInvertZ != invertZ)
        {
            invertX = newInvertX;
            invertY = newInvertY;
            invertZ = newInvertZ;
            UpdatePreview();
        }
        
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
        
        // Опции типов позиций
        EditorGUILayout.LabelField("Типы позиций:", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        
        bool newAffectLocal = EditorGUILayout.Toggle("Local Position", affectLocalPosition);
        bool newAffectWorld = EditorGUILayout.Toggle("World Position", affectWorldPosition);
        
        if (newAffectLocal != affectLocalPosition || newAffectWorld != affectWorldPosition)
        {
            affectLocalPosition = newAffectLocal;
            affectWorldPosition = newAffectWorld;
            UpdatePreview();
        }
        
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
        
        // Проверяем, выбрана ли хотя бы одна ось и один тип позиции
        bool hasAxisSelected = invertX || invertY || invertZ;
        bool hasPositionTypeSelected = affectLocalPosition || affectWorldPosition;
        
        if (!hasAxisSelected)
        {
            EditorGUILayout.HelpBox("Выберите хотя бы одну ось для инверсии", MessageType.Warning);
        }
        
        if (!hasPositionTypeSelected)
        {
            EditorGUILayout.HelpBox("Выберите хотя бы один тип позиции", MessageType.Warning);
        }
        
        // Предварительный просмотр
        showPreview = EditorGUILayout.Foldout(showPreview, "Предварительный просмотр изменений");
        if (showPreview && affectedPaths.Count > 0)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField($"Будет изменено кривых: {affectedPaths.Count}", EditorStyles.miniLabel);
            
            foreach (string path in affectedPaths)
            {
                EditorGUILayout.LabelField("• " + path, EditorStyles.miniLabel);
            }
            EditorGUI.indentLevel--;
        }
        else if (showPreview)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Нет кривых для изменения", EditorStyles.miniLabel);
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.Space();
        
        // Кнопки действий
        EditorGUI.BeginDisabledGroup(!hasAxisSelected || !hasPositionTypeSelected);
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Применить инверсию", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Подтверждение", 
                $"Применить инверсию к клипу '{clip.name}'?\n\nЭто действие нельзя отменить через Undo.", 
                "Применить", "Отмена"))
            {
                ApplyInversion();
            }
        }
        
        if (GUILayout.Button("Создать копию", GUILayout.Height(30)))
        {
            CreateInvertedCopy();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.Space();
        
        // Дополнительная информация
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Информация:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"• Длительность: {clip.length:F2}s", EditorStyles.miniLabel);
        EditorGUILayout.LabelField($"Частота кадров: {clip.frameRate}fps", EditorStyles.miniLabel);
        EditorGUILayout.LabelField($"• Зациклена: {(clip.isLooping ? "Да" : "Нет")}", EditorStyles.miniLabel);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndScrollView();
    }
    
    private void UpdatePreview()
    {
        affectedPaths.Clear();
        
        if (clip == null) return;
        
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
        
        foreach (var binding in bindings)
        {
            bool shouldAffect = false;
            string axisName = "";
            
            // Проверяем локальную позицию
            if (affectLocalPosition)
            {
                if (invertX && binding.propertyName == "m_LocalPosition.x")
                {
                    shouldAffect = true;
                    axisName = "Local Position X";
                }
                else if (invertY && binding.propertyName == "m_LocalPosition.y")
                {
                    shouldAffect = true;
                    axisName = "Local Position Y";
                }
                else if (invertZ && binding.propertyName == "m_LocalPosition.z")
                {
                    shouldAffect = true;
                    axisName = "Local Position Z";
                }
            }
            
            // Проверяем мировую позицию
            if (affectWorldPosition)
            {
                if (invertX && binding.propertyName == "RootT.x")
                {
                    shouldAffect = true;
                    axisName = "Root Position X";
                }
                else if (invertY && binding.propertyName == "RootT.y")
                {
                    shouldAffect = true;
                    axisName = "Root Position Y";
                }
                else if (invertZ && binding.propertyName == "RootT.z")
                {
                    shouldAffect = true;
                    axisName = "Root Position Z";
                }
            }
            
            if (shouldAffect)
            {
                string pathInfo = string.IsNullOrEmpty(binding.path) ? 
                    $"{axisName}" : 
                    $"{binding.path} - {axisName}";
                affectedPaths.Add(pathInfo);
            }
        }
    }
    
    private void ApplyInversion()
    {
        if (clip == null) return;
        
        // Создаём Undo операцию
        Undo.RegisterCompleteObjectUndo(clip, "Invert Animation Position");
        
        int modifiedCurves = InvertPositionInClip(clip);
        
        if (modifiedCurves > 0)
        {
            // Помечаем ассет как грязный и сохраняем
            EditorUtility.SetDirty(clip);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Инверсия применена к {modifiedCurves} кривым в клипе '{clip.name}'");
            ShowNotification(new GUIContent($"Применено к {modifiedCurves} кривым"));
        }
        else
        {
            Debug.LogWarning($"Не найдено кривых для инверсии в клипе '{clip.name}'");
            ShowNotification(new GUIContent("Кривые не найдены"));
        }
    }
    
    private void CreateInvertedCopy()
    {
        if (clip == null) return;
        
        // Создаём копию клипа
        string assetPath = AssetDatabase.GetAssetPath(clip);
        string directory = System.IO.Path.GetDirectoryName(assetPath);
        string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
        string extension = System.IO.Path.GetExtension(assetPath);
        
        string newPath = $"{directory}/{fileName}_Inverted{extension}";
        newPath = AssetDatabase.GenerateUniqueAssetPath(newPath);
        
        if (AssetDatabase.CopyAsset(assetPath, newPath))
        {
            AssetDatabase.Refresh();
            
            // Загружаем новый клип и применяем инверсию
            AnimationClip newClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(newPath);
            if (newClip != null)
            {
                int modifiedCurves = InvertPositionInClip(newClip);
                
                EditorUtility.SetDirty(newClip);
                AssetDatabase.SaveAssets();
                
                Debug.Log($"Создана инвертированная копия '{newClip.name}' с {modifiedCurves} изменёнными кривыми");
                ShowNotification(new GUIContent("Копия создана"));
                
                // Выделяем новый клип в Project
                Selection.activeObject = newClip;
                EditorGUIUtility.PingObject(newClip);
            }
        }
        else
        {
            Debug.LogError("Не удалось создать копию клипа");
            ShowNotification(new GUIContent("Ошибка создания копии"));
        }
    }
    
    private int InvertPositionInClip(AnimationClip targetClip)
    {
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(targetClip);
        int modifiedCount = 0;
        
        foreach (var binding in bindings)
        {
            bool shouldInvert = false;
            
            // Проверяем локальную позицию
            if (affectLocalPosition)
            {
                if ((invertX && binding.propertyName == "m_LocalPosition.x") ||
                    (invertY && binding.propertyName == "m_LocalPosition.y") ||
                    (invertZ && binding.propertyName == "m_LocalPosition.z"))
                {
                    shouldInvert = true;
                }
            }
            
            // Проверяем мировую позицию (Root Transform)
            if (affectWorldPosition)
            {
                if ((invertX && binding.propertyName == "RootT.x") ||
                    (invertY && binding.propertyName == "RootT.y") ||
                    (invertZ && binding.propertyName == "RootT.z"))
                {
                    shouldInvert = true;
                }
            }
            
            if (shouldInvert)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(targetClip, binding);
                if (curve != null)
                {
                    // Инвертируем значения кривой
                    Keyframe[] keys = curve.keys;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        keys[i].value = -keys[i].value;
                        keys[i].inTangent = -keys[i].inTangent;
                        keys[i].outTangent = -keys[i].outTangent;
                    }
                    curve.keys = keys;
                    
                    // Применяем изменённую кривую
                    AnimationUtility.SetEditorCurve(targetClip, binding, curve);
                    modifiedCount++;
                }
            }
        }
        
        return modifiedCount;
    }
}