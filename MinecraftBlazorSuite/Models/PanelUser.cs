using System;

namespace MinecraftBlazorSuite.Models
{
    public class PanelUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public PanelUserRights Rights { get; set; }
        public string LastLoginIp { get; set; }
    }
}