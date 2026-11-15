
using Microsoft.Data.SqlClient;
using ShoeInventoryControl_POO_DB.Data;
using ShoeInventoryControl_POO_DB.Models;

namespace ShoeInventoryControl_POO_DB.Repos;

public class Brandrepo
{
    private readonly DBconection _dbConnection;

    public Brandrepo(DBconection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Brand AddBrand(string name)
    {
        string query = @"
            INSERT INTO Brands (Name)
            OUTPUT INSERTED.Id
            VALUES (@Name);";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", name);

        connection.Open();

        int id = (int)command.ExecuteScalar();

        return new Brand(id, name);
    }

    public List<Brand> GetAllBrands()
    {
        List<Brand> brands = new();

        string query = "SELECT Id, Name FROM Brands";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Brand brand = new Brand(
                reader.GetInt32(0),
                reader.GetString(1)
            );

            brands.Add(brand);
        }

        return brands;
    }
}
