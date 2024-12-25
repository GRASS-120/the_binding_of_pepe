using System;
using System.Collections.Generic;

// ! как у меня работает mediator? что с чем он связывает? какие еще связи можно в него добавить?

// паттерн  mediator (посредник)
// ? как я понимаю, связывает статы и модификаторы. то есть если есть доступ к статам, то есть и к медиатору => ко всему,
// ? что связано со статами

// по сути медиатор - это хаб, который обеспечивает доступ к набору из нескольких элементов + в нем храниться логика по
// взаимоедйствию с этими элементами. (все равно до конца пока не понимаю......)

// паттерн: chain of responsibilities
// можно переделать на другую структуру данных (думаю на словарь) или только SO
// просто линкед лист - клаасика (нужно чтобы можно было итерироваться)

// глава цепи (client)
public class StatsMediator {
    public event EventHandler<Query> Queries;

    // !!!! событие чтобы знать о любом изменении статов ==> ??? а если на тебя дебаффы накладывают? потом перепишу
    public event EventHandler OnStatsChange;
    // событие чтобы в Unit можно было узнать когда таймер иссяк для того чтобы вызвать updateCalculatedStats()
    public event EventHandler OnDisposeModifier; 
    // аналогичное назначение, только когда модификатор появляется (чтобы не прописывать везде)
    public event EventHandler OnAddModifier; 

    // причем круто, что отслеживается именно модификатор. благодаря этому по идее эти события должны
    // также работать и на шмотках, и на баффах, и на рассходниках и тп. и причем не только у игрока,
    // но и у мобов. гениально! хорошая архитектура окупается!

    readonly LinkedList<StatModifier> modifiers = new(); // связь 

    public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

    public void AddModifier(StatModifier modifier) {
        modifiers.AddLast(modifier);
        Queries += modifier.Handle;

        OnStatsChange?.Invoke(this, EventArgs.Empty);
        OnAddModifier?.Invoke(this, EventArgs.Empty);

        modifier.OnDispose += _ => {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
        };
    }

    public void Update(float deltaTime) {
        //  обновляем таймеры у всех модификаций с ограничением по времени
        var node = modifiers.First;
        while (node != null) {
            var modifier = node.Value;
            modifier.Update(deltaTime);
            node = node.Next;
        }

        // если таймер иссяк (markedForRemoval = true), то удаляем
        node = modifiers.First;
        while (node != null) {
            var nextNode = node.Next;

            if (node.Value.markedForRemoval) {
                node.Value.Dispose();
                OnStatsChange?.Invoke(this, EventArgs.Empty);
                OnDisposeModifier?.Invoke(this, EventArgs.Empty);
            }

            node = nextNode;
        }
    }
}

// query contains a value that gets mutated by everything in the chain
// as it goes through each of them ====>>> like a middleware???????

// объект-запрос
public class Query {
    public readonly StatType statType;
    public float value;

    public Query(StatType statType, float value) {
        this.statType = statType;
        this.value = value;
    }
}