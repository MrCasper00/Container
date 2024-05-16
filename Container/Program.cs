using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Web;
using Container;

Main();

void Main()
{
begin:

    Console.WriteLine("Welcome to the container loading program");
    Console.WriteLine("Do you want to choose everything yourself?");
    Console.WriteLine("0. Yes");
    Console.WriteLine("1. No");
    Console.WriteLine("2. predetermined");

    int choice = int.Parse(Console.ReadLine());
    if (choice < 0 || choice > 2)
    {
        Console.WriteLine("Invalid input");
        goto begin;
    }
    if (choice == 0)
    {

    }
    else if (choice == 2)
    {
        Predetermined();
    }
    else
    {
        Random();
    }


Manual:

    Console.WriteLine("Please enter the width of the ship");
    int width = int.Parse(Console.ReadLine());
    Console.WriteLine("Please enter the length of the ship");
    int length = int.Parse(Console.ReadLine());


    Captain captain = new Captain(width, length);

    Console.WriteLine("How many containers do you want to add?");
    int numberOfContainers = int.Parse(Console.ReadLine());

    if (numberOfContainers < 1)
    {
        Console.WriteLine("Invalid input");
        return;
    }

    List<Container.Container> containers = new List<Container.Container>();
    for (int i = 0; i < numberOfContainers; i++)
    {
        Container.Container container = CreateContainer();
        containers.Add(container);
    }

    int weight = 0;
    foreach (var container in containers)
    {
        weight += container.Weight;
    }


    try
    {
        captain.AddContainers(containers);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        goto Manual;
    }
    captain.DetermineBest();

    if (captain.LeftOverContainers.Count > 0)
    {
        Console.WriteLine("The ship is full and the following containers are left over:");
        foreach (var container in captain.LeftOverContainers)
        {
            Console.WriteLine($"Weight: {container.Weight} Type: {container.ContainerType}");
        }
    }
    else
    {
        Console.WriteLine("The ship was succesfully loaded");
        UrlBuilder(captain.Ship);
    }

}


void Predetermined()
{
    Captain captain = new Captain(3, 3);

    List<Container.Container> containers = new List<Container.Container>();
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(30, ContainerType.ValuableCooled));
    containers.Add(new Container.Container(30, ContainerType.ValuableCooled));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(30, ContainerType.Cooled));
    containers.Add(new Container.Container(30, ContainerType.Valueable));
    containers.Add(new Container.Container(30, ContainerType.Valueable));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(30, ContainerType.Cooled));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(4, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(16, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(27, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(24, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(30, ContainerType.Cooled));
    containers.Add(new Container.Container(30, ContainerType.Cooled));
    containers.Add(new Container.Container(30, ContainerType.Cooled));
    containers.Add(new Container.Container(30, ContainerType.Cooled));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(30, ContainerType.Cooled));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));
    containers.Add(new Container.Container(20, ContainerType.Ordinary));

    captain.AddContainers(containers);


    var result = captain.DetermineBest();
    Console.WriteLine(result.WeightLeft + result.WeightRight);
    UrlBuilder(captain.Ship);
}

void Random()
{
Random:
    Random random = new Random();
    int width = random.Next(1, 10);
    int length = random.Next(1, 10);

    Captain captain = new Captain(width, length);

    int numberOfContainers = random.Next(80, 80);

    List<Container.Container> containers = new List<Container.Container>();

    for (int i = 0; i < numberOfContainers; i++)
    {
        Container.Container container = new Container.Container(random.Next(30, 30), (ContainerType)random.Next(1, 1));
        containers.Add(container);
    }

    try
    {
        captain.AddContainers(containers);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        goto Random;
    }


    captain.DetermineBest();
    UrlBuilder(captain.Ship);
}

void UrlBuilder(Ship ship)
{
    string baseUrl = "https://i872272.luna.fhict.nl/ContainerVisualizer/index.html";

    string stacks = "";
    string weights = "";

    for (int j = ship.Cargo.GetLength(1) - 1; j >= 0; j--)
    {
        string rowStacks = "";
        string rowWeights = "";

        for (int i = ship.Cargo.GetLength(0) - 1; i >= 0; i--)
        {
            var containerStack = ship.Cargo[i, j];
            string stackTypes = "";
            string stackWeights = "";

            for (int k = 0; k < containerStack.Containers.Count; k++)
            {
                stackTypes += ((int)containerStack.Containers[k].ContainerType).ToString();
                stackWeights += containerStack.Containers[k].Weight.ToString();

                if (k < containerStack.Containers.Count - 1)
                {
                    stackTypes += "-";
                    stackWeights += "-";
                }
            }

            rowStacks += (ship.Cargo.GetLength(0) - 1 - i > 0 ? "," : "") + (string.IsNullOrEmpty(stackTypes) ? "" : stackTypes);
            rowWeights += (ship.Cargo.GetLength(0) - 1 - i > 0 ? "," : "") + (string.IsNullOrEmpty(stackWeights) ? "" : stackWeights);
        }

        stacks += (j < ship.Cargo.GetLength(1) - 1 ? "/" : "") + rowStacks;
        weights += (j < ship.Cargo.GetLength(1) - 1 ? "/" : "") + rowWeights;
    }

    string url = $"{baseUrl}?length={ship.LengthInContainers}&width={ship.WidthInContainers}&stacks={stacks}&weights={weights}";

    Console.WriteLine(url);
}

Container.Container CreateContainer()
{
    Container.Container container = new Container.Container(20, ContainerType.Ordinary);

AddContainers:
    Console.WriteLine("What kind of cargo is it?");
    Console.WriteLine("4. Valuable and Cooled");
    Console.WriteLine("2. Valuable");
    Console.WriteLine("3. Cooled");
    Console.WriteLine("1. Ordinary");

    int containerType = int.Parse(Console.ReadLine());
    if (containerType < 1 || containerType > 5)
    {
        Console.WriteLine("Invalid input");
        goto AddContainers;
    }

SelectWeight:

    Console.WriteLine("What does it weigh?");
    int cargoWeight = int.Parse(Console.ReadLine());
    if (container.MinWeight < 4 || container.MaxWeight > 30)
    {
        Console.WriteLine("Invalid input");
        goto SelectWeight;
    }

    container = new Container.Container(cargoWeight, (ContainerType)containerType);

    return container;
}





