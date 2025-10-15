using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Entities;

public abstract class Animal : IAlive, IInventory
{

    public abstract string Name { get; }
    public int Food { get; set; }
    public int Number { get; set; }
    public bool IsHealthy { get; set; }
    public bool CanBeInContactZoo { get; protected init; }
    protected Animal(int food, int number)
    {
        Food = food;
        Number = number;
    }
}