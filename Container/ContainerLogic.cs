using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Container
{
    public enum ContainerType
    {
        Ordinary = 1,
        Valueable = 2,
        Cooled = 3,
        ValuableCooled = 4
    }

    public class Captain()
    {
        public Ship Ship { get; private set; }

        public List<Container> Containers { get; private set; } = new List<Container>();

        public List<Container> LeftOverContainers { get; private set; } = new List<Container>();

        public Captain(int width, int length) : this()
        {
            Ship = new Ship(width, length);
        }
        public void AddContainers(List<Container> container)
        {
            InputValidation(container);
            foreach (var cont in container)
            {
                Containers.Add(cont);
            }
        }

        private void InputValidation(List<Container> container)
        {
            int containerWeight = 0;
            int ValuableCooled = 0;
            int Valuable = 0;
            foreach (var cont in container)
            {
                containerWeight += cont.Weight;
                if (cont.ContainerType == ContainerType.Valueable)
                {
                    Valuable++;
                }
                if (cont.ContainerType == ContainerType.ValuableCooled)
                {
                    ValuableCooled++;
                    Valuable++;
                }
            }
            if (containerWeight < Ship.MaxWeight / 2)
            {
                throw new ArgumentOutOfRangeException("Je moet minimaal 50% bezetten");
            }
            if (containerWeight > Ship.MaxWeight)
            {
                throw new ArgumentOutOfRangeException("Je hebt teveel gewicht");
            }
            if (ValuableCooled > Ship.WidthInContainers)
            {
                throw new ArgumentOutOfRangeException("Je meer waardevolle gekoelde containers dan dat er plek voor is");
            }
            if (Valuable > Ship.WidthInContainers * 2)
            {
                throw new ArgumentOutOfRangeException("Je meer waardevolle containers dan dat er plek voor is");
            }
        }

        public Ship DetermineBest()
        {
            List<Container> OrderdContainersCooled = Containers.Where(x => x.ContainerType == ContainerType.Cooled).OrderByDescending(x => x.Weight).ToList();
            List<Container> OrderdContainersOrdinary = Containers.Where(x => x.ContainerType == ContainerType.Ordinary).OrderByDescending(x => x.Weight).ToList();
            List<Container> OrderdContainersValueable = Containers.Where(x => x.ContainerType == ContainerType.Valueable).OrderByDescending(x => x.Weight).ToList();
            List<Container> OrderdContainersValueableCooled = Containers.Where(x => x.ContainerType == ContainerType.ValuableCooled).OrderByDescending(x => x.Weight).ToList();

            List<Container> mergedOrderdList = OrderdContainersCooled
            .Concat(OrderdContainersOrdinary)
            .Concat(OrderdContainersValueable)
            .Concat(OrderdContainersValueableCooled)
            .ToList();


            List<Container> containersDontFit = new List<Container>();
            foreach (var container in mergedOrderdList)
            {
                if (!Ship.AddContainer(container))
                {
                    containersDontFit.Add(container);
                }
            }

            if (containersDontFit.Count > 0)
            {
                Ship newShip;
                List<Container> containersDontFitNew;

                int numberOfTries = 0;
                while (true)
                {
                    newShip = new Ship(Ship.WidthInContainers, Ship.LengthInContainers);
                    containersDontFitNew = new List<Container>();
                    mergedOrderdList = mergedOrderdList.OrderBy(x => Guid.NewGuid()).ToList();

                    foreach (var container in mergedOrderdList)
                    {
                        if (!newShip.AddContainer(container))
                        {
                            containersDontFitNew.Add(container);
                        }
                    }

                    //Console.WriteLine($"Tried {numberOfTries} times");
                    if (containersDontFitNew.Count == 0)
                    {
                        //Console.WriteLine($"Found solution with {numberOfTries}");
                        Ship = newShip;
                        return Ship;
                    }

                    if (numberOfTries > 100000)
                    {
                        Console.WriteLine("Could not find a solution");
                        break;
                    }
                    numberOfTries++;
                }
                Console.WriteLine("Could not find a solution");
                LeftOverContainers = containersDontFitNew;
                return Ship;

            }
            else
            {
                Console.WriteLine("Found solution with 1 try with normal sorting");
                return Ship;
            }
        }

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
        public Ship(int width, int length) : this()
        {
            this.WidthInContainers = width;
            this.LengthInContainers = length;
            MaxWeight = width * length * 150;

            this.Cargo = new ContainerStack[LengthInContainers, WidthInContainers];

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

        public bool AddContainer(Container contianer)
        {
            for (int w = 0; w < WidthInContainers; w++)
            {
                if (!InBalance(contianer, w)) { continue; }

                for (int l = 0; l < LengthInContainers; l++)
                {
                    if (Cargo[l, w].AddContainer(contianer))
                    {
                        Weight += contianer.Weight;
                        UpdateWeight();
                        return true;
                    }
                }
            }
            return false;
        }

        private void UpdateWeight()
        {
            var middle = WidthInContainers / 2;
            if (Cargo.GetLength(0) % 2 != 0) //if the ship is uneven
            {
                WeightLeft = GetWeightInCollumn(0, middle + 1);
                WeightRight = GetWeightInCollumn(middle + 1, WidthInContainers);
                Weight = WeightLeft + WeightRight + GetWeightInCollumn(middle, middle + 1);
            }
            else
            {
                WeightLeft = GetWeightInCollumn(0, middle);
                WeightRight = GetWeightInCollumn(middle, WidthInContainers);
                Weight = WeightLeft + WeightRight;
            }
        }

        private bool InBalance(Container container, int collumn)
        {
            var middle = WidthInContainers / 2;

            int NewWeightLeft = 0;
            int NewWeightRight = 0;

            if (WidthInContainers % 2 != 0)
            {
                if (collumn == middle + 1 || WidthInContainers == 1)
                {
                    return true;
                }

                if (collumn < middle + 1)
                {
                    NewWeightLeft = WeightLeft + container.Weight;
                    NewWeightRight = WeightRight;
                }
                else
                {
                    NewWeightLeft = WeightLeft;
                    NewWeightRight = WeightRight + container.Weight;
                }
            }
            else
            {
                if (collumn < middle)
                {
                    NewWeightLeft = WeightLeft + container.Weight;
                    NewWeightRight = WeightRight;
                }
                else
                {
                    NewWeightLeft = WeightLeft;
                    NewWeightRight = WeightRight + container.Weight;
                }
            }

            double weightLeftPercentage = 0;
            double weightRightPercentage = 0;
            double newTotalWeight = NewWeightLeft + NewWeightRight;
            if (Weight != 0)
            {
                weightLeftPercentage = (double)NewWeightLeft / newTotalWeight * 100;
                weightRightPercentage = (double)NewWeightRight / newTotalWeight * 100;
            }

            double difference;
            difference = Math.Abs(weightLeftPercentage - weightRightPercentage);

            if (difference > 20)
            {
                if (newTotalWeight < 100)
                {
                    if (difference < 60)
                    {
                        return true;
                    }
                }
                return false;
            }

            return true;
        }

        private int GetWeightInCollumn(int startCollumn, int lastCollumn)
        {
            int totalWeight = 0;
            for (int l = 0; l < LengthInContainers; l++)
            {
                for (int w = startCollumn; w < lastCollumn; w++)
                {
                    totalWeight += Cargo[l, w].TotalWeight;
                }
            }
            return totalWeight;
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
            int weightOnTop = 0;
            foreach (var cont in Containers)
            {
                weightOnTop += cont.Weight;
            }
            weightOnTop -= Containers.First().Weight;
            Containers.First().SetWeightOnTop(weightOnTop);

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

            if (Containers.Count == 0)
            {
                return true;
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
            this.ContainerType = type;
            this.Weight = weight;
        }

        public void SetWeightOnTop(int weight)
        {
            if (weight < 0 || weight > MaxWeightOnTop)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.WeightOnTop = weight;
        }
    }
}
