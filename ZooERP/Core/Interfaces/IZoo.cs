using ZooERP.Core.Entities;

namespace ZooERP.Core.Interfaces;

public interface IZoo
{
    void AddAnimal(Animal animal);
    void AddThing(Thing thing);
    int GetTotalFoodRequired();
    IEnumerable<Animal> GetContactZooAnimals();
    IEnumerable<IInventory> GetAllInventory();
    int GetAnimalCount();
    IEnumerable<Animal> GetAllAnimals();
    IEnumerable<Thing> GetAllThings();
}