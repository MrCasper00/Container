
using System.ComponentModel;
using System.Runtime.CompilerServices;

Ship ship = new Ship(5, 5, 1000);

void Main()
{
    Console.WriteLine("How many containers do you want to add?");
    int numberOfContainers = int.Parse(Console.ReadLine());

    if (numberOfContainers < 1)
    {
        Console.WriteLine("Invalid input");
        return;
    }

    List<Container> containers = new List<Container>();
    for (int i = 0; i < numberOfContainers; i++)
    {
        Container container = CreateContainer();
        containers.Add(container);
    }


}

// when placing things ontop only the most bottom one matters

Container CreateContainer()
{
    Container container = null;

    AddContainers:
    Console.WriteLine("What kind of cargo is it?");
    Console.WriteLine("0. Valuable and Cooled");
    Console.WriteLine("1. Valuable");
    Console.WriteLine("2. Cooled");
    Console.WriteLine("3. Ordinary");

    int containerType = int.Parse(Console.ReadLine());
    if (containerType < 0 || containerType > 3)
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

    container = new Container(cargoWeight, (ContainerType)containerType);

    return container;
}

public enum ContainerType
{
    ValuableCooled,
    Valueable,
    Cooled,
    Ordinary
}


public class Ship()
{
    public int WidthInContainers { get; private set; }
    public int LengthInContainers { get; private set; }
    public int MaxWeight { get; private set; }
    public int Weight { get; private set; }
    public int WeightRight { get; private set; }
    public int WeightLeft { get; private set; }
    public ContainerStack[,] Cargo { get; private set; }
    public Ship(int width, int length, int Max) : this()
    {
        this.WidthInContainers = width;
        this.LengthInContainers = length;
        this.MaxWeight = Max;

        for (int l = 0; l < LengthInContainers; l++)
        {
            for (int w = 0; w < WidthInContainers; w++)
            {
                if (l == 0)
                {
                    //first row
                    Cargo[l, w] = new ContainerStack(true, true);
                }
                else if (l == LengthInContainers - 1)
                {
                    //last row
                    Cargo[l, w] = new ContainerStack(false, true);
                }
                else
                {
                    Cargo[l, w] = new ContainerStack(false, false);
                }
            }
        }
    }

    public bool AddContainer(Container container)
    {
        if ()
    }



}

public class ContainerStack()
{
    public bool isCooled { get; private set; }
    public bool isAccessible { get; private set; }
    public int TotalWeight { get; private set; }
    public List<Container> Containers { get; private set; } = new List<Container>();

    public ContainerStack(bool cooled, bool accessible) : this()
    {
        this.isCooled = cooled;
        this.isAccessible = accessible;
    }
    
    public bool AddContainer(Container container)
    {
        if (!IsAllowed(container))
        {
            return false;
        }
        Containers.Add(container);
        SetTotalWeight();
        Containers.First().AddWeightOnTop(container.Weight);

        return true;
    }

    private bool IsAllowed(Container container)
    {
        if (!isCooled && container.ContainerType == ContainerType.Cooled || !isCooled && container.ContainerType == ContainerType.ValuableCooled)
        {
            return false;
        }
        if (!isAccessible && container.ContainerType == ContainerType.Valueable || !isAccessible && container.ContainerType == ContainerType.ValuableCooled)
        {
            return false;
        }

        if (Containers.Last().ContainerType == ContainerType.Valueable || Containers.Last().ContainerType == ContainerType.ValuableCooled)
        {
            return false;
        }

        if (!WeightAllowed(container))
        {
            return false;
        }

        return true;
    }

    private bool WeightAllowed(Container container)
    {
        if (Containers.Count == 0)
        {
            return true;
        }
        return Containers.First().MaxWeightOnTop >= Containers.First().WeightOnTop + container.Weight;
    }

    public void SetTotalWeight()
    {
        int totalWeight = 0;
        foreach (var container in Containers)
        {
            totalWeight += container.Weight;
        }
        this.TotalWeight = totalWeight;
    }
}


public class Container
{
    public int MaxWeight { get; private set; } = 30;
    public int MinWeight { get; private set; } = 4;
    public int MaxWeightOnTop { get; private set; } = 120;
    public int Weight { get; private set; }
    public int WeightOnTop { get; private set; }
    public ContainerType ContainerType { get; private set; }

    public Container(int weight, ContainerType type)
    {
        if (weight < MinWeight || weight > MaxWeight)
        {
            throw new ArgumentOutOfRangeException();
        }
        this.ContainerType= type;
        this.Weight = weight;
    }

    public void AddWeightOnTop(int weight)
    {
        if (weight < 0 || weight > MaxWeightOnTop)
        {
            throw new ArgumentOutOfRangeException();
        }
        this.WeightOnTop = weight;
    }
}

