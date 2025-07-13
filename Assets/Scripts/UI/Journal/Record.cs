using UnityEngine;

public abstract class Record : MonoBehaviour
{
    public abstract void Initialize<T>(T data);
}
