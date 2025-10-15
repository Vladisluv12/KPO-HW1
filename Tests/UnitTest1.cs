using FluentAssertions;
using Moq;
using ZooERP.Core.Entities;
using ZooERP.Core.Interfaces;
using ZooERP.Core.Services;
using Xunit;

namespace Tests;

public class ZooServiceTests
{
    private readonly Mock<IVetClinic> _veterinaryClinicMock;
    private readonly IZoo _zooService;
    
    public ZooServiceTests()
    {
        _veterinaryClinicMock = new Mock<IVetClinic>();
        _zooService = new ZooService(_veterinaryClinicMock.Object);
    }
    
    [Fact]
    public void AddAnimal_HealthyAnimal_ShouldAddToZoo()
    {
        // Arrange
        var animalMock = new Rabbit(2, 3, 6);
        _veterinaryClinicMock.Setup(v => v.CheckHealth(animalMock)).Returns(true);
        
        // Act
        _zooService.AddAnimal(animalMock);
        
        // Assert
        _zooService.GetAnimalCount().Should().Be(1);
        _veterinaryClinicMock.Verify(v => v.CheckHealth(animalMock), Times.Once);
    }
    
    [Fact]
    public void AddAnimal_UnhealthyAnimal_ShouldNotAddToZoo()
    {
        // Arrange
        var animalMock = new Wolf(2, 3);
        _veterinaryClinicMock.Setup(v => v.CheckHealth(animalMock)).Returns(false);
        
        // Act
        _zooService.AddAnimal(animalMock);
        
        // Assert
        _zooService.GetAnimalCount().Should().Be(0);
    }
    
    [Fact]
    public void GetTotalFoodRequired_WithMultipleAnimals_ShouldReturnCorrectSum()
    {
        // Arrange
        var animal1Mock = new Monkey(5, 2, 6);
        var animal2Mock = new Wolf(3, 2);
        
        
        _veterinaryClinicMock.Setup(v => v.CheckHealth(It.IsAny<Animal>())).Returns(true);
        
        // Act
        _zooService.AddAnimal(animal1Mock);
        _zooService.AddAnimal(animal2Mock);
        
        // Assert
        _zooService.GetTotalFoodRequired().Should().Be(8);
    }
    
    [Fact]
    public void GetContactZooAnimals_ShouldReturnOnlyEligibleAnimals()
    {
        // Arrange
        var contactAnimal = new Rabbit(1, 2, 6);
        var nonContactAnimal = new Rabbit(1, 2, 4);
        
        _veterinaryClinicMock.Setup(v => v.CheckHealth(It.IsAny<Animal>())).Returns(true);
        
        // Act
        _zooService.AddAnimal(contactAnimal);
        _zooService.AddAnimal(nonContactAnimal);
        
        var contactAnimals = _zooService.GetContactZooAnimals().ToList();
        
        // Assert
        contactAnimals.Should().HaveCount(1);
    }
    
    [Fact]
    public void GetAllInventory_ShouldReturnAnimalsAndThings()
    {
        // Arrange
        var animal = new Wolf(1, 1);
        var thing = new Computer(2);
        
        _veterinaryClinicMock.Setup(v => v.CheckHealth(animal)).Returns(true);
        
        // Act
        _zooService.AddAnimal(animal);
        _zooService.AddThing(thing);
        
        var inventory = _zooService.GetAllInventory().ToList();
        
        // Assert
        inventory.Should().HaveCount(2);
    }
}

public class HerboTests
{
    [Fact]
    public void Herbo_WithKindnessAbove5_ShouldBeContactZooEligible()
    {
        // Arrange
        var herbo = new Rabbit(1, 2, 7);
        
        // Act & Assert
        herbo.CanBeInContactZoo.Should().BeTrue();
    }
    
    [Fact]
    public void Herbo_WithKindnessBelow5_ShouldNotBeContactZooEligible()
    {
        // Arrange
        var herbo = new Monkey(1, 2, 3);
        
        // Act & Assert
        herbo.CanBeInContactZoo.Should().BeFalse();
    }
}
