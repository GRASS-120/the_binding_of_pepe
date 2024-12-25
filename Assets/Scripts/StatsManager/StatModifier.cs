using System;
using System.Collections;

public class StatModifier : IDisposable {
    public event Action<StatModifier> OnDispose = delegate { }; // ?????
    
    readonly CountdownTimer timer;
    readonly StatType type;
    readonly Func<float, float> operation;
    public bool markedForRemoval { get; private set; }

    public StatModifier(StatType type, float duration, Func<float, float> operation) {
        this.type = type;
        this.operation = operation;

        // если duration <= 0, значит модификатор постоянный
        if (duration <= 0) return;

        // ? переписать на корутины?
        timer = new CountdownTimer(duration);
        timer.OnTimerStop += () => markedForRemoval = true;  // когда таймер кончается, вызывается метод dispose для удаления модификатора
        timer.Start();
    }

    public void Update(float deltaTime) => timer?.Tick(deltaTime);

    public void Handle(object sender, Query query) {
        if (query.statType == type) {
            query.value = operation(query.value);
        }
    }

    // удаление модификатора из списка когда истечет таймер
    public void Dispose() {
        OnDispose.Invoke(this);
    }
}