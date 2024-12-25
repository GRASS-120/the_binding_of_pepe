using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stat Modifier")]
public class StatModifierSO : SerializedScriptableObject {

    public enum OperationType { Add, Multiply, Subtraction, Division }

    public struct StatModifierParams {
        public StatType type;
        public OperationType operationType;
        public int value;
        public float duration;
    }

    // [TableList] 
    public List<StatModifierParams> StatModifierList;

    // то есть один модификатор может изменять несколько статов (если сделать, чтобы один мод - один стат,
    // то тогда придется создавать много файлов для одного предмета/эффекта)
    public void ApplyModifierEffect(Unit unit) {
        foreach (var item in StatModifierList) {
            StatModifier modifier = item.operationType switch {
                OperationType.Add => new StatModifier(item.type, item.duration, v => v + item.value),
                OperationType.Multiply => new StatModifier(item.type, item.duration, v => v * item.value),
                OperationType.Subtraction => new StatModifier(item.type, item.duration, v => v - item.value),
                OperationType.Division => new StatModifier(item.type, item.duration, v => v / item.value),  //? округлять
                _ => throw new ArgumentOutOfRangeException()
            };

            unit.Stats.Mediator.AddModifier(modifier);
        }
    }
}