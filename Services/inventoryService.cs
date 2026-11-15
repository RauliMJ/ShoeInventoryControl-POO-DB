
using ShoeInventoryControl_POO_DB.Models;
using ShoeInventoryControl_POO_DB.Models;

namespace ShoeInventoryControl_POO_DB.Services;

public class InventoryService
{
    private readonly List<Brand> _brands = new();
    private readonly List<Size> _sizes = new();
    private readonly List<Shoe> _shoes = new();

    private int _nextBrandId = 1;
    private int _nextSizeId = 1;
    private int _nextShoeId = 1;

    public Brand AddBrand(string name)
    {
        if (_brands.Exists(brand =>
            brand.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("La marca ya está registrada.");
        }

        Brand brand = new Brand(_nextBrandId, name);

        _brands.Add(brand);
        _nextBrandId++;

        return brand;
    }

    public Size AddSize(int number)
    {
        if (_sizes.Exists(size => size.Number == number))
        {
            throw new InvalidOperationException("La talla ya está registrada.");
        }

        Size size = new Size(_nextSizeId, number);

        _sizes.Add(size);
        _nextSizeId++;

        return size;
    }

    public Brand GetBrandById(int id)
    {
        Brand brand = _brands.Find(brand => brand.Id == id);

        return brand ?? throw new KeyNotFoundException(
            "No se encontró la marca.");
    }

    public Size GetSizeById(int id)
    {
        Size size = _sizes.Find(size => size.Id == id);

        return size ?? throw new KeyNotFoundException(
            "No se encontró la talla.");
    }

    public Shoe AddShoe(
        string model,
        int brandId,
        int sizeId,
        decimal price,
        int stock)
    {
        Brand brand = GetBrandById(brandId);
        Size size = GetSizeById(sizeId);

        Shoe shoe = new Shoe(
            _nextShoeId,
            model,
            brand,
            size,
            price,
            stock
        );

        _shoes.Add(shoe);
        _nextShoeId++;

        return shoe;
    }

    public Shoe GetShoeById(int id)
    {
        Shoe shoe = _shoes.Find(shoe => shoe.Id == id);

        return shoe ?? throw new KeyNotFoundException(
            "No se encontró el calzado.");
    }

    public IReadOnlyList<Shoe> GetAllShoes()
    {
        return _shoes.AsReadOnly();
    }

    public IReadOnlyList<Shoe> SearchShoesByModel(string model)
    {
        return _shoes
            .FindAll(shoe => shoe.Model.Contains(
                model,
                StringComparison.OrdinalIgnoreCase))
            .AsReadOnly();
    }

    public Shoe UpdateShoe(
        int id,
        string model,
        int brandId,
        int sizeId,
        decimal price,
        int stock)
    {
        Shoe existingShoe = GetShoeById(id);

        Brand brand = GetBrandById(brandId);
        Size size = GetSizeById(sizeId);

        Shoe updatedShoe = new Shoe(
            id,
            model,
            brand,
            size,
            price,
            stock
        );

        int index = _shoes.IndexOf(existingShoe);
        _shoes[index] = updatedShoe;

        return updatedShoe;
    }

    public void DeleteShoe(int id)
    {
        Shoe shoe = GetShoeById(id);

        _shoes.Remove(shoe);
    }


    public IReadOnlyList<Brand> GetAllBrands()
    {
        return _brands.AsReadOnly();
    }

    public IReadOnlyList<Size> GetAllSizes()
    {
        return _sizes.AsReadOnly();
    }
}