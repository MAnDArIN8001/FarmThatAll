using System.Collections.Generic;
using NUnit.Framework;
using Storage.Items;
using Storage.Setup;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Storage.Tests
{
    [TestFixture]
    public class StorageTest
    {
        private DiContainer _container;

        private Storage _storage;

        [SetUp]
        public void Setup()
        {
            _container = new DiContainer();
            
            var testItemSetup = new TestItemSetup();

            _container.Bind<ItemsSetup>().FromInstance(testItemSetup).AsSingle();
            _container.BindInterfacesAndSelfTo<Storage>().FromNew().AsSingle();

            _storage = _container.Resolve<Storage>();
        }

        [Test]
        public void TestGetItemWithScopeReturnsCorrectValue()
        {
            _storage.IncreaseItem(ItemType.Blueberry, 5);
            _storage.IncreaseItem(ItemType.Stone, 4);

            var itemInFoodScope = _storage.GetItemsWithScope(ItemScope.Food);
            
            Debug.Log(itemInFoodScope.Count);
            
            Assert.AreEqual(1, itemInFoodScope.Count);
            Assert.IsTrue(itemInFoodScope.Exists(item => item.Item.ItemType == ItemType.Blueberry));
            
            var itemInResourceScope = _storage.GetItemsWithScope(ItemScope.Resource);
            
            Assert.AreEqual(1, itemInResourceScope.Count);
            Assert.IsTrue(itemInResourceScope.Exists(item => item.Item.ItemType == ItemType.Stone));
        }

        [Test]
        public void TestGetItemCountReturnsCorrectValue()
        {
            var increaseValue = 3;
            var decreaseValue = 2;
            
            var blueberryItemCountOnStart = _storage.GetItemsCount(ItemType.Blueberry);
            
            _storage.IncreaseItem(ItemType.Blueberry, increaseValue);
            
            var blueberryItemCount = _storage.GetItemsCount(ItemType.Blueberry);
            
            Assert.AreEqual(blueberryItemCountOnStart + increaseValue, blueberryItemCount);
            
            blueberryItemCountOnStart = _storage.GetItemsCount(ItemType.Blueberry);
            
            _storage.DecreaseItem(ItemType.Blueberry, decreaseValue);
            
            blueberryItemCount = _storage.GetItemsCount(ItemType.Blueberry);
            
            Assert.AreEqual(blueberryItemCountOnStart - decreaseValue, blueberryItemCount);
        }
    }

    public class TestItemSetup : ItemsSetup
    {
        public TestItemSetup()
        {
            var item1 = new Item("Blueberry", ItemType.Blueberry);
            var item2 = new Item("Stone", ItemType.Stone);

            _itemBindings = new List<ItemBinding>
            {
                new ItemBinding(ItemScope.Food, new List<Item>() { item1 }),
                new ItemBinding(ItemScope.Resource, new List<Item>() { item2 }),
            };
        }
    }
}