
namespace ShoeInventoryControl_POO_DB.Models;

public class Size
{
    public int Id { get; private set; }
    public int Number { get; private set; }

    public Size(int id, int number)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID debe ser mayor que cero.");
        }

        if (number <= 0)
        {
            throw new ArgumentException("El número de talla debe ser mayor que cero.");
        }

        Id = id;
        Number = number;
    }
}
