using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using MinecraftBlazorSuite.Models;

namespace MinecraftBlazorSuite.Services;

public class SqliteManager
{
    private const string UserTableQuery =
        "CREATE TABLE IF NOT EXISTS users (id INTEGER NOT NULL PRIMARY KEY, username varchar, password_hash varchar, rights int, last_login_ip varchar)";

    private const string DataSource = "Data Source=BlazorShortener.db";
    private readonly string Salt; // Hier sollte der Salt aus der Konfigurationsdatei geladen werden.

    public SqliteManager(string salt)
    {
        Salt = salt;
    }

    private SqliteConnection Con { get; set; }

    private SqliteConnection GetCon()
    {
        Con ??= new SqliteConnection(DataSource);
        CreateTables();

        return Con;
    }

    private void CreateTables()
    {
        using SqliteConnection con = new(DataSource);
        con.Open();

        SqliteCommand cmd = con.CreateCommand();
        cmd.CommandText = UserTableQuery;

        cmd.ExecuteNonQuery();
    }

    public async Task<int> CreateUser(string username, string password, PanelUserRights rights, string lastLoginIp)
    {
        using SqliteConnection con = GetCon();
        con.Open();

        // Hash des Passworts erstellen
        string passwordHash = HashPassword(password);
        SqliteCommand cmd = con.CreateCommand();

        cmd.CommandText =
            "INSERT INTO users (username, password_hash, rights, last_login_ip) VALUES (@username, @passwordHash, @rights, @lastLoginIp)";
        cmd.Parameters.AddWithValue("@username", username);

        cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
        cmd.Parameters.AddWithValue("@rights", (int)rights);

        cmd.Parameters.AddWithValue("@lastLoginIp", lastLoginIp);
        await cmd.ExecuteNonQueryAsync();

        // Abrufen der ID der zuletzt eingefügten Zeile
        cmd.CommandText = "SELECT last_insert_rowid()";
        int userId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        return userId;
    }

    public async Task UpdateUser(int userId, string username, string newPassword, PanelUserRights newRights,
        string newLastLoginIp)
    {
        using SqliteConnection con = GetCon();
        con.Open();

        // Hash des neuen Passworts erstellen
        string newPasswordHash = HashPassword(newPassword);
        SqliteCommand cmd = con.CreateCommand();

        cmd.CommandText =
            "UPDATE users SET password_hash = @passwordHash, rights = @rights, last_login_ip = @lastLoginIp WHERE id = @userId";
        cmd.Parameters.AddWithValue("@passwordHash", newPasswordHash);
        cmd.Parameters.AddWithValue("@rights", (int)newRights);
        cmd.Parameters.AddWithValue("@lastLoginIp", newLastLoginIp);
        cmd.Parameters.AddWithValue("@userId", userId);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteUser(int userId)
    {
        using SqliteConnection con = GetCon();
        con.Open();

        SqliteCommand cmd = con.CreateCommand();
        cmd.CommandText = "DELETE FROM users WHERE id = @userId";

        cmd.Parameters.AddWithValue("@userId", userId);
        await cmd.ExecuteNonQueryAsync();
    }

    public PanelUser GetUserByUsername(string username)
    {
        using SqliteConnection con = GetCon();
        con.Open();

        SqliteCommand cmd = con.CreateCommand();
        cmd.CommandText = "SELECT * FROM users WHERE username = @username";

        cmd.Parameters.AddWithValue("@username", username);
        using SqliteDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
            return new PanelUser
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Rights = (PanelUserRights)reader.GetInt32(3),
                LastLoginIp = reader.GetString(4)
            };

        return null; // Benutzer wurde nicht gefunden
    }

    public async Task<List<PanelUser>> GetAllUsers()
    {
        using SqliteConnection con = GetCon();
        con.Open();

        List<PanelUser> userList = new List<PanelUser>();
        SqliteCommand cmd = con.CreateCommand();

        cmd.CommandText = "SELECT * FROM users";
        using SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            PanelUser user = new PanelUser
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Rights = (PanelUserRights)reader.GetInt32(3),
                LastLoginIp = reader.GetString(4)
            };

            userList.Add(user);
        }

        return userList;
    }

    private string HashPassword(string password)
    {
        using Rfc2898DeriveBytes pbkdf2 = new(password, Encoding.UTF8.GetBytes(Salt), 10000);
        byte[] hashBytes = pbkdf2.GetBytes(20); // PBKDF2-Hash hat 20 Byte

        return Convert.ToBase64String(hashBytes);
    }
}