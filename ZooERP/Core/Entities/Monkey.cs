namespace ZooERP.Core.Entities;

public class Monkey(int food, int number, int kindnessLevel) : Herbo(food, number, kindnessLevel)
{
    public override string Name => "Monkey";
}