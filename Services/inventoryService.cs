
using ShoeInventoryControl_POO_DB.Models;
using ShoeInventoryControl_POO_DB.Repos;

namespace ShoeInventoryControl_POO_DB.Services;

public class InventoryService
{
    private readonly Brandrepo _brandRepo;
    private readonly Sizerepo _sizeRepo;
    private readonly Shoerepo _shoeRepo;

    public InventoryService(
        Brandrepo brandRepo,
        Sizerepo sizeRepo,
        Shoerepo shoeRepo)
    {
        _brandRepo = brandRepo;
        _sizeRepo = sizeRepo;
        _shoeRepo = shoeRepo;
    }

    public Brand AddBrand(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la marca no puede estar vacío.");
        }

        bool exists = _brandRepo.GetAllBrands().Exists(
            brand => brand.Name.Equals(
                name.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new InvalidOperationException("La marca ya está registrada.");
        }

        return _brandRepo.AddBrand(name.Trim());
    }

    public List<Brand> GetAllBrands()
    {
        return _brandRepo.GetAllBrands();
    }

    public Size AddSize(int number)
    {
        if (number <= 0)
        {
            throw new ArgumentException("El número de talla debe ser mayor que cero.");
        }

        bool exists = _sizeRepo.GetAllSizes().Exists(
            size => size.Number == number);

        if (exists)
        {
            throw new InvalidOperationException("La talla ya está registrada.");
        }

        return _sizeRepo.AddSize(number);
    }

    public List<Size> GetAllSizes()
    {
        return _sizeRepo.GetAllSizes();
    }

    public Shoe AddShoe(
        string model,
        int brandId,
        int sizeId,
        decimal price,
        int stock)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("El modelo no puede estar vacío.");
        }

        if (price < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.");
        }

        if (stock < 0)
        {
            throw new ArgumentException("Las existencias no pueden ser negativas.");
        }

        Brand? brand = _brandRepo.GetAllBrands()
            .Find(brand => brand.Id == brandId);

        if (brand is null)
        {
            throw new KeyNotFoundException("No se encontró la marca.");
        }

        Size? size = _sizeRepo.GetSizeById(sizeId);

        if (size is null)
        {
            throw new KeyNotFoundException("No se encontró la talla.");
        }

        return _shoeRepo.AddShoe(
            model.Trim(),
            brandId,
            sizeId,
            price,
            stock);
    }

    public List<Shoe> GetAllShoes()
    {
        return _shoeRepo.GetAllShoes();
    }

    public Shoe? GetShoeById(int id)
    {
        return _shoeRepo.GetShoeById(id);
    }

    public List<Shoe> SearchShoesByModel(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("El modelo de búsqueda no puede estar vacío.");
        }

        return _shoeRepo.SearchByModel(model.Trim());
    }

    public List<Shoe> SearchShoesByBrand(int brandId)
    {
        return _shoeRepo.SearchByBrand(brandId);
    }

    public List<Shoe> SearchShoesBySize(int sizeId)
    {
        return _shoeRepo.SearchBySize(sizeId);
    }

    public Shoe? UpdateShoe(
        int id,
        string model,
        int brandId,
        int sizeId,
        decimal price,
        int stock)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("El modelo no puede estar vacío.");
        }

        if (price < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.");
        }

        if (stock < 0)
        {
            throw new ArgumentException("Las existencias no pueden ser negativas.");
        }

        if (_brandRepo.GetAllBrands().Find(brand => brand.Id == brandId) is null)
        {
            throw new KeyNotFoundException("No se encontró la marca.");
        }

        if (_sizeRepo.GetSizeById(sizeId) is null)
        {
            throw new KeyNotFoundException("No se encontró la talla.");
        }

        return _shoeRepo.UpdateShoe(
            id,
            model.Trim(),
            brandId,
            sizeId,
            price,
            stock);
    }

    public bool DeleteShoe(int id)
    {
        return _shoeRepo.DeleteShoe(id);
    }
}
