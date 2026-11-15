
using ShoeInventoryControl_POO_DB.Services;
using ShoeInventoryControl_POO_DB.UI;

InventoryService inventoryService = new InventoryService();

ConsoleMenu menu = new ConsoleMenu(inventoryService);

menu.Run();
