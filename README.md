## Lemmings Paintball Level Editor

This is an incomplete but working level editor for the 1996 game Lemmings Paintball.

The isometric map editor provides a WYSIWYG interface. The editor backs up then edits the Lemmings Paintball `PBAIMOG.VSR` resource file to include your level(s).

## Basics

The status bar at the bottom gives some usage advice.

![Lemball Editor User Interface](docs/ui.png?raw=true "Lemball Editor User Interface")

### Terrain Editor

- Scroll through the level with the arrows keys
- Use the mouse wheel to change the elevation
- Drag to select multiple tiles
- Select `Tiles` in the side bar. Select a tile and click to paste that on the map

### Placing Objects

- Select `Objects` in the side bar. Click on an object and left click on the map to place it
- Click and drag objects to move them
- Right click on objects to customise them

### Testing Levels

- Use the `Compile` dropdown to test your level within the game

## Status

I implemented this some years back in .NET Framework 2.0 and WinForms when first learning C# and OOP, so the codebase could do with some modernising and refactoring. In 2024 I fixed some issues and updated it to .NET Framework 4.8 (later versions of .NET will need work to upgrade to).

- ✔ Create new levels
- ✔ Grass level theme (but not all tiles)
- ✔ Connect switches to gates
- ❌ Load and modify existing levels
- ❌ Lego, Space, Snow themes
- ❌ Undo/redo
