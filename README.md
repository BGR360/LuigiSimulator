# LuigiSimulator
A 3D Simulation of our 2015 VEX Skyrise Robot, Luigi. Created using Unity Game Engine, Blender, and Autodesk Inventor.

## Demo Video

[![Simulator Demo](http://img.youtube.com/vi/s_RUZVgpZfw/0.jpg)](http://www.youtube.com/watch?v=s_RUZVgpZfw)

## Overview

I made this simulator for the 2015 VEX National competition in Council Bluffs, Iowa. It is a simulator of Team 2360N's robot, dubbed "Luigi."

The simulator was designed to be controlled using two Xbox 360 controllers, similar to how Luigi was actually controlled on the field with two VEX controllers. The controls in the simulator work identically to the controls of the real robot.

## Modeling the Robot

To create the model of the robot, I took *many* reference pictures of it and created a very rough model of the bot in Autodesk Inventor. I exported the Inventor model to an STL file and imported it into Blender because, for SOME REASON, Autodesk likes to export files with MILLIONS of vertices!! Not very good for a 30 fps robot simulator...

So I had to meticulously remodel the whole robot in Blender using basic polygons in order to reduce the vertex count. That was... fun...

Once I had remodeled the bot, I performed the same process for the game field, the skyrise pins, and the cubes. I imported all of the Blender assets into Unity and began programming the simulator.
