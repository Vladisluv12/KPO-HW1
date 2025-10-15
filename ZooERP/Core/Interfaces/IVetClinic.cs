using ZooERP.Core.Entities;

namespace ZooERP.Core.Interfaces;

public interface IVetClinic
{
    bool CheckHealth(Animal animal);
}