using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Entities;
/// <summary>
/// Абстрактный класс для хищников, им по умолчанию нельзя идти в контактный зоопарк
/// </summary>
public abstract class Predator(int food, int number) : Animal(food, number)
{
}