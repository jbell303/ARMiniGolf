# ARMiniGolf
Mini Golf Project with ARKit and Unity

This app was built for iPhone using Unity's ARKit. This is a quick demo project I built in a couple weeks as a concept for my Magic Leap
grant application. 

## Requirements
* Unity v2017.4+
* Apple Xcode 10.0+ with latest iOS SDK that contains ARKit Framework
* Apple iOS device that supports ARKit (iPhone 6S or later, iPad (2017) or later)
* Apple iOS 12+ installed on device

see [this](https://bitbucket.org/Unity-Technologies/unity-arkit-plugin/src/default/) post for detailed requirements.


## Installation
Clone this repo into your Unity working directory and open the project in Unity. Build and Run the project using iOS build settings. 
Note: A current developer account is needed to install this app from Xcode onto an iOS device. You may also need Blender to view the model
files. This project will not work on ARKit Remote.

## Usage
Run the app. Accept the camera usage request and aim your camera at a flat, well lit surface. The ARKit point cloud particle effect is remains
in the game as a guide to help the user find a suitable playing surface. This game can be played inside or outdoors. Follow the on-screen
play instructions to play the game. The hit direction is controlled by the camera. Purple chevrons should appear to assist with aiming.
Stroke power is controlled by touching and dragging up the screen. Release touch to hit the ball. The game will end if the ball goes out of
bounds or in the hole. **See if you can get a hole-in-one!**

## Limitations
The graphics are very basic and there is no sound.

![alt text](https://github.com/jbell303/ARMiniGolf/blob/master/Images/IMG_0030%20(Small).png)
![alt text](https://github.com/jbell303/ARMiniGolf/blob/master/Images/IMG_0029%20(Small).png)
