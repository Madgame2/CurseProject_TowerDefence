using Newtonsoft.Json;

namespace Scenes.SessionRework.Scripts.GameWorld.Graph.Common.Meta.Enums
{
    public enum ConditionType
    {
        Compare,   // Сравнение со значением (A > B)
        Range,     // Проверка диапазонов (Min <= A <= Max)
        Expression // Если в будущем появится текстовое выражение
    }
}