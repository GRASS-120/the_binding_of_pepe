using UnityEngine;
using Object = UnityEngine.Object;

// base позволяет вызвать конструктор родительского класса. так как PoolBase в конструкторе
// требует передать ей 4 параметра, то значит в дочернем классе должен быть конструктор c base,
// чтобы передать параметры именно родителю - это необходимость просто

public class OldObjectPool : PoolBase<GameObject> {
    public OldObjectPool(GameObject prefab, Transform tearPoolPoint, int preloadCount) : base(
        () => Preload(prefab, tearPoolPoint),
        GetAction,
        ReturnAction,
        preloadCount
    ) {}

    // вне контекста (видимо без наследования от MonoBehavior) Instantiate не может вызываться напрямую,
    // поэтому нужно вызывать его от Object (от Object именно UNITY!!!, нужен импорт отдельный)
    public static GameObject Preload(GameObject prefab, Transform tearPoolPoint) => Object.Instantiate(prefab, tearPoolPoint);

    // так как object уже зарезервированное имя, то добавляется @ что б не спутать
    public static void GetAction(GameObject @object) =>  @object.SetActive(true);

    public static void ReturnAction(GameObject @object) => @object.SetActive(false);
}
