# MinecraftBlazorSuite

This is a web-based tool designed to simplify the administration of Minecraft Java servers.


## Features

### Web-Based Interface

- **User-Friendly Web Interface:** Easily manage your Minecraft server through a user-friendly web interface.
- **Technology Stack:** Built with C# .NET Core 7 and Blazor Server technology.


### Web Console

- **Real-time Console:** Monitor your server's console messages in real-time.
- **Execute Commands:** Execute server commands directly from the web console.
- **Customizable Display:** Limit the number of console messages displayed on the console page.
- **Server Control:** Buttons for everything, time change, stopping the server etc.
- **Server Environment Compatibility:** Please note that some functions depend on specific server environments and Minecraft versions like Forge, CraftBukkit, etc.

For more information about forks, environments, and versions see here: [spigotmc.org - article](https://www.spigotmc.org/wiki/what-is-spigot-craftbukkit-bukkit-vanilla-forg "https://www.spigotmc.org/wiki/what-is-spigot-craftbukkit-bukkit-vanilla-forg")


### Player Management

- **Player Management:** Easily manage players on your server.
- **Player List:** View a list of all players currently active on the server.
- **Actions:** Perform actions such as sending messages, changing player gamemodes, killing, kicking, banning, and granting or removing operator rights.
- **Version Compatibility:** Some actions depend on the Minecraft server version (s


### Settings

- **Customize Settings:** Configure application settings through the 'BlazorMinecraftServerSettings.json' file.
- **Adjust Visibility:** Customize the maximum number of visible console messages.
- **Server Configuration:** Set the server JAR file path, Java arguments, and server type/version for future updates.


### Compatibility

- **Tested Versions:** Blazor Minecraft Server Manager has been tested with vanilla, Spigot, and Craftbukkit server versions.
- **Recommended Version:** Only versions 1.17 or higher are supported due to specific console message formats.


## Installation and Usage

### Prerequisites

Before getting started, make sure you have the following dependencies and prerequisites installed:

- **.NET Core 7:** Ensure that .NET Core 7 is installed on your system.
- **Java:** Minecraft servers typically require Java. Verify that you have the required Java version installed for your server software.

Information about java and minecraft: [minecraft.fandom.com - java](https://minecraft.fandom.com/wiki/Tutorials/Update_Java#Why_update? "https://minecraft.fandom.com/wiki/Tutorials/Update_Java#Why_update?")
- Java Edition 1.12 to 1.16.5 requires Java 8 (1.8.0) or newer.
- Java Edition 1.17 to 1.17.1 requires Java 16 or newer.
- Since Java Edition 1.18, Minecraft requires Java 17 or newer.


### Installation

Follow these steps to install and use Blazor Minecraft Server Manager:

1. **Download:** Download the latest release of the wrapper from the 'Releases' section of the repository.

2. **Configuration:** Customize the application settings by editing the 'BlazorMinecraftServerSettings.json' file to match your server setup. This includes specifying the server jar file path, Java arguments, and other relevant configurations.

3. **Launch:** Run the application using .NET Core 7. Navigate to the project directory and execute the appropriate commands to start the server manager.

4. **Access:** Open your web browser and access the web interface by entering the appropriate URL.

- #### The url should be http://server-ip:5000


### Known bugs!

Please be aware that MinecraftBlazorSuite is a work in progress, and there are some known limitations and issues:
- Versions below 1.17: MinecraftBlazorSuite may not fully support versions below 1.17.X due to differences in console messages and styles. You can still use it with lower versions, most features may just not work as expected or at all!


### Troubleshooting

If you encounter any issues during installation or usage, refer to the following troubleshooting tips:

- Q: The MinecraftBlazorSuite console says something like "Error: Unable to access jarfile x.... .jar"
  - A: Is the path really correct, make sure it is.
  
- Q: The MinecraftBlazorSuite console says something like "File/Process is locked x...."
  - A: The server (or another server) is already running, try closing all servers or restart the host to solve this.


## Screenshots / Videos

Here is a GIF, that shows how the server is booting up in the interface, not much but honest work.

![GIF file](https://i.imgur.com/MlhgjvY.gif)


## Contributing

If you'd like to contribute, please do so contributions are welcome!


## License

This project is licensed under the AGPL license - see the [LICENSE](https://raw.githubusercontent.com/liebki/MinecraftBlazorSuite/master/MinecraftBlazorSuite/License.txt) file for details.