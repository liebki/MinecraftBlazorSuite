# MinecraftBlazorSuite

This is a simple web-based dashboard, created to make administration of a Minecraft Java server easier.

## Features

### Web-Based Interface

- **User-Friendly Web Interface:** Easily manage your Minecraft server through a user-friendly web interface.

- **Technology Stack:** Built with C# .NET Core 8 and Blazor Server.

### Console

- **Real-time Console:** Monitor your server's console messages in real-time.

- **Execute Commands:** Execute server commands directly from the web console.

- **Customizable Display:** Limit the number of console messages displayed on the console page.

- **Server Control:** Quick-controls for things like time, status, weather, etc.

- **Server Environment Compatibility:** Please note that some functions depend on specific server environments and Minecraft versions like Forge, CraftBukkit, etc.

For more information about forks, environments, and versions see here: [spigotmc.org - article](https://www.spigotmc.org/wiki/what-is-spigot-craftbukkit-bukkit-vanilla-forg "https://www.spigotmc.org/wiki/what-is-spigot-craftbukkit-bukkit-vanilla-forg")

### Player Management

- **Player Management:** Easily manage players on your server.

- **Player List:** View a list of all players currently active on the server.

- **Actions:** Perform actions such as sending messages, changing player gamemodes, killing, kicking, banning, and granting or removing operator rights.

- **Version Compatibility:** Some actions depend on the Minecraft server version (s)

### Management

- **Customize Settings:** Configure application settings through the 'BlazorMinecraftServerSettings.json' file.

- **Adjust Visibility:** Customize the maximum number of visible console messages.

- **Server Configuration:** Set the server JAR file path, Java arguments, and server type/version for future updates.

### Compatibility

- **Tested Versions:** Blazor Minecraft Server Manager has been tested with vanilla, Spigot, and Craftbukkit server versions.

- **Recommended Version:** Only versions 1.17 or higher are really "supported" due to specific needed message formats.

### HTTPS/HTTP Configuration

- **Secure Connection:** The application supports HTTPS for secure connections using Let's Encrypt. For users who wish to enable HTTPS, configuration is required in the `appsettings.json` and `BlazorMinecraftServerSettings.json` files.

- **Port Usage:** When HTTPS is enabled, the server listens on port 443. If HTTPS is not enabled, the server defaults to HTTP on port 5000. It is recommended to use HTTPS for better security and user trust.

## Installation and Usage

### Prerequisites

Before getting started, make sure you have the following dependencies and prerequisites installed:

- **.NET Core 8:** Ensure that .NET Core 8 is installed on your system or [download it](https://dotnet.microsoft.com/en-us/download).

- **Java:** Minecraft servers need Java to run. Verify that you have the required Java version installed for the right minecraft version. Download java runtimes for example at "azul" [here](https://www.azul.com/downloads/?package=jdk#zulu).

Information about java and minecraft: [minecraft.fandom.com - java](https://minecraft.fandom.com/wiki/Tutorials/Update_Java#Why_update? "https://minecraft.fandom.com/wiki/Tutorials/Update_Java#Why_update?")

- Java Edition 1.12 to 1.16.5 requires Java 8 (1.8.0) or newer.

- Java Edition 1.17 to 1.17.1 requires Java 16 or newer.

- Java Edition 1.18, Minecraft requires Java 17 or newer.

- Since Java Edition 1.21, Minecraft requires [Java 21 or newer and a 64-Bit system](https://minecraft.wiki/w/Java_Edition_1.21).

### Installation

Follow these steps to install and use Blazor Minecraft Server Manager:

1. **Download:** Download the latest release from the [Releases](https://github.com/liebki/MinecraftBlazorSuite/releases) section of the repository.

2. **Configuration:**
  - **BlazorMinecraftServerSettings.json:** Customize the Minecraft server settings, such as the server jar file path and Java command arguments.
  - **Enabling HTTPS and a Domain Name with Let's Encrypt:**
    If you want to use HTTPS with a custom domain and automatically generate SSL certificates using Let's Encrypt, you'll need to update the following configuration files:

    - In the `appsettings.json` file, under the `LettuceEncrypt` section, provide the necessary details for the certificate generation:
      ```json
      "LettuceEncrypt": {
        "AcceptTermsOfService": true,
        "DomainNames": [ "yourdomain.com", "www.yourdomain.de" ],
        "EmailAddress": "your-email@server.com"
      }
      ```
      - Replace the `DomainNames` with your actual domain and subdomain (if applicable).
      - Provide a valid email address in the `EmailAddress` field. This is required for Let's Encrypt to contact you regarding certificate expiration and updates.

    - In the `BlazorMinecraftServerSettings.json` file, set the option `"UseLetsEncrypt"` to `true`:
      ```json
      "UseLetsEncrypt": true
      ```
      - When `"UseLetsEncrypt": true`, the server will automatically use HTTPS on port 443. If this option is set to `false`, the server will use HTTP on port 5000 by default.

3. **Launch:** Run the application along with the Minecraft server using .NET Core: `dotnet [MinecraftBlazorSuite.dll/MinecraftBlazorSuite.exe]`

4. **Access:**
- If HTTPS is enabled and configured with Let's Encrypt, open your web browser and access the web interface by entering the following URL:
  - `https://yourdomain.com`
- If HTTPS is not enabled, the web interface will be accessible at:
  - `http://server-ip:5000`

### Version limitations:

Please be aware that this is work in progress, and there are some issues:

- **I can't support all versions:**

  - They are different in style, the parsing would have to be changed to support every version, I don't have the time and energy to add all versions, I noticed that everything from 1.17 and upwards works so I will stick with this for now.

- **Using older Versions than 1.17:** 

  - You can use theoretically any version, most features will just not work as expected or at all because the messages can't be parsed but the console for example should work at least.
### Troubleshooting

If you encounter any issues during installation or usage, refer to the following troubleshooting tips [or create an issue](https://github.com/liebki/MinecraftBlazorSuite/issues):

- **Q: The MinecraftBlazorSuite console says something like "Error: Unable to access jarfile x.... .jar"**  
  **A:** Make sure the path in the settings file is correct. It sounds like the specified path does not point to the actual JAR file.


- **Q: How do I start the server after I started the interface?**  
  **A:** Simply log in and visit the console page at `/console`. The server should start after a few seconds. If it doesn’t, verify that the location of the server JAR file is correct.


- **Q: The MinecraftBlazorSuite console says something like "File/Process is locked x...."**  
  **A:** This usually indicates that another server instance is already running. Try closing all running server instances or restart your entire system to resolve this more quickly.


- **Q: How do I log in to the panel?**  
  **A:** The default password to log in is '12345678'. Please change it after your first login! Note that sessions are only valid for about three hours; if you try to access the panel after this period, you will need to log in again. Multiple users can access the interface simultaneously, but there are no checks in place for concurrent logins.


- **Q: After logging in, I can't open the /console page; I get a red bar at the bottom which says "an error occurred."**  
  **A:** This issue likely arises because the server JAR file is not set correctly, or the file is incompatible with the server. Please ensure the path to the JAR file is correct before reporting the problem.


- **Q: What do I need to do to use Let's Encrypt with my domain name?**  
  **A:** To use Let's Encrypt for SSL certification with your domain name, ensure that your domain is correctly configured to point to the server where the application is running. 
  This is essential for Let's Encrypt to verify ownership of the domain. You can typically do this by setting an A record in your domain's DNS settings that directs the domain to your server's IP address.
  Once the domain points to your server, the application will be able to request and validate the SSL certificate. [Getting Started with Let's Encrypt](https://letsencrypt.org/getting-started/).


## Screenshots / Videos

Here is a GIF, that shows how the server is booting up in the interface, not much but honest work.

![IMAGE](https://github.com/liebki/MinecraftBlazorSuite/blob/master/MinecraftBlazorSuite/img/console-view.png)
![GIF](https://github.com/liebki/MinecraftBlazorSuite/blob/master/MinecraftBlazorSuite/img/application.gif)

## Contributing

If you'd like to contribute, please do so contributions are welcome, especially if you want new features or support for more versions.

### TODO:
- Auto-Commands
- Optional Command-History
- Auto-Restart
- More advanced player informations

## License

This project is licensed under the AGPL license - see the [LICENSE](https://raw.githubusercontent.com/liebki/MinecraftBlazorSuite/master/MinecraftBlazorSuite/License.txt) file for details.