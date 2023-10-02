
namespace MinecraftBlazorSuite.Models.LogEntries
{
    public class LoginLogEntry
    {
        public LoginLogEntry(string time, string playerName, string ipAddress, string uUID)
        {
            Time = time;
            PlayerName = playerName;
            IPAddress = ipAddress;
            UUID = uUID;
        }

        public string Time { get; set; }
        public string PlayerName { get; set; }
        public string IPAddress { get; set; }
        public string UUID { get; set; }

        public override string ToString()
        {
            return $"{Time} {PlayerName} {IPAddress} {UUID}";
        }
    }
}
