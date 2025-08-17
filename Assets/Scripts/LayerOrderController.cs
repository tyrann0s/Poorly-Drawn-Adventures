using UnityEngine;

[ExecuteAlways] // чтобы работало и в редакторе
public class LayerOrderController : MonoBehaviour
{
    public int baseOrder = 0;
    public int orderStep = 1;
    public bool applyEveryFrame = true;

    void Update()
    {
        if (applyEveryFrame)
            ApplyOrder();
    }

    [ContextMenu("Apply Order Now")]
    public void ApplyOrder()
    {
        int index = 0;
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.sortingOrder = baseOrder + index * orderStep;
            index++;
        }
    }
}
