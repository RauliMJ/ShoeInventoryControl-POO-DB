
namespace ShoeInventoryControl_POO_DB.Models;

public class Brand
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    public Brand(int id, string name)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID debe ser mayor que cero.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la marca no puede estar vacío.");
        }

        Id = id;
        Name = name.Trim();
    }
}
