using Container;
using NUnit.Framework;
using Containerr = Container.Container;
namespace TestContainer
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestContainer_ContainerInitExpectedBehaviour()
        {
            Containerr container = new Containerr(20, ContainerType.Ordinary);

            Assert.AreEqual(20, container.Weight);
            Assert.AreEqual(ContainerType.Ordinary, container.ContainerType);

        }

        [Test]
        public void TestContainer_ContainerInitExpectedBehaviour2()
        {
            Containerr container = new Containerr(30, ContainerType.ValuableCooled);

            Assert.AreEqual(30, container.Weight);
            Assert.AreEqual(ContainerType.ValuableCooled, container.ContainerType);

        }

        [Test]
        public void TestContainer_ContainerInitExpectedBehaviour3()
        {
            Containerr container = new Containerr(4, ContainerType.ValuableCooled);

            Assert.AreEqual(4, container.Weight);
            Assert.AreEqual(ContainerType.ValuableCooled, container.ContainerType);

        }

        [Test]
        public void TestContainer_ContainerInitExpectedBehaviour4()
        {
            Containerr container = new Containerr(20, ContainerType.Cooled);

            Assert.AreEqual(20, container.Weight);
            Assert.AreEqual(ContainerType.Cooled, container.ContainerType);

        }

        [Test]
        public void TestContainer_WeightTooHigh()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Containerr(31, ContainerType.Cooled));
        }

        [Test]
        public void TestContainer_WeightTooLow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Containerr(3, ContainerType.Cooled));
        }

        [Test]
        public void TestContainer_WeightOntopToHigh()
        {
            Containerr containerToAddWeightOh = new Containerr(30, ContainerType.Cooled);

            Assert.Throws<ArgumentOutOfRangeException>(() => containerToAddWeightOh.SetWeightOnTop(150));
        }

        [Test]
        public void TestContainer_WeightOntopToLow()
        {
            Containerr containerToAddWeightOh = new Containerr(30, ContainerType.Cooled);

            Assert.Throws<ArgumentOutOfRangeException>(() => containerToAddWeightOh.SetWeightOnTop(-10));
        }
    }
}