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