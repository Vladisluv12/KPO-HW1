using Spectre.Console;
using ZooERP.Core.Entities;
using ZooERP.Core.Interfaces;
using Table = Spectre.Console.Table;

namespace ZooERP.Application;

public class ZooApplication(IZoo zooService)
{
    public void Run()
    {
        AnsiConsole.Clear();
        ShowHeader();
        
        while (true)
        {
            var choice = ShowMainMenu();
            
            switch (choice)
            {
                case "Добавить животное":
                    AddAnimal();
                    break;
                case "Добавить вещь":
                    AddThing();
                    break;
                case "Количество потребляемой еды":
                    ShowTotalFood();
                    break;
                case "Животные для контактного зоопарка":
                    ShowContactZooAnimals();
                    break;
                case "Инвентарь":
                    ShowAllInventory();
                    break;
                case "Всего животных":
                    ShowAnimalCount();
                    break;
                case "Животные":
                    ShowAllAnimals();
                    break;
                case "Выход":
                    ShowExitMessage();
                    return;
            }
            
            if (choice != "Выход")
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Нажмите любую клавишу для продолжения...[/]");
                Console.ReadKey();
                AnsiConsole.Clear();
            }
        }
    }
    
    private static void ShowHeader()
    {
        AnsiConsole.Write(
            new FigletText("Московский Зоопарк")
                .Color(Color.Green));
        
        AnsiConsole.MarkupLine("[bold yellow]=== Система учета животных и инвентаря ===[/]");
        AnsiConsole.WriteLine();
    }
    
    private static string ShowMainMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold cyan]Выберите действие:[/]")
                .PageSize(10)
                .AddChoices(
                    "Добавить животное",
                    "Добавить вещь",
                    "Количество потребляемой еды",
                    "Всего животных",
                    "Животные для контактного зоопарка",
                    "Животные",
                    "Инвентарь",
                    "Выход"
                ));
    }
    
    private void AddAnimal()
    {
        AnsiConsole.Write(new Rule("[bold green]Добавление животного[/]"));
        
        var animalType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Выберите тип животного:[/]")
                .PageSize(5)
                .AddChoices(
                    "Кролик", "Обезьяна", "Тигр", "Волк"
                ));
        
        var food = AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Количество еды (кг/сутки):[/]")
                .ValidationErrorMessage("[red]Введите корректное число[/]")
                .Validate(value => value switch
                {
                    <= 0 => ValidationResult.Error("[red]Количество еды должно быть больше 0[/]"),
                    >= 100 => ValidationResult.Error("[red]Слишком большое количество еды[/]"),
                    _ => ValidationResult.Success()
                }));
        
        var number = AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Инвентарный номер:[/]")
                .ValidationErrorMessage("[red]Введите корректный номер[/]")
                .Validate(value => value switch
                {
                    <= 0 => ValidationResult.Error("[red]Номер должен быть больше 0[/]"),
                    _ => ValidationResult.Success()
                }));
        
        Animal animal = animalType switch
        {
            "Кролик" => new Rabbit(food, number, GetKindnessLevel()),
            "Обезьяна" => new Monkey(food, number, GetKindnessLevel()),
            "Тигр" => new Tiger(food, number),
            "Волк" => new Wolf(food, number),
            _ => throw new ArgumentException("Неверный тип животного")
        };
        
        // Проверка здоровья
        AnsiConsole.Status()
            .Start("Проверка здоровья...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));
                
                Thread.Sleep(1000); // Имитация проверки
            });
        
        zooService.AddAnimal(animal);

        AnsiConsole.MarkupLine(animal.IsHealthy
            ? $"[green] {animal.Name} успешно принят в зоопарк![/]"
            : $"[red] {animal.Name} не принят в зоопарк по состоянию здоровья.[/]");
    }
    
    private static int GetKindnessLevel()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Уровень доброты (0-10):[/]")
                .ValidationErrorMessage("[red]Введите число от 0 до 10[/]")
                .Validate(value => value switch
                {
                    < 0 => ValidationResult.Error("[red]Уровень не может быть меньше 0[/]"),
                    > 10 => ValidationResult.Error("[red]Уровень не может быть больше 10[/]"),
                    _ => ValidationResult.Success()
                }));
    }
    
    private void AddThing()
    {
        AnsiConsole.Write(new Rule("[bold blue]Добавление вещи[/]"));
        
        var thingType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Выберите тип вещи:[/]")
                .PageSize(5)
                .AddChoices("Стол", "Компьютер"));
        
        var number = AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Инвентарный номер:[/]")
                .ValidationErrorMessage("[red]Введите корректный номер[/]")
                .Validate(value => value switch
                {
                    <= 0 => ValidationResult.Error("[red]Номер должен быть больше 0[/]"),
                    _ => ValidationResult.Success()
                }));
        
        Thing thing = thingType switch
        {
            "Стол" => new Core.Entities.Table(number),
            "Компьютер" => new Computer(number),
            _ => throw new ArgumentException("Неверный тип вещи")
        };
        
        zooService.AddThing(thing);
        AnsiConsole.MarkupLine($"[green]{thing.Name} успешно добавлен в инвентарь![/]");
    }
    
    private void ShowTotalFood()
    {
        AnsiConsole.Write(new Rule("[bold yellow]Количество потребляемой еды[/]"));
        
        var totalFood = zooService.GetTotalFoodRequired();
        
        var panel = new Panel($"[bold green]{totalFood} кг/сутки[/]")
            .Header("Статистика")
            .BorderColor(Color.Yellow);
        AnsiConsole.Write(panel);
    }
    
    private void ShowContactZooAnimals()
    {
        AnsiConsole.Write(new Rule("[bold cyan]Животные для контактного зоопарка[/]"));
        
        var contactAnimals = zooService.GetContactZooAnimals().ToList();
        
        if (contactAnimals.Count != 0)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(Color.Cyan1);
            
            table.AddColumn("[bold]Животное[/]");
            table.AddColumn("[bold]Инвентарный номер[/]");
            table.AddColumn("[bold]Еда (кг/день)[/]");
            table.AddColumn("[bold]Уровень доброты[/]");
            
            foreach (var animal in contactAnimals)
            {
                if (animal is Herbo herbivore)
                {
                    table.AddRow(
                        $"[green]{animal.Name}[/]",
                        $"[blue]{animal.Number}[/]",
                        $"[yellow]{animal.Food}[/]",
                        $"[cyan]{herbivore.KindnessLevel}/10[/]"
                    );
                }
            }
            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]Нет животных, подходящих для контактного зоопарка[/]");
        }
    }
    
    private void ShowAllInventory()
    {
        AnsiConsole.Write(new Rule("[bold cyan]Инвентарь[/]"));
        
        var inventory = zooService.GetAllInventory().ToList();
        
        if (inventory.Any())
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(Color.Cyan1);
            
            table.AddColumn("[bold]Тип[/]");
            table.AddColumn("[bold]Наименование[/]");
            table.AddColumn("[bold]Инвентарный номер[/]");
            table.AddColumn("[bold]Доп. информация[/]");
            
            foreach (var item in inventory)
            {
                var type = item is Animal ? "Животное" : "Вещь";
                var additionalInfo = item is Animal animal 
                    ? (animal.CanBeInContactZoo ? "[green]Допущен к контактам[/]" : "[red]Не контактный[/]")
                    : "[grey]—[/]";
                
                table.AddRow(
                    type,
                    $"[purple]{(item is Animal animal1 ? animal1.Name : ((Thing)item).Name)}[/]",
                    $"[blue]{item.Number}[/]",
                    additionalInfo
                );
            }
            
            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]Инвентарь пуст[/]");
        }
    }
    
    private void ShowAnimalCount()
    {
        AnsiConsole.Write(new Rule("[bold green]Всего животных[/]"));
        
        var count = zooService.GetAnimalCount();
        
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        
        grid.AddRow(new Text("Всего животных:", new Style(Color.Purple)), 
            new Text($"{count}", new Style(Color.Green)));
        
        var panel = new Panel(grid)
            .BorderColor(Color.Green)
            .Header("Статистика");
        AnsiConsole.Write(panel);
    }
    
    private void ShowAllAnimals()
    {
        AnsiConsole.Write(new Rule("[bold blue]Животные[/]"));
        
        var animals = zooService.GetAllAnimals().ToList();
        
        if (animals.Count != 0)
        {
            foreach (var animal in animals)
            {
                var panel = new Panel(
                    $"[purple]Инвентарный номер:[/] [blue]{animal.Number}[/]\n" +
                    $"[purple]Еда:[/] [yellow]{animal.Food} кг/сутки[/]\n" +
                    $"[purple]Здоровье:[/] {(animal.IsHealthy ? "[green] Здоров[/]" : "[red] Не здоров[/]")}\n" +
                    $"[purple]Контактный зоопарк:[/] " +
                    $"{(animal.CanBeInContactZoo ? "[green] Можно[/]" : "[red] Нельзя[/]")}" +
                    (animal is Herbo herbivore ? 
                        $"\n[purple]Уровень доброты:[/] [cyan]{herbivore.KindnessLevel}/10[/]" : "")
                )
                .Header($"[bold]{animal.Name}[/]")
                .BorderColor(animal.IsHealthy ? Color.Green : Color.Red);
                
                AnsiConsole.Write(panel);
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow] В зоопарке нет животных[/]");
        }
    }
    
    private static void ShowExitMessage()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[bold green]Завершение работы[/]"));
        AnsiConsole.MarkupLine("[bold green] Спасибо за использование системы учета зоопарка![/]");
        AnsiConsole.MarkupLine("[grey]Нажмите любую клавишу для выхода...[/]");
        Console.ReadKey();
    }
}