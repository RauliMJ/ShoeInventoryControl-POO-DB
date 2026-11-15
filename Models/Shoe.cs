
using ShoeInventoryControl_POO_DB.Models;

namespace ShoeInventoryControl_POO_DB.Models;

public class Shoe
{
    public int Id { get; private set; }
    public string Model { get; private set; }
    public Brand Brand { get; private set; }
    public Size Size { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    public Shoe(int id, string model, Brand brand, Size size, decimal price, int stock)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID debe ser mayor que cero.");
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("El modelo no puede estar vacío.");
        }

        if (brand is null)
        {
            throw new ArgumentNullException(nameof(brand));
        }

        if (size is null)
        {
            throw new ArgumentNullException(nameof(size));
        }

        if (price < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.");
        }

        if (stock < 0)
        {
            throw new ArgumentException("Las existencias no pueden ser negativas.");
        }

        Id = id;
        Model = model.Trim();
        Brand = brand;
        Size = size;
        Price = price;
        Stock = stock;
    }
}
