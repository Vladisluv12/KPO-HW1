# Московский зоопарк - ERP система

## Технологии
- .NET 8.0
- Spectre.Console (UI) (запускалось в консоли Rider на Windows, если возникнут проблемы с кодировкой)
- Microsoft.Extensions.DependencyInjection + Microsoft.Extensions.Hosting для Dependency Injection из коробки
- xUnit + Moq + FluentAssertions (юнит тесты)

## Архитектура
Предложенная в ТЗ. Классы Animal, Thing - абстрактные, Herbo и Predator - наследники Animal, также абстрактные. Интерфейсы IZoo и IVetClinic были сделаны для инверсии зависимостей и мокирования при тестировании.
ZooService и VetClinicService сделаны синглтонами при конфигурации в DI-контейнере.

## SOLID принципы:

**SRP** - каждый класс имеет одну задачу:
- `ZooService` - управление животными
- `VeterinaryClinic` - проверка здоровья
- `ZooApplication` - UI логика

**OCP** - легко расширить уже существующие классы: животных, наследуя их от абстрактного класса Herbo или Predator, и вещи, наследуя от класса Thing.

**LSP** - наследники заменяют родителей, можно создать Rabbit типа Animal. У наследующихся классов есть все свойства родительских.

**ISP** - интерфейсы реализуют только необходимые методы и свойства.

**DIP** - класс ZooService зависит от абстракции вет клиники, также приложение достаёт нужный сервис по абстракции, благодаря реализации DI-контейнера от Microsoft.

## Тестирование
Было проведено тестирование классов ZooService и Herbo. В них сосредоточена основная логика работы приложения. Покрытие тестами проекта ZooERP составило 66%.

<img width="483" height="247" alt="image" src="https://github.com/user-attachments/assets/4241342b-199f-4c87-b0dc-9171faa2d43f" />

## Как запустить
После скачивания проекта и захода в директорию:
```
dotnet restore
dotnet build
dotnet run --project ZooERP
dotnet test Tests/Tests.csproj
dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage"
```
Отчёт будет находиться в папке ZooERP/TestResults. Папку Application нужно не учитывать в покрытии, поскольку там UI. Она не учитывается в подсчёте покрытия на скрине.
