
using Microsoft.Data.SqlClient;
using ShoeInventoryControl_POO_DB.Data;
using ShoeInventoryControl_POO_DB.Models;
using System.Data;

namespace ShoeInventoryControl_POO_DB.Repos;

public class Shoerepo
{
    private readonly DBconection _dbConnection;

    public Shoerepo(DBconection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Shoe AddShoe(
        string model,
        int brandId,
        int sizeId,
        decimal price,
        int stock)
    {
        string query = @"
            INSERT INTO Shoes (Model, BrandId, SizeId, Price, Stock)
            OUTPUT INSERTED.Id
            VALUES (@Model, @BrandId, @SizeId, @Price, @Stock);";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Model", SqlDbType.NVarChar, 100).Value = model;
        command.Parameters.Add("@BrandId", SqlDbType.Int).Value = brandId;
        command.Parameters.Add("@SizeId", SqlDbType.Int).Value = sizeId;
        command.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = stock;

        connection.Open();

        int id = (int)command.ExecuteScalar();

        Brand brand = GetBrandById(brandId);
        Size size = GetSizeById(sizeId);

        return new Shoe(id, model, brand, size, price, stock);
    }

    public List<Shoe> GetAllShoes()
    {
        List<Shoe> shoes = new();

        string query = @"
            SELECT
                s.Id,
                s.Model,
                s.Price,
                s.Stock,
                b.Id AS BrandId,
                b.Name AS BrandName,
                sz.Id AS SizeId,
                sz.Number AS SizeNumber
            FROM Shoes s
            INNER JOIN Brands b ON s.BrandId = b.Id
            INNER JOIN Sizes sz ON s.SizeId = sz.Id;";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Brand brand = new Brand(
                reader.GetInt32(4),
                reader.GetString(5)
            );

            Size size = new Size(
                reader.GetInt32(6),
                reader.GetInt32(7)
            );

            Shoe shoe = new Shoe(
                reader.GetInt32(0),
                reader.GetString(1),
                brand,
                size,
                reader.GetDecimal(2),
                reader.GetInt32(3)
            );

            shoes.Add(shoe);
        }

        return shoes;
    }

    public Shoe? GetShoeById(int id)
    {
        return GetShoesByQuery(
            "WHERE s.Id = @Value",
            command => command.Parameters.Add("@Value", SqlDbType.Int).Value = id
        ).FirstOrDefault();
    }

    public List<Shoe> SearchByModel(string model)
    {
        return GetShoesByQuery(
            "WHERE s.Model LIKE @Value",
            command => command.Parameters.Add("@Value", SqlDbType.NVarChar, 100).Value = $"%{model}%"
        );
    }

    public List<Shoe> SearchByBrand(int brandId)
    {
        return GetShoesByQuery(
            "WHERE s.BrandId = @Value",
            command => command.Parameters.Add("@Value", SqlDbType.Int).Value = brandId
        );
    }

    public List<Shoe> SearchBySize(int sizeId)
    {
        return GetShoesByQuery(
            "WHERE s.SizeId = @Value",
            command => command.Parameters.Add("@Value", SqlDbType.Int).Value = sizeId
        );
    }

    public Shoe? UpdateShoe(
        int id,
        string model,
        int brandId,
        int sizeId,
        decimal price,
        int stock)
    {
        string query = @"
            UPDATE Shoes
            SET Model = @Model,
                BrandId = @BrandId,
                SizeId = @SizeId,
                Price = @Price,
                Stock = @Stock
            WHERE Id = @Id;";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.Parameters.Add("@Model", SqlDbType.NVarChar, 100).Value = model;
        command.Parameters.Add("@BrandId", SqlDbType.Int).Value = brandId;
        command.Parameters.Add("@SizeId", SqlDbType.Int).Value = sizeId;
        command.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = stock;

        connection.Open();

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected == 0)
        {
            return null;
        }

        return GetShoeById(id);
    }

    public bool DeleteShoe(int id)
    {
        string query = "DELETE FROM Shoes WHERE Id = @Id";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }

    private List<Shoe> GetShoesByQuery(
        string condition,
        Action<SqlCommand> configureCommand)
    {
        List<Shoe> shoes = new();

        string query = $@"
            SELECT
                s.Id,
                s.Model,
                s.Price,
                s.Stock,
                b.Id,
                b.Name,
                sz.Id,
                sz.Number
            FROM Shoes s
            INNER JOIN Brands b ON s.BrandId = b.Id
            INNER JOIN Sizes sz ON s.SizeId = sz.Id
            {condition};";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        configureCommand(command);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Brand brand = new Brand(
                reader.GetInt32(4),
                reader.GetString(5)
            );

            Size size = new Size(
                reader.GetInt32(6),
                reader.GetInt32(7)
            );

            Shoe shoe = new Shoe(
                reader.GetInt32(0),
                reader.GetString(1),
                brand,
                size,
                reader.GetDecimal(2),
                reader.GetInt32(3)
            );

            shoes.Add(shoe);
        }

        return shoes;
    }

    private Brand GetBrandById(int id)
    {
        string query = "SELECT Id, Name FROM Brands WHERE Id = @Id";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
        {
            throw new KeyNotFoundException("No se encontró la marca.");
        }

        return new Brand(reader.GetInt32(0), reader.GetString(1));
    }

    private Size GetSizeById(int id)
    {
        string query = "SELECT Id, Number FROM Sizes WHERE Id = @Id";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
        {
            throw new KeyNotFoundException("No se encontró la talla.");
        }

        return new Size(reader.GetInt32(0), reader.GetInt32(1));
    }
}
