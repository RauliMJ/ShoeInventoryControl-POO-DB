
using Microsoft.Data.SqlClient;
using ShoeInventoryControl_POO_DB.Data;
using ShoeInventoryControl_POO_DB.Models;

namespace ShoeInventoryControl_POO_DB.Repos;

public class Sizerepo
{
    private readonly DBconection _dbConnection;

    public Sizerepo(DBconection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Size AddSize(int number)
    {
        string query = @"
            INSERT INTO Sizes (Number)
            OUTPUT INSERTED.Id
            VALUES (@Number);";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Number", System.Data.SqlDbType.Int).Value = number;

        connection.Open();

        int id = (int)command.ExecuteScalar();

        return new Size(id, number);
    }

    public List<Size> GetAllSizes()
    {
        List<Size> sizes = new();

        string query = "SELECT Id, Number FROM Sizes";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Size size = new Size(
                reader.GetInt32(0),
                reader.GetInt32(1)
            );

            sizes.Add(size);
        }

        return sizes;
    }

    public Size? GetSizeById(int id)
    {
        string query = "SELECT Id, Number FROM Sizes WHERE Id = @Id";

        using SqlConnection connection = _dbConnection.GetConnection();
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Size(
                reader.GetInt32(0),
                reader.GetInt32(1)
            );
        }

        return null;
    }
}
