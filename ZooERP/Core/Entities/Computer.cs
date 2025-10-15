using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Entities;

public class Computer(int number) : Thing(number)
{
    public override string Name => "Computer";
}