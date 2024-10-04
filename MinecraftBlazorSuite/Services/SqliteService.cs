using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using MinecraftBlazorSuite.Models;

namespace MinecraftBlazorSuite.Services;

public class SqliteService
{
    private const string SessionTableQuery =
        "CREATE TABLE IF NOT EXISTS user_sessions (id INTEGER NOT NULL PRIMARY KEY, sessionvalue varchar, sessioncreated varchar    )";

    private const string DataSource = "Data Source=MinecraftBlazorSuite.db";

    public SqliteService()
    {
        CreateTables();
    }

    private SqliteConnection Con { get; set; }

    private SqliteConnection GetCon()
    {
        Con ??= new SqliteConnection(DataSource);
        CreateTables();

        return Con;
    }

    private static void CreateTables()
    {
        using SqliteConnection con = new(DataSource);
        con.Open();

        SqliteCommand cmd = con.CreateCommand();
        cmd.CommandText = SessionTableQuery;

        cmd.ExecuteNonQuery();
    }

    public void AddSession(string sessionValue, string sessionCreated)
    {
        using SqliteConnection con = GetCon();
        con.Open();

        SqliteCommand cmd = con.CreateCommand();
        cmd.CommandText =
            "INSERT INTO user_sessions (sessionvalue, sessioncreated) VALUES (@sessionvalue, @sessioncreated)";
        
        cmd.Parameters.AddWithValue("@sessionvalue", sessionValue);
        cmd.Parameters.AddWithValue("@sessioncreated", sessionCreated);
        
        cmd.ExecuteNonQueryAsync();
    }

    public void DeleteSession(string sessionValue)
    {
        using SqliteConnection con = GetCon();
        con.Open();

        SqliteCommand cmd = con.CreateCommand();
        cmd.CommandText = "DELETE FROM user_sessions WHERE sessionvalue = @sessionvalue";

        cmd.Parameters.AddWithValue("@sessionvalue", sessionValue);
        cmd.ExecuteNonQueryAsync();
    }

    public UserSession? GetSingleUserSession(string SessionValue)
    {
        using SqliteConnection con = GetCon();
        con.Open();

        SqliteCommand cmd = con.CreateCommand();
        cmd.CommandText = "SELECT sessionvalue, sessioncreated FROM user_sessions WHERE sessionvalue = @SessionValue LIMIT 1;";

        cmd.Parameters.AddWithValue("@SessionValue", SessionValue);
        using SqliteDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new UserSession
            {
                SessionValue = reader.GetString(0),
                SessionCreated = reader.GetString(1)
            };
        }

        return null;
    }

    public List<UserSession> GetAllUserSessions()
    {
        using SqliteConnection con = GetCon();
        con.Open();

        List<UserSession> userList = [];
        SqliteCommand cmd = con.CreateCommand();

        cmd.CommandText = "SELECT sessionvalue, sessioncreated FROM user_sessions";
        using SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            UserSession session = new UserSession
            {
                SessionValue = reader.GetString(0),
                SessionCreated = reader.GetString(1)
            };

            userList.Add(session);
        }

        return userList;
    }

}