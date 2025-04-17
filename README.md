# AngryBots for Unity 6
![A thumbnail of AngryBots showing the player character shooting two enemies.](https://raw.githubusercontent.com/BobyStar/AngryBots-Unity6/d9c6bc2fce40b847024ae6e73ea0d7a8c24ddc61/AngryBots-Unity6_Thumbnail.png)
The original Unity AngryBots demo project upgraded to Unity 6. This project was the default shipped demo with Unity 3.4, the Unity 4.x version was used with Unity 4.5.5 for reference.

# Run the Project:
1. Download or clone the project and open the containing folder with Unity 6000 or higher.
2. Locate and Open the `Default.unity` scene file in the `Scenes` folder.
3. Enter Play Mode.

# Major Changes
* Js Unity Scripts were converted to C# Mono Scripts (Reference Js Unity Scripts are included in project files).
* Legacy GUI was converted to Unity UI, including the "Lowest" Quality setting.
* Particle Renderers and Particle Emitter Systems were converted to use Particle Systems.
* Shaders were fixed to include shadow pass for depth effects used in Highest Quality setting.
* Re-baked lighting for main `AngryBots.unity` scene.

# Notes:
* Building the project has not been tested.
* Some models may report legacy import warnings, which can be ignored.
* Rendering with Forward BiRP, which is supposedly being deprecated for "Unified Rendering" in a later Unity 6 or upcoming "Unity 7" editor by Unity.
* Previous versions of Unity may not be supported due to API changes made for Unity 6 (e.g. `Rigidbody.velocity` => `Rigidbody.linearVelocity`).

Special thanks to Unity's [unityscript2csharp](https://github.com/Unity-Technologies/unityscript2csharp) repo.

The Angry Bots name and logo © copyright Unity Technologies A/S.
