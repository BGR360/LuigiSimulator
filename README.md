# Luigi Simulator: VEX Skyrise Team 2360N
A 3D Simulation of our 2015 VEX Skyrise Robot, Luigi. Created using Unity Game Engine, Blender, and Autodesk Inventor.

## Demo Video

[![Simulator Demo](http://i.imgur.com/zslzdqM.gif)](http://www.youtube.com/watch?v=s_RUZVgpZfw)

http://www.youtube.com/watch?v=s_RUZVgpZfw

## Overview

I made this simulator for the 2015 VEX National competition in Council Bluffs, Iowa. It is a simulator of Team 2360N's robot, dubbed "Luigi."

The simulator was designed to be controlled using two Xbox 360 controllers, similar to how Luigi was actually controlled on the field with two VEX controllers. The controls in the simulator work identically to the controls of the real robot. I used the [XInputDotNet library](https://github.com/speps/XInputDotNet) to be able to interface with the Xbox 360 controllers.

Since this is a Unity project, the actual code that I wrote can be found in the [Scripts folder](Assets/Scripts). Keep in mind that some of these files were auto-generated and hence not written by me.

## The Robot

This is Luigi. He is pleased to meet you.

<img src="http://i.imgur.com/G0jQd7v.jpg" width="200"> <img src="http://i.imgur.com/5DJb3k1.jpg" width="200"> <img src="http://i.imgur.com/1NW5x03.jpg" width="200">

<img src="http://i.imgur.com/l7hxam4.jpg" width="200"> <img src="http://i.imgur.com/p1FfoIs.jpg" width="200"> <img src="http://i.imgur.com/aAVsDVF.jpg" width="200">

### The Auton Killer

When competition strikes, the 10-point bonus earned from winning the 15-second autonomous period can often be the deciding factor in a match. The Autonomous was where our bot really shone. The swinging arm design and vertical elevator lift allowed us to "park and score" skyrise pins for a total of **12 points** in the autonomous period.

#### Autonomous Video Demonstration

Here is the robot performing an 8-point autonomous (2 skyrise pins):

[![Auton Video](http://img.youtube.com/vi/D42NXZmOWQk/0.jpg)](http://www.youtube.com/watch?v=D42NXZmOWQk)

http://www.youtube.com/watch?v=D42NXZmOWQk

I do not have a video of the bot completing a full 12-point autonomous (2 skyrise pins + 1 skyrise block), but here is an animation of it that I made in Blender:

[![Full Auton Demo](http://i.imgur.com/Ztz7Bcd.gif)](http://www.youtube.com/watch?v=h1dJypvh4AM)

http://www.youtube.com/watch?v=h1dJypvh4AM



## Modeling the Robot

To create the model of the robot, I took *many* reference pictures of it and created a very rough model of the bot in Autodesk Inventor. 
  
<p align="center">
  <img src="http://i.imgur.com/944g6OA.png" width="250"> <img src="http://i.imgur.com/QyqOb6q.png" width="250"> <img src="http://i.imgur.com/7U5WqDk.png" width="250"> 
</p>


I exported the Inventor model to an STL file and imported it into Blender because, for SOME REASON, Autodesk likes to export files with MILLIONS of vertices!! Not very good for a 30 fps robot simulator...

<p align="center">
  <img src="http://i.imgur.com/sdQqfC0.png" width="300">
</p>

So I had to meticulously remodel the whole robot in Blender using basic polygons in order to reduce the vertex count. That was... fun...

<p align="center">
  <img src="http://i.imgur.com/9Fa1bty.png" width="250"> <img src="http://i.imgur.com/yJwj83T.png" width="250"> <img src="http://i.imgur.com/k27wcuT.png" width="250"> 
</p>

<p align="center">
  <img src="http://i.imgur.com/A5K3BxO.png" height="250"> <img src="http://i.imgur.com/YwNAbA0.png" height="250">
</p>

Once I had remodeled the bot, I performed the same process for the game field, the skyrise pins, and the cubes. I imported all of the Blender assets into Unity and began programming the simulator.

<p align="center">
  <img src="http://i.imgur.com/uWXOCkb.png" width="400">
</p>

<p align="center">
  <img src="http://i.imgur.com/i2ARfWk.png" width="400"> <img src="http://i.imgur.com/hc5jJxh.png" width="400">
</p>
