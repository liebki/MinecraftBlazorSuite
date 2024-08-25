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

- **Server Control:** Quick-controls for things like time, status, weather etc.

- **Server Environment Compatibility:** Please note that some functions depend on specific server environments and Minecraft versions like Forge, CraftBukkit, etc.

For more information about forks, environments, and versions see here: [spigotmc.org - article](https://www.spigotmc.org/wiki/what-is-spigot-craftbukkit-bukkit-vanilla-forg "https://www.spigotmc.org/wiki/what-is-spigot-craftbukkit-bukkit-vanilla-forg")

### Player Management

- **Player Management:** Easily manage players on your server.

- **Player List:** View a list of all players currently active on the server.

- **Actions:** Perform actions such as sending messages, changing player gamemodes, killing, kicking, banning, and granting or removing operator rights.

- **Version Compatibility:** Some actions depend on the Minecraft server version (s

### Management

- **Customize Settings:** Configure application settings through the 'BlazorMinecraftServerSettings.json' file. (Dashboard WIP)

- **Adjust Visibility:** Customize the maximum number of visible console messages.

- **Server Configuration:** Set the server JAR file path, Java arguments, and server type/version for future updates.

### Compatibility

- **Tested Versions:** Blazor Minecraft Server Manager has been tested with vanilla, Spigot, and Craftbukkit server versions.

- **Recommended Version:** Only versions 1.17 or higher are really "supported" due to specific needed message formats.

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

2. **Configuration:** Customize the settings by editing the 'BlazorMinecraftServerSettings.json' file. This includes specifying the server jar file path and Java cmd arguments.

3. **Launch:** Run the application and therefore the Minecraft server, using .NET Core `dotnet [MinecraftBlazorSuite.dll/MinecraftBlazorSuite.exe]`

4. **Access:** Open your web browser and access the web interface by entering the appropriate URL.

- **The url should be http://server-ip:5000**

### Version limitations:

Please be aware that this is work in progress, and there are some issues:

- **I can't support all versions:**

  - They are different in style, the parsing would have to be changed to support every version, I don't have the time and energy to add all versions, I noticed that everything from 1.17 and upwards works so I will stick with this for now.

- **Using older Versions than 1.17:** 

  - You can use theoretically any version, most features will just not work as expected or at all because the messages can't be parsed but the console for example should work at least.

### Troubleshooting

If you encounter any issues during installation or usage, refer to the following troubleshooting tips:

- **Q: The MinecraftBlazorSuite console says something like "Error: Unable to access jarfile x.... .jar"**

  - A: Make sure the path in the settings file is correct, it sounds like it is not!

- **Q: The MinecraftBlazorSuite console says something like "File/Process is locked x...."**

  - A: This sounds like another server is already running, try closing all servers or better just restart the whole system to solve this faster.

## Screenshots / Videos

Here is a GIF, that shows how the server is booting up in the interface, not much but honest work.

![IMAGE](https://github.com/liebki/MinecraftBlazorSuite/blob/master/MinecraftBlazorSuite/img/console-view.png)
![GIF](https://github.com/liebki/MinecraftBlazorSuite/blob/master/MinecraftBlazorSuite/img/players-view.gif)

## Contributing

If you'd like to contribute, please do so contributions are welcome, especially if you want new features or support for more versions.

## License

This project is licensed under the AGPL license - see the [LICENSE](https://raw.githubusercontent.com/liebki/MinecraftBlazorSuite/master/MinecraftBlazorSuite/License.txt) file for details.