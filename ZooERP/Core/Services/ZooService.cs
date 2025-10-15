using ZooERP.Core.Entities;
using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Services;

public class ZooService : IZoo
{
    private readonly List<Animal> _animals = new();
    private readonly List<Thing> _things = new();
    private readonly IVetClinic _veterinaryClinic;
        
    public ZooService(IVetClinic veterinaryClinic)
    {
        _veterinaryClinic = veterinaryClinic;
    }
        
    public void AddAnimal(Animal animal)
    {
        if (_veterinaryClinic.CheckHealth(animal))
        {
            _animals.Add(animal);
            Console.WriteLine($"{animal.Name} успешно принят в зоопарк!");
        }
        else
        {
            Console.WriteLine($"{animal.Name} не принят в зоопарк по состоянию здоровья.");
        }
    }
        
    public void AddThing(Thing thing)
    {
        _things.Add(thing);
        Console.WriteLine($"{thing.Name} успешно добавлен!");
    }
        
    public int GetTotalFoodRequired()
    {
        return _animals.Sum(animal => animal.Food);
    }
        
    public IEnumerable<Animal> GetContactZooAnimals()
    {
        return _animals.Where(animal => animal.CanBeInContactZoo);
    }
        
    public IEnumerable<IInventory> GetAllInventory()
    {
        var inventory = new List<IInventory>();
        inventory.AddRange(_animals);
        inventory.AddRange(_things);
        return inventory;
    }
        
    public int GetAnimalCount()
    {
        return _animals.Count;
    }
        
    public IEnumerable<Animal> GetAllAnimals()
    {
        return _animals;
    }
        
    public IEnumerable<Thing> GetAllThings()
    {
        return _things;
    }
}