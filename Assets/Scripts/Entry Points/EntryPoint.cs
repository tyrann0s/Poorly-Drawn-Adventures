using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class EntryPoint : MonoBehaviour
{
    private void Start()
    {
        CheckDependencies();
    }
    
    protected async Task InitializeService(IService service)
    {
        try
        {
            await service.InitializeAsync();
            Debug.Log($"Сервис {service.GetType().Name} инициализирован");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка инициализации {service.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    protected abstract void CheckDependencies();
    protected abstract void OnFinish();
}
