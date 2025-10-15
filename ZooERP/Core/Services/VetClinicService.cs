using ZooERP.Core.Entities;
using ZooERP.Core.Interfaces;

namespace ZooERP.Core.Services;

public class VetClinicService : IVetClinic
{
    private readonly Random _random = new();

    /// <summary>
    /// Имитация проверки здоровья - 80% шанс, что животное здорово
    /// </summary>
    /// <param name="animal">живтоне</param>
    /// <returns>флаг о здоровье животного</returns>
    public bool CheckHealth(Animal animal)
    {
        bool isHealthy = _random.Next(0, 100) < 80;
        animal.IsHealthy = isHealthy;
        return isHealthy;
    }
}