using GameDevTV.Core.UI.Dragging;
using NoName.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.VisualScripting.Member;

namespace NoName
{
    [RequireComponent(typeof(CanvasGroup))]
    public class InventoryIconUI : DragItem<ItemSO>, IDragHandler
    {
        protected override void DropItemIntoContainer(IDragDestination<ItemSO> destination)
        {
            if (object.ReferenceEquals(destination, source)) return;

            var destinationContainer = destination as IDragContainer<ItemSO>;
            var sourceContainer = source as IDragContainer<ItemSO>;

            // Swap won't be possible
            if (destinationContainer == null || sourceContainer == null ||
                destinationContainer.GetItem() == null ||
                object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
            }
            else
            {
                if (sourceContainer.GetItem().IsCombinable && sourceContainer.GetItem().ComplementaryItem.Equals(destinationContainer.GetItem()))
                {
                    CombineItem(sourceContainer.GetItem().ResultingItem, sourceContainer.GetItem().ResultingItemQuantity, destinationContainer, sourceContainer);
                }
                else
                {
                    AttemptSwap(destinationContainer, sourceContainer);
                }
            }
        }

        protected void CombineItem(ItemSO resultingItem, int quantity, IDragContainer<ItemSO> destinationContainer, IDragContainer<ItemSO> sourceContainer)
        {
            sourceContainer.RemoveItems(1);
            destinationContainer.RemoveItems(1);

            destinationContainer.AddItems(resultingItem, quantity);
        }
    }
}
