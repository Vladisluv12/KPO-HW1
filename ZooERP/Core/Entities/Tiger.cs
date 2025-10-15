using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Entities;

public class Tiger(int food, int number) : Predator(food, number)
{
    public override string Name => "Tiger";
    
}