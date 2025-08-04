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
            cachedName = gameObject != null ? gameObject.name : "Unknown";
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
            spriteRenderer.enabled = true;
        }

        public void ShowTarget()
        {
            if (!ValidateComponent("ShowTarget")) return;
            spriteRenderer.color = Color.red;
            spriteRenderer.enabled = true;
            ZoomOut();
        }

        public void Hide()
        {
            if (!ValidateComponent("Hide")) return;
            spriteRenderer.enabled = false;
        }

        public void HideTarget()
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.enabled = false;
        }

        public void Activate()
        {
            if (isDestroying) return;
            spriteRenderer.color = Color.green;
            PulseTween();
        }

        public void Deactivate()
        {
            if (isDestroying) return;
            
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
                
            pulseTween = DOTween.Sequence()
                .Append(transform.DOScale(1.1f, .3f))
                .Append(transform.DOScale(1, .3f))
                .SetLoops(-1)
                .SetTarget(transform);
        }

        public void ZoomIn()
        {
            transform.DOScale(1f, .3f).SetTarget(transform);
        }

        public void ZoomOut()
        {
            transform.DOScale(.7f, .3f).SetTarget(transform);
        }

        public void PickTarget()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(0f, .3f))
                .OnComplete(() => {
                    if (!isDestroying && this != null)
                    {
                        Deactivate();
                    }
                })
                .SetTarget(transform);
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