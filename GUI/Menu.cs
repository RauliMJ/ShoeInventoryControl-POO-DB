
using ShoeInventoryControl_POO_DB.Models;
using ShoeInventoryControl_POO_DB.Services;

namespace ShoeInventoryControl_POO_DB.UI;

public class ConsoleMenu
{
    private readonly InventoryService _inventoryService;

    public ConsoleMenu(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public void Run()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n=== CONTROL DE INVENTARIO DE ZAPATOS ===");
            Console.WriteLine("1. Gestionar calzados");
            Console.WriteLine("2. Gestionar marcas");
            Console.WriteLine("3. Gestionar tallas");
            Console.WriteLine("0. Salir");

            int option = ReadInt("Seleccione una opción: ");

            switch (option)
            {
                case 1:
                    ManageShoes();
                    break;

                case 2:
                    ManageBrands();
                    break;

                case 3:
                    ManageSizes();
                    break;

                case 0:
                    exit = true;
                    Console.WriteLine("\n¡Gracias por utilizar el sistema!");
                    break;

                default:
                    Console.WriteLine("\nOpción no válida.");
                    break;
            }
        }
    }

    private void ManageShoes()
    {
        bool back = false;

        while (!back)
        {
            Console.WriteLine("\n=== GESTIÓN DE CALZADOS ===");
            Console.WriteLine("1. Registrar calzado");
            Console.WriteLine("2. Buscar calzado");
            Console.WriteLine("3. Modificar calzado");
            Console.WriteLine("4. Eliminar calzado");
            Console.WriteLine("5. Lista de calzados");
            Console.WriteLine("0. Volver al menú principal");

            int option = ReadInt("Seleccione una opción: ");

            switch (option)
            {
                case 1:
                    ExecuteSafely(AddShoe);
                    break;

                case 2:
                    ExecuteSafely(SearchShoe);
                    break;

                case 3:
                    ExecuteSafely(UpdateShoe);
                    break;

                case 4:
                    ExecuteSafely(DeleteShoe);
                    break;

                case 5:
                    ExecuteSafely(ListShoes);
                    break;

                case 0:
                    back = true;
                    break;

                default:
                    Console.WriteLine("\nOpción no válida.");
                    break;
            }
        }
    }

    private void ManageBrands()
    {
        bool back = false;

        while (!back)
        {
            Console.WriteLine("\n=== GESTIÓN DE MARCAS ===");
            Console.WriteLine("1. Registrar marca");
            Console.WriteLine("2. Lista de marcas");
            Console.WriteLine("0. Volver al menú principal");

            int option = ReadInt("Seleccione una opción: ");

            switch (option)
            {
                case 1:
                    ExecuteSafely(AddBrand);
                    break;

                case 2:
                    ExecuteSafely(ListBrands);
                    break;

                case 0:
                    back = true;
                    break;

                default:
                    Console.WriteLine("\nOpción no válida.");
                    break;
            }
        }
    }

    private void ManageSizes()
    {
        bool back = false;

        while (!back)
        {
            Console.WriteLine("\n=== GESTIÓN DE TALLAS ===");
            Console.WriteLine("1. Registrar talla");
            Console.WriteLine("2. Lista de tallas");
            Console.WriteLine("0. Volver al menú principal");

            int option = ReadInt("Seleccione una opción: ");

            switch (option)
            {
                case 1:
                    ExecuteSafely(AddSize);
                    break;

                case 2:
                    ExecuteSafely(ListSizes);
                    break;

                case 0:
                    back = true;
                    break;

                default:
                    Console.WriteLine("\nOpción no válida.");
                    break;
            }
        }
    }

    private void AddBrand()
    {
        string name = ReadText("Nombre de la marca: ");

        Brand brand = _inventoryService.AddBrand(name);

        Console.WriteLine(
            $"\nMarca registrada correctamente. ID: {brand.Id}");
    }

    private void ListBrands()
    {
        List<Brand> brands = _inventoryService.GetAllBrands();

        if (brands.Count == 0)
        {
            Console.WriteLine("\nNo hay marcas registradas.");
            return;
        }

        Console.WriteLine("\n=== MARCAS REGISTRADAS ===");

        foreach (Brand brand in brands)
        {
            Console.WriteLine($"ID: {brand.Id} | Nombre: {brand.Name}");
        }
    }

    private void AddSize()
    {
        int number = ReadInt("Número de talla: ");

        Size size = _inventoryService.AddSize(number);

        Console.WriteLine(
            $"\nTalla registrada correctamente. ID: {size.Id}");
    }

    private void ListSizes()
    {
        List<Size> sizes = _inventoryService.GetAllSizes();

        if (sizes.Count == 0)
        {
            Console.WriteLine("\nNo hay tallas registradas.");
            return;
        }

        Console.WriteLine("\n=== TALLAS REGISTRADAS ===");

        foreach (Size size in sizes)
        {
            Console.WriteLine($"ID: {size.Id} | Número: {size.Number}");
        }
    }

    private void AddShoe()
    {
        string model = ReadText("Modelo: ");

        ListBrands();
        int brandId = ReadInt("\nID de la marca: ");

        ListSizes();
        int sizeId = ReadInt("\nID de la talla: ");

        decimal price = ReadDecimal("Precio: ");
        int stock = ReadInt("Existencias: ");

        Shoe shoe = _inventoryService.AddShoe(
            model,
            brandId,
            sizeId,
            price,
            stock
        );

        Console.WriteLine(
            $"\nCalzado registrado correctamente. ID: {shoe.Id}");
    }

    private void SearchShoe()
    {
        Console.WriteLine("\n=== BUSCAR CALZADO ===");
        Console.WriteLine("1. Buscar por ID");
        Console.WriteLine("2. Buscar por modelo");

        int option = ReadInt("Seleccione una opción: ");

        switch (option)
        {
            case 1:
                int id = ReadInt("ID del calzado: ");

                Shoe? shoe = _inventoryService.GetShoeById(id);

                if (shoe is null)
                {
                    Console.WriteLine("\nNo se encontró el calzado.");
                    return;
                }

                DisplayShoe(shoe);
                break;

            case 2:
                string model = ReadText("Modelo a buscar: ");

                List<Shoe> results =
                    _inventoryService.SearchShoesByModel(model);

                if (results.Count == 0)
                {
                    Console.WriteLine("\nNo se encontraron resultados.");
                    return;
                }

                foreach (Shoe result in results)
                {
                    DisplayShoe(result);
                }

                break;

            default:
                Console.WriteLine("\nOpción no válida.");
                break;
        }
    }

    private void UpdateShoe()
    {
        int id = ReadInt("ID del calzado que desea modificar: ");

        Shoe? existingShoe = _inventoryService.GetShoeById(id);

        if (existingShoe is null)
        {
            Console.WriteLine("\nNo se encontró el calzado.");
            return;
        }

        Console.WriteLine("\nIngrese los nuevos datos:");

        string model = ReadText("Nuevo modelo: ");

        ListBrands();
        int brandId = ReadInt("\nNuevo ID de la marca: ");

        ListSizes();
        int sizeId = ReadInt("\nNuevo ID de la talla: ");

        decimal price = ReadDecimal("Nuevo precio: ");
        int stock = ReadInt("Nuevas existencias: ");

        Shoe? updatedShoe = _inventoryService.UpdateShoe(
            id,
            model,
            brandId,
            sizeId,
            price,
            stock
        );

        if (updatedShoe is null)
        {
            Console.WriteLine("\nNo se pudo encontrar el calzado para modificar.");
            return;
        }

        Console.WriteLine("\nCalzado modificado correctamente.");
        DisplayShoe(updatedShoe);
    }

    private void DeleteShoe()
    {
        int id = ReadInt("ID del calzado que desea eliminar: ");

        Shoe? shoe = _inventoryService.GetShoeById(id);

        if (shoe is null)
        {
            Console.WriteLine("\nNo se encontró el calzado.");
            return;
        }

        DisplayShoe(shoe);

        Console.Write("\n¿Está seguro de que desea eliminarlo? (S/N): ");
        string confirmation = Console.ReadLine() ?? string.Empty;

        if (confirmation.Equals("S", StringComparison.OrdinalIgnoreCase))
        {
            bool deleted = _inventoryService.DeleteShoe(id);

            if (deleted)
            {
                Console.WriteLine("\nCalzado eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("\nNo se pudo eliminar el calzado.");
            }
        }
        else
        {
            Console.WriteLine("\nOperación cancelada.");
        }
    }

    private void ListShoes()
    {
        List<Shoe> shoes = _inventoryService.GetAllShoes();

        if (shoes.Count == 0)
        {
            Console.WriteLine("\nNo hay calzados registrados.");
            return;
        }

        Console.WriteLine("\n=== INVENTARIO DE CALZADOS ===");

        foreach (Shoe shoe in shoes)
        {
            DisplayShoe(shoe);
        }
    }

    private void DisplayShoe(Shoe shoe)
    {
        Console.WriteLine("\n------------------------------");
        Console.WriteLine($"ID: {shoe.Id}");
        Console.WriteLine($"Modelo: {shoe.Model}");
        Console.WriteLine($"Marca: {shoe.Brand.Name}");
        Console.WriteLine($"Talla: {shoe.Size.Number}");
        Console.WriteLine($"Precio: ${shoe.Price:F2}");
        Console.WriteLine($"Existencias: {shoe.Stock}");
        Console.WriteLine("------------------------------");
    }

    private int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }

            Console.WriteLine(
                "Entrada no válida. Introduzca un número entero.");
        }
    }

    private decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (decimal.TryParse(Console.ReadLine(), out decimal value))
            {
                return value;
            }

            Console.WriteLine(
                "Entrada no válida. Introduzca un número válido.");
        }
    }

    private string ReadText(string message)
    {
        while (true)
        {
            Console.Write(message);

            string value = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Este campo no puede estar vacío.");
        }
    }

    private void ExecuteSafely(Action operation)
    {
        try
        {
            operation();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }
}
