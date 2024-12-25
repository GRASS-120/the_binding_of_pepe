using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// как улучшить?
// попробовать переписать на стек. потому что
// 1. на нем работает пул от юнити
// 2. по сути атк правильнее, так как алгоритм берет только первые объекты (так как выбирает по принципу 
// кто первый свободный попался) - он не проходится по всем. а это как раз про стек
// фабрика?
// получить все элементы активные - если понадобиться

public class ObjectPool<T> where T : MonoBehaviour {
    private T _prefab;
    private Transform _container;
    private List<T> _pool;
    private bool _autoExpand;

    public ObjectPool(T prefab, Transform container, int preloadObjectCount) {
        this._prefab = prefab;
        this._container = container; 
        this._autoExpand = true;

        this.CreatePool(preloadObjectCount);
    }

    private void CreatePool(int preloadObjectCount) {
        this._pool = new List<T>();

        for (int i = 0; i < preloadObjectCount; i++) {
            this.CreateObject();
        } 
    }

    private T CreateObject(bool isActiveByDefault = false) {
        var obj = Object.Instantiate(this._prefab, this._container);
        obj.gameObject.SetActive(isActiveByDefault);
        this._pool.Add(obj);

        return obj;
    }

    public bool HasFreeElement(out T element) {
        foreach (var mono in _pool) {
            if (!mono.gameObject.activeInHierarchy) {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }
        element = null;
        return false;
    }

    public T GetFreeElement() {
        if (this.HasFreeElement(out var element)) {
            return element;
        }

        if (this._autoExpand) {
            var obj = CreateObject(true);
            return obj;
        }

        throw new Exception($"there is no free elements in pool of type {typeof(T)}");
    }

    public void Remove(T element) {
        element.gameObject.SetActive(false); 
    }
}
