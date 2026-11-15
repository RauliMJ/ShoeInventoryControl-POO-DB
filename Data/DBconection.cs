
using Microsoft.Data.SqlClient;

namespace ShoeInventoryControl_POO_DB.Data;

public class DBconection
{
    private readonly string _connectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=ShoeInventoryDB;Integrated Security=True;TrustServerCertificate=True;";

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
