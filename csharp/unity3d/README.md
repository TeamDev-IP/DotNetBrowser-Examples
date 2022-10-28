# Using [DotNetBrowser](https://www.teamdev.com/dotnetbrowser) with [Unity3D](https://unity3d.com)

[DotNetBrowser](https://www.teamdev.com/dotnetbrowser) is a .NET library which allows embedding Chromium into .NET applications to load and display web pages built with **HTML5**, **CSS3**, **JavaScript**, etc.
  
This example project has two scenes that demonstrate how to use DotNetBrowser with Unity3D to show the browser in the 3D scene on surfaces of geometry primitives.

1. "DnbSimple" folder contains content for "DotNetBrowserScene.unity" scene, where you can see how to use a browser on base geometry primitives in the simplest way.
2. "DnbFps" folder contains content for the modified Unity "[Fps Microgame](https://learn.unity.com/project/fps-template)" project. You can see how the browser could be implemented there.

### How to run

The folder of the Unity3D project is **DotNetBrowser_Unity**. It should be selected in [**Unity Hub**](https://unity.com/unity-hub) as [**Unity3D**](https://unity3d.com) project to open.
  
Please note that this project uses a dedicated local package to automate pulling and updating DotNetBrowser libraries. You can see it referenced in _DotNetBrowser_Unity\Packages\packages-lock.json_ and _DotNetBrowser_Unity\Packages\manifest.json_ as **com.teamdev.dotnetbrowser.deps** package. The package source code is located in a **Dependencies** folder.

The package will check and download the latest version of **DotNetBrowser** libraries on loading the Unity project. The same check is performed on recompiling the Unity project scripts.

### How it works
The main idea consists of four classes located in _DotNetBrowser_Unity\Assets\DnbSimple\Scripts\\_:

1. `BrowserScript` - encapsulates the logic to control the browser lifecycle. It creates `IEngine` and `IBrowser` and disposes of them at the end of the work. Also provides the picture of the loaded web page as an instance of `Bitmap`.
2. `BrowserViewScript` - this script requires a `BrowserScript` instance to work. Updates texture of geometry primitive with `BrowserScript.Bitmap` pixel data. Does forwarding of input to the browser and focus management. Several `BrowserViewScript` can use the same `BrowserScript` to simultaneously show the same web page from the same browser on their attached primitives with separated input forwarding.
3. `MouseHelper` - contains logic that is used to redirect mouse input.
4. `KeyboardHelper` - contains logic that is used to redirect keyboard input.

You can use `BrowserViewScript` directly if you need to display the web page and perform input handling on a static mesh. But it will look better to bring some action to the scene and a bit of classic browser UI. `CubeScript`, `PlaneScript` and `SphereScript` are `BrowserViewScript` subclasses that add a some additional behavior directly to according primitives. `PlaneScript` has a little bit of classic browser UI. It allows typing a URL and performing back/forward navigation. `SphereScript` and `CubeScript` just add a simple rotation to bring some action to the scene.

### Scene content
Both examples have their own content. However, _DnbSimple_ has the base content and _DnbFps_ uses it as a basement.

#### DnbSimple and DotNetBrowserScene.unity
The scene objects are geometry primitives, UI elements, and empty objects for `BrowserScript`(s):

![image](https://user-images.githubusercontent.com/85240195/196160746-ef5e6c03-d9c6-4078-9498-0540e70d2fe7.png)
  
The "**Browser 0**" and "**Browser 1**" are empty objects where we attach `BrowserScript` scripts. They are empty because they don't require visualization, they are needed to control the browser lifecycle in attached `BrowserScript`(s).
  
![image](https://user-images.githubusercontent.com/85240195/196161142-102da254-2679-4a2c-8c23-7dd767afc024.png)
  
**Primitives** are primitives with standard material to show HTML pages. It is **Plane**, **Cube** and **Sphere**. They have the corresponding script component in **Inspector** panel. It is `PlaneScript`, `CubeScript` and `SphereScript` accordingly.
Here is how the attached `CubeScript` looks, for example:
  
![image](https://user-images.githubusercontent.com/85240195/166907252-2d9eff6a-7d6d-4e4a-a8e0-0e6de4eb42af.png)
  
The attached `SphereScript` looks similar in **Inspector** panel of **Sphere** object. However, the `PlaneScript` has additional parameters to attach simple UI scene objects: 
  
![image](https://user-images.githubusercontent.com/85240195/196161396-cc972c22-0155-4cac-b1b6-7798d472e313.png)
  
We bind certain scene objects to their respective `PlaneScript` fields via the Inspector panel. The "**Browser Game Object**" field is common for every primitive script.

![image](https://user-images.githubusercontent.com/85240195/166907731-36b88e85-e722-4af4-985c-94512f71f05d.png)

This field is set to the empty scene object that has attached `BrowserScript`. In this way, we specify what browser will be shown by this primitive.

Also, you can see an **EventSystem** and **Canvas** with UI elements in the scene tree. The UI elements are bound to `PlaneScript`(see the `PlaneScript` component picture above). And there are default **Main Camera** and **Directional Light** objects for the normal rendering process.

#### DnbFps and MainScene
DnbFps is a Unity Fps Microgame with an implemented custom HTML menu and a design concept of the game chat. It has subclasses `MenuBrowserScript` and `MenuViewScript` that extend `BrowserViewScript` and allow it to affect the game from the HTML page of the menu.

Necessary scene objects are geometry primitives, UI elements, and empty objects for `BrowserScript` and `MenuBrowserScript`:
![image](https://user-images.githubusercontent.com/85240195/196393367-821b32a7-063c-48d6-9e1c-cf815471e4fd.png)

The "ChatBrowser" and "MenuBrowser" are empty objects where we attach `BrowserScript` and `MenuBrowserScript` respectively. They are empty because they don't require a visualization and needed to control the browser lifecycle in the attached script. Also, you can see _Html UI Manager_ object there which we use to assign _Html UI Manager_ script where we handle a Tab key.

To show the menu and the chat, we use the Raw Image elements of Canvas:

![image](https://user-images.githubusercontent.com/85240195/196395032-8a4c0a6f-0187-4860-b997-a9cef7ae8351.png)

There are two fields for `MenuViewScript`:

![image](https://user-images.githubusercontent.com/85240195/196397478-d4b329fb-577f-435f-b03f-d51080b1b090.png)

The "Browser Game Object" is an object that has attached `BrowserScript`. In this way, we specify what browser will be shown by this Raw Image element.
The "Menu Manager Game Object" is an object that has `HtmlUIManager` script attached. This script controls the activation and deactivation of the menu.
