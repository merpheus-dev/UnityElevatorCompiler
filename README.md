# UnityElevatorCompiler
A plugin that plays elevator music while Unity compiles your code.

## Installation
**2018.1+ users:**

You can use [Unity Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html) to install this package.
Follow this steps:
- Clone the repo
- Copy **com.m3rt32.elevatorcompiler** folder, under to your _Packages_ directory.
- Open **manifest.json** under the Packages folder with a text editor/IDE.![Manifest](https://i.ibb.co/31DxBXr/Screenshot-4.png)
- Add this line under your dependencies(**don't forget to add a comma if you have other packages down below**): 
```"com.m3rt32.elevatorcompiler": "file:com.m3rt32.elevatorcompiler"```
- Example file:
```
{
  "dependencies": {
    "com.unity.2d.spriteshape": "1.0.12-preview.1",
    "com.m3rt32.elevatorcompiler": "file:com.m3rt32.elevatorcompiler",
    "com.unity.ads": "2.3.1"
   }
}
```
- **You are good to go!**


**Earlier Unity versions/Alternative installation**

- Clone the repo
- import the unitypackage
- Make sure everything is under **Editor** folder

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
