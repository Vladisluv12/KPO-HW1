using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Entities;

public abstract class Thing(int number) : IInventory
{
    public int Number { get; set; } = number;
    public abstract string Name { get; }
}