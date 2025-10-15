using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Entities;

public class Wolf(int food, int number) : Predator(food, number)
{
    public override string Name => "Wolf";
}