# MessedUp

### Disclosure and Safety Notice

This project was developed as part of a college dissertation investigating the use of system-based mechanics within videogame narrative. As a result, the prototype contains functionality that intentionally interacts with the Windows operating system outside of the normal game window.

The project includes mechanics capable of:

- Reading system information such as the computer name, system time, audio settings, and connected devices.
- Creating, opening, modifying, and deleting files.
- Opening external applications and URLs.
- Detecting and changing the state of the game window.
- Changing the desktop wallpaper and rearranging desktop icons.
- Displaying Windows system messages.
- Forcefully closing the game application.
- Triggering a Windows system shutdown.

An exact list of these mechanics is included as an image labled `Mechanics.png` in the assets folder.

These mechanics were originally tested in a controlled Windows environment using prepared dummy files rather than participants' personal data. Potentially disruptive system-level functionality was also placed behind a simulation mode during development so that it would not be executed unintentionally.

### Before Running the Project

It is strongly recommended that you:

- Review the system-interaction code before enabling any non-simulated functionality.
- Keep simulation mode enabled unless you specifically intend to test the system-level mechanics.
- Use a test Windows account, virtual machine, or other controlled environment.
- Do not point file-manipulation functionality toward personal or important files.
- Save any open work before testing the forced termination or shutdown mechanics.

This prototype was created for research and demonstration purposes. The system-facing mechanics are intentionally intrusive because their effect on player immersion, comfort, perceived risk, and expectations of developer communication formed part of the study.

### Project Setup

Included in the `Assets` folder is a directory called `Personal Photos`. This folder must be placed in the user's `Documents/` directory for the game to recognise it correctly.

Additionally, the hard-coded value for Simulation Mode can be changed in:

`Assets/Scripts/Meta Systems/System-Interractions/SI Main.cs`

This setting affects the game's ability to perform dangerous system-level interactions, including creating and deleting files and shutting down the PC environment.

It is recommended that Simulation Mode remains enabled unless these mechanics are being tested in a controlled environment.

Use the project at your own discretion.