using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Entities;

/// <summary>
/// Абстрактный класс для травоядных
/// </summary>
public abstract class Herbo : Animal
{
    /// <summary>
    /// Уровень доброты
    /// </summary>
    public int KindnessLevel { get; }

    protected Herbo(int food, int number, int kindnessLevel) : base(food, number)
    {
        CanBeInContactZoo = kindnessLevel > 5; // необходимая доброта, чтобы можно было попасть в контактный зоопарк
        KindnessLevel = kindnessLevel;
    }

}