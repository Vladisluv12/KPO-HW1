namespace ZooERP.Core.Entities;

public class Rabbit(int food, int number, int kindnessLevel) : Herbo(food, number, kindnessLevel)
{
    public override string Name => "Rabbit";
}