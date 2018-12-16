# A Lost Robot
Moray Game Jam 2017 - A Game About A Lost Robot
Requires:
 - Unity 2017.4.17f1
 - Wwise 2018.1.4.6807
## Code
 - Josh Hale
 - Stewart McCready
## Audio
 - Shane Ellis
## Art
 - Rebecca Roe
 - Jacob Naylor

# Wwise Setup

You will need to point it towards the correct version of Wwise
 - Edit -> Wwise Settings...
 - Modify `Wwise Installation Path` to point at your Wwise 2018.1.4.6807 install directory

Once hooked up, open the Wwise Picker `Window -> Wwise Picker` click refresh project, and then generate Sound banks. Now the project will be ready to play in editor. When you build our already setup pre/post hooks will import the sound banks for the correct platforms. We currently support iOS, Android, OSX, and Windows.

# Debug Ui
Whilst in the game press F12 or touch 5 fingers on the screen to bring up the debug Ui. This gives some handy buttons for fake winning etc.

## Note
This repo uses [git-lfs](https://git-lfs.github.com/) for large files therefore Githubs Download Zip button won't produce a usable download. 
If you are using 'Github for windows' or 'Gitkraken' download via cloning will work fine and pull all the required files from the github lfs server.
There are precompiled versions of the game in the releases tab. 

