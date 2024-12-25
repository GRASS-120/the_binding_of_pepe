using System;
using System.Collections.Generic;
using UnityEngine;

// кароче, проблема с пулями из-за пула видимо. когда пули беруться последовательно, то все норм. но в какой-то момент
// один и тот же элемент начинает браться много раз и возникает баг этот. так что нужно другой видос чекнуть и 
// объединить их (+ обязательно переписать на лист думаю)

public class PoolBase<T> {        
    private Func<T> _preloadFunc;
    private Action<T> _getAction;
    private Action<T> _returnAction;
    private Queue<T> _pool = new Queue<T>();
    private List<T> _active = new List<T>();

    public PoolBase(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount) {
        this._preloadFunc = preloadFunc; // preloadFunc - создание новых элементов
        this._getAction = getAction; // getAction - функция активации
        this._returnAction = returnAction; // returnAction - функция деактивации

        if (preloadFunc == null) {
            Debug.LogError("Preload func is null!");
            return; 
        }

        // предзагрузка объектов в конструкторе
        for (int i = 0; i < preloadCount; i++) {
            Return(preloadFunc()); 
        }
    }

    // выдает из пула
    public T Get() {
        T item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
        _getAction(item);
        _active.Add(item);

        return item;
    }

    // возвращает в пул
    public void Return(T item) {
        _returnAction(item);
        _pool.Enqueue(item);
        _active.Remove(item);
    }

    // возвращает все объекты, взятые из пула, обратно
    public void ReturnAll() {
        foreach (T item in _active.ToArray()) {
            Return(item);
        }
    }
}
