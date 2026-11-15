
using ShoeInventoryControl_POO_DB.Data;
using ShoeInventoryControl_POO_DB.Repos;
using ShoeInventoryControl_POO_DB.Services;
using ShoeInventoryControl_POO_DB.UI;

DBconection dbConnection = new DBconection();

Brandrepo brandRepo = new Brandrepo(dbConnection);
Sizerepo sizeRepo = new Sizerepo(dbConnection);
Shoerepo shoeRepo = new Shoerepo(dbConnection);

InventoryService inventoryService = new InventoryService(
    brandRepo,
    sizeRepo,
    shoeRepo
);

ConsoleMenu menu = new ConsoleMenu(inventoryService);

menu.Run();
