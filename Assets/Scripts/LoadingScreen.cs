using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI Elements")] 
    private CanvasGroup loadingCanvasGroup;
    
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI tipText;
    
    [Header("Settings")]
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float minimumLoadTime = 2f;
    [SerializeField] private string[] loadingTips;
    
    private bool isLoading = false;

    private void Awake()
    {
        loadingCanvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void LoadScene(string sceneName)
    {
        DOTween.KillAll();
        gameObject.SetActive(true);
        if (!isLoading)
        {
            StartCoroutine(LoadSceneWithTransition(sceneName));
        }
    }

    public void LoadBase()
    {
        DOTween.KillAll();
        gameObject.SetActive(true);
        if (!isLoading)
        {
            StartCoroutine(LoadSceneWithTransition("BaseScene"));
        }
    }

    private IEnumerator LoadSceneWithTransition(string sceneName)
    {
        // Чистим Service Locator
        ServiceLocator.Clear();
        
        isLoading = true;
        float startTime = Time.time;
        
        // Показываем случайный совет ДО fade эффекта
        ShowRandomTip();
        
        // Показываем экран загрузки с плавным появлением
        yield return StartCoroutine(FadeIn());
        
        // Начинаем загрузку сцены
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // Обновляем прогресс бар
            if (progressBar != null)
                progressBar.value = progress;
            
            // Обновляем текст И принудительно перерисовываем
            if (loadingText != null)
            {
                loadingText.text = $"Загрузка... {progress * 100:F0}%";
                loadingText.ForceMeshUpdate();
            }
            
            // Проверяем готовность к активации
            if (operation.progress >= 0.9f)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime >= minimumLoadTime)
                {
                    if (loadingText != null)
                    {
                        loadingText.text = "Завершение...";
                        loadingText.ForceMeshUpdate();
                    }
                    
                    yield return new WaitForSeconds(0.5f);
                    operation.allowSceneActivation = true;
                    StartCoroutine(FadeOut());
                }
            }
            
            yield return null;
        }
        
        isLoading = false;
    }
    
    private IEnumerator FadeIn()
    {
        loadingCanvasGroup.alpha = 0f;
        
        while (loadingCanvasGroup.alpha < 1f)
        {
            loadingCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            
            yield return null;
        }
        
        loadingCanvasGroup.alpha = 1f;
    }
    
    private IEnumerator FadeOut()
    {
        while (loadingCanvasGroup.alpha > 0f)
        {
            loadingCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        
        loadingCanvasGroup.alpha = 0f;
    }
    
    private void ShowRandomTip()
    {
        if (tipText != null && loadingTips.Length > 0)
        {
            string randomTip = loadingTips[Random.Range(0, loadingTips.Length)];
            tipText.text = randomTip;
            tipText.ForceMeshUpdate();
        }
    }
}