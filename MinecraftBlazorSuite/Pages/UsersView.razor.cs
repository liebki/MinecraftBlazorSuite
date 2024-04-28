using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Services;

namespace MinecraftBlazorSuite.Pages;

partial class UsersView
{
    private string customPassword; // Separate Variable für das benutzerdefinierte Passwort
    private string deleteUsername;
    private string newLastLoginIp;
    private string newPassword;
    private PanelUserRights newRights;

    private PanelUser newUser = new();
    private PanelUser selectedUser; // Ausgewählter Benutzer für die Aktualisierung
    private string updateUsername;

    private List<PanelUser> userList = new();

    [Inject] public SqliteManager sqliteMan { get; set; }

    private async Task UpdateUser()
    {
        if (selectedUser != null)
        {
            await sqliteMan.UpdateUser(selectedUser.Id, selectedUser.Username, newPassword, newRights, newLastLoginIp);
            userList = await sqliteMan.GetAllUsers();
        }

        newPassword = newLastLoginIp = string.Empty; // Leeren Sie die Eingabefelder
    }

    private async Task CreateUser()
    {
        // Das benutzerdefinierte Passwort aus der Variable abrufen
        string userPassword = customPassword;

        // Erstellen Sie den Benutzer mit dem benutzerdefinierten Passwort
        int userId = await sqliteMan.CreateUser(newUser.Username, userPassword, newUser.Rights, newUser.LastLoginIp);
        userList.Add(new PanelUser
            { Id = userId, Username = newUser.Username, Rights = newUser.Rights, LastLoginIp = newUser.LastLoginIp });

        newUser = new PanelUser(); // Leeren Sie das Eingabeformular
        customPassword = null; // Leeren Sie das benutzerdefinierte Passwort
    }

    private async Task DeleteUser()
    {
        await sqliteMan.DeleteUser(selectedUser.Id);
        userList = await sqliteMan.GetAllUsers();

        deleteUsername = null; // Leeren Sie das Eingabefeld
    }

    protected override async Task OnInitializedAsync()
    {
        userList = await sqliteMan.GetAllUsers();
    }
}