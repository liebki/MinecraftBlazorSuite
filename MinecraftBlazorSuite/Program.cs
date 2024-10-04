/*
    <MinecraftBlazorSuite - Minecraft Server Wrapper>
    Copyright (C) <2024>  <liebki>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program (License.txt).  If not, see <https://www.gnu.org/licenses/>.
 */

using MinecraftBlazorSuite.Manager;
using MinecraftBlazorSuite.Services;
using MudBlazor.Services;

namespace MinecraftBlazorSuite;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(
            "This software is licensed under the 'GNU AFFERO GENERAL PUBLIC LICENSE' V3.0 all details can be found under License.txt found in the same directory.");

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddRazorPages();

        builder.Services.AddServerSideBlazor();
        builder.Services.AddMudServices();
        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<AddressContext>();
        
        builder.Services.AddSingleton<ServerManagementService>();
        builder.Services.AddSingleton<CryptoService>();
        
        builder.Services.AddSingleton<MinecraftServerStateService>();
        builder.Services.AddSingleton<SettingsService>();
        
        builder.Services.AddSingleton<NotificationService>();
        builder.Services.AddScoped<SqliteService>();
        
        WebApplication app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.MapBlazorHub();

        app.MapFallbackToPage("/_Host");
        app.Run();
    }
}