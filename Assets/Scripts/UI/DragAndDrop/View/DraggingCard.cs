using Storage.Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DragAndDrop.View
{
    public class DraggingCard : MonoBehaviour
    {
        public ItemType ItemType { get; private set; }
        public ItemScope ItemScope { get; private set; }

        [field: SerializeField] public Image Icon { get; private set; }

        public void Initialize(ItemType itemType, ItemScope itemScope, Sprite icon)
        {
            ItemType = itemType;
            ItemScope = itemScope;
            Icon.sprite = icon;
        }
    }
}