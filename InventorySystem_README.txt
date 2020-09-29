InventorySystem

Repository link:
https://github.com/labramusic/InventorySystem


Buttons and input mappings:

-WASD/arrow keys - player movement on XY axis
-I - open/close inventory screen 
-E - open/close equipment screen 
-C - open/close attributes screen 
-K - switch collision used
-space - spawn random item near current player position
-left mouse button - places an item currently in the air in an unoccupied inventory slot, swaps with an existing inventory item or places the item stack on the ground
-right mouse button - interacts with an item on the ground (if spatial collision is selected), equips/dequips an equippable item to its corresponding slot
-middle mouse button - consumes a consumable item when over its inventory slot



Scripts by subtask point:

#1 - Player movement
PlayerController - controls player movement and sprite animations
CameraController - controls the 2D camera

#2 - Pickup items from the ground 
PlayerController - checks for collision with items
InteractableItem - handles interaction with items on the ground, checks for trigger collision
CollisionTester - manages current collision checking logic used

#3 - Inventory and equipment UI 
UIPanelManager - handles opening and closing inventory and equipment screens
InventoryUI - manages inventory UI changes
EquipmentUI - updates equipment UI changes 

#4 - Inventory grid 
Inventory - manages inventory data and logic 
InventorySlot - displays inventory item slot
InventoryUI

#5 - Equip screen
Equipment - manages equipment data and logic 
EquipSlot - displays equipment item slot
EquipmentUI

#6 - Inventory functionality
Inventory, Equipment

#7 - Item interaction 
InventorySlot, EquipSlot - handle mouse events for selecting slotted items and interacting with other slots 
ItemSlot - parent slot class
Tooltip - inventory tooltip 

#8 - Items
Item - generic item class 
    SingleUseItem - cannot be picked up, consumed on interaction
    PickupableItem - able to slot into inventory 
        EquippableItem - can be equipped 
        ConsumableItem - can be consumed from the inventory
ItemSpawner - spawns items on the ground
ItemStack - struct modelling item stacks in the inventory

#9 - Attributes 
Attribute - models a player attribute and applies any modifiers
AttributesUI - displays player attributes
AttributeModifier - serializable struct modelling attribute modifiers 
PlayerAttributes - manages player's attributes values
