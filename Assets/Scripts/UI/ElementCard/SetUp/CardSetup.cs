using Storage.Items;
using UnityEngine;

namespace UI.ElementCard.SetUp
{
    public struct CardSetup
    {
        public string Name { get; set; }

        public int Count { get; set; }
        
        public ItemScope ItemScope { get; set; }
        public ItemType ItemType { get; set; }

        public Sprite Icon { get; set; }
    }
}