InventorySystem

Repository link:
https://github.com/labramusic/InventorySystem


Buttons and input mappings:

-WASD/arrow keys - player movement on XY axis
-I - open/close inventory screen 
-E - open/close equipment screen 
-C - open/close attributes screen 
-K - switch collision checking method used
-space - spawn random item near current player position
-left mouse button - places an item currently in the air in an unoccupied inventory slot, swaps with an existing inventory item or places the item stack on the ground, focuses camera on clicked item on ground
-right mouse button - interacts with an item on the ground (if spatial or other input collision is selected), equips/dequips an equippable item to its corresponding slot, opens split stack screen for stackable inventory items
-middle mouse button - consumes a consumable item when over its inventory slot

Touch controls:
-single tap - display tooltip for tapped item, focus item on ground
-double tap - use consumable from inventory or equip/unequip equippable item
-tap and hold - select item from slot and drag while touching screen
-release - slot item if finger over slot, place on ground if over environment, otherwise return to original slot
-pinch screen - zoom camera in/out



Scripts by subtask point:

#1 - Update player camera
CameraController - controls the 2D camera
InteractableItem - mouse event handler for focus

#2 - Pickup items from the ground 
PlayerCollisionController - manages collision for player

#3 - Update UI 

#4 - Update equip screen
Equipment - manages equipment data and logic 
EquipmentUI - updates equipment UI changes 

#5 - Update item interaction 
StackSplitPanel - manages item stack split screen and interaction

#6 - Update items 
ExpendableItem - equippable item wrapper that holds current durability for each item instance
PlayerMovementController - triggers events for X units walked

#7 - Update attributes 
Attribute - models a player attribute and applies any modifiers
AttributesUI - displays player attributes
PlayerAttributes - manages player's attributes values

#8 - Spendable attributes and buff system
SpendableAttribute - attribute with an additional current value
AttributeModifier - modifies/increments attribute value based on absolute amount or percentage of base value
TimedAttributeModifier - attribute modifier that involves a timed event (hold, ramp and hold, or modify each second)
BuffTimer - timer with tick and finish events dependent on modifier values
PlayerAttributes - starts timer for attributes when an item is consume 

Example items:
Steak - Buffs strength by 30% of base strength and holds new value for 4 seconds
Beer - ramps vitality for 25 in 2 seconds and holds new value for 5 seconds
Health Potion - restores current health by 3 each second for 6 seconds
Poison - damages current health by 3 each second for 6 seconds

#9 - Mobile touch input
InputController - sets input source based on platform on startup
IInputSource - interface for input checking for commands shared between platforms
InputSourceMobile - input source for mobile platforms
ItemSelector - manages input and actions over item slots

#10 - Analytics
InputController - invokes platform dependent analytics event on startup, never destroyed
Inventory - analytics event on item picked up and when used 
Equipment - analytics event on item equipped 
UIPanel - panel that can be toggled, analytics event when opened
PlayerMovementController - analytics event on every 10 units walked, 5 times in a single execution