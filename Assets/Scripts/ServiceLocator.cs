using System.Collections.Generic;
using System;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new();

    public static void Register<T>(T service) where T : class
    {
        var type = typeof(T);
        if (services.ContainsKey(type))
        {
            throw new Exception($"Сервис {type} уже зарегистрирован.");
        }
        services[type] = service;
    }

    public static T Get<T>() where T : class
    {
        var type = typeof(T);
        if (services.TryGetValue(type, out var service))
        {
            return service as T;
        }
        throw new Exception($"Сервис {type} не найден.");
    }

    public static void Clear()
    {
        services.Clear();
    }
}
