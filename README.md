# DateTimePicker
Unity C# implementation of a date and time picker scrollview

# What It Is:

I didn't expect to build a utility to handle activating a scrolling date and time picker. However, having looked around (admittedly not in a lot of depth) and not finding anything that fit my needs I ended up creating my own.

As part of the redevelopment of my ADK46erNow app I needed to be able to tap on a date or time field and have a window or panel appear allowing me to scroll through months, days, years or hours and minutes. And I had to develop it in C# since I'm converting this app from Solar2D using Lua to Unity using C#.

## Features

<ul>
<li>A parent panel with a date scrolling panel and a time scrolling panel.</li>
<li>The parent panel is hidden off scene by default.</li>
<li>A test date field and a test time field.</li>
<li>Tapping the date or time field animates the appearance of the parent panel and enables the respective scroller.</li>
<li>The date scrollview allows scrolling through days, months and years. The time scrollview supports hour and minute scrolling.</li>
<li>The picker panel is dismissed using the "Cancel" (no change to the data in the date or time field, "Clear" (empties the value out of the calling field), or "Save" (updates the calling field with the new date or time) button.</li>
<li>If the date or time field is already populated, then tapping the field will pre-populate the picker.</li>
<li>If the date or time field is blank then the pickers default to the current date and time.</li>
<li>The day scrollview will adjust its days based on the selected month and year (and takes into account leap years).</li>
</ul>

## About The Code

It's free! Download it, use it, modify it!
I developed this as standalone code you can incorporate into your own projects. It's not "pretty", nor is it meant to be. That's up to you. I will be tailoring this code for the new ADK46erNow app and it will be gorgeous!
A PDF document diagramming the code architecture is available and may help to visualize where everything lives and how the various objects communicate with each other.

## Installation

<ul>
<li>Download this repo.</li>
<li>New Project:</li>
-- Create 2D Universal project
-- Import "DateTimePicker.unitypackage"
-- Delete "Sample Scene"
-- Select "DateTimePicker" scene
-- Select "SceneTitle" under "SceneTitleBar"
-- If asked, "Import TMP Essentials"
-- You may need to quit the Unity project and relaunch it to see the text in the scene.
<li>Existing Project:</li>
-- After downloading take the individual resources and add them to your project (scripts, images, scene, prefabs) manually or by importing "DateTimePicker.unitypackage".
-- Run the "DateTimePicker" scene!
-- Update the code as needed for your projects.
</ul>

## License

[MIT](https://choosealicense.com/licenses/mit/)
