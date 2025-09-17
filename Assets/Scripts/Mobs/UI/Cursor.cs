using DG.Tweening;
using UnityEngine;

namespace Mobs
{
    public class Cursor : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Tween pulseTween;
        private bool isInitialized = false;
        private bool isDestroying = false;
        private string cachedName; // Кешируем имя объекта

        private void Awake()
        {
            // Кешируем имя сразу при создании
            cachedName = gameObject ? gameObject.name : "Unknown";
            InitializeSpriteRenderer();
        }

        private void Start()
        {
            if (!isInitialized)
            {
                InitializeSpriteRenderer();
            }
        }

        private string GetSafeName()
        {
            // Безопасное получение имени объекта
            if (!string.IsNullOrEmpty(cachedName))
                return cachedName;
            
            try
            {
                if (gameObject != null)
                    return gameObject.name;
            }
            catch
            {
                // Игнорируем ошибки при получении имени
            }
            
            return "DestroyedObject";
        }

        private void InitializeSpriteRenderer()
        {
            if (isDestroying) return;
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
                isInitialized = true;
            }
            else
            {
                Debug.LogError($"[Cursor] SpriteRenderer NOT found on {GetSafeName()}!");
            }
        }

        private bool ValidateComponent(string methodName)
        {
            if (isDestroying)
            {
                return false;
            }

            if (this == null)
            {
                return false;
            }

            if (gameObject == null)
            {
                return false;
            }

            if (spriteRenderer == null)
            {
                // Попытаемся найти компонент заново
                try
                {
                    var newRenderer = GetComponent<SpriteRenderer>();
                    if (newRenderer != null)
                    {
                        spriteRenderer = newRenderer;
                    }
                    else
                    {
                        // Проверим все компоненты на объекте
                        var allComponents = GetComponents<Component>();

                        return false;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[Cursor] Exception while trying to find SpriteRenderer: {e.Message}");
                    return false;
                }
            }

            return true;
        }

        public void Show()
        {
            if (!ValidateComponent("Show")) return;
            spriteRenderer.color = Color.green;
            spriteRenderer.enabled = true;
            PulseTween();
        }

        public void ShowTarget()
        {
            if (!ValidateComponent("ShowTarget")) return;
            spriteRenderer.color = Color.red;
            spriteRenderer.enabled = true;
            
            PulseTween();
        }

        public void Hide()
        {
            if (!ValidateComponent("Hide")) return;
            spriteRenderer.enabled = false;
            StopAllTweens();
        }

        public void Deactivate()
        {
            if (isDestroying) return;
            transform.localScale = Vector3.one;
            StopAllTweens();
            Hide();
            if (ValidateComponent("Deactivate"))
            {
                try
                {
                    spriteRenderer.color = Color.white;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[Cursor] Exception in Deactivate(): {e.Message}");
                }
            }
        }

        private void StopAllTweens()
        {
            if (pulseTween != null)
            {
                pulseTween.Kill(true);
                pulseTween = null;
            }
                
            if (transform != null)
            {
                DOTween.Kill(transform, true);
            }
        }

        public void PulseTween()
        {
            StopAllTweens();
            
            Vector3 startPosition = transform.localPosition;
            Vector3 upPosition = startPosition + Vector3.up * 0.15f;   // поднимаем на 0.15 единиц
        
            pulseTween = DOTween.Sequence()
                .Append(transform.DOLocalMoveY(upPosition.y, 0.3f))      // движение вверх// движение вниз
                .Append(transform.DOLocalMoveY(startPosition.y, 0.3f))   // возврат в исходную позицию
                .SetLoops(-1)
                .SetTarget(transform);
        }
        
        public void PickTarget(bool isFinal)
        {
            DOTween.Sequence()
                .Append(transform.DOPunchScale(new Vector3(.7f, .7f, .7f), .3f, 5, .5f))
                .OnComplete(() =>
                {
                    if (!isDestroying && this && isFinal)
                    {
                        Deactivate();
                    }
                });
        }

        private void OnDisable()
        {
            StopAllTweens();
        }

        private void OnDestroy()
        {
            isDestroying = true;
            isInitialized = false;
            
            StopAllTweens();
            spriteRenderer = null;
        }
    }
}