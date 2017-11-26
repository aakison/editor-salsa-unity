# editor-salsa-unity
Editor extensions to add handy features to menu, inspector and hierarchy.

## Installation
This repo is designed to be used as a submodule of an existing Unity3D repo.  E.g.

```
    git submodule add https://github.com/aakison/editor-salsa-unity.git Assets/EditorSalsa
```

There are no demo scenes or resources in this repo, just the required files so that your solution is not polluted with unnecessary files.

## Feature: Open Application Folder

Ever spend minutes tracking down those screenshots and database files?  Now it's seconds, saving the economy tens of cents annually!

A menu is installed at Tools..Open Application Folder which will open the persistent data path of the application.

## Feature: Delete Empty Directories

When using Git and Unity, sharing can be complicated by Unity adding a .meta file for each directory but Git not saving the actual directory.
Empty directories will end up causing a checkout by another team member to remove the .meta file as the directory has gone missing.

A menu is installed at Tools..Delete Empty Directories which will delete these unused directories, useful for just before a Git commit.

## Feature: ReadOnlyInInspector attribute

Decorate your fields with `[ReadOnlyInInspector]` and the field simply becomes read only.
Useful for game objects that use fields to inform the user about state but that don't allow the developer to change in the inspector.

## Feature: Prefab Highlight in Hierarchy

The boundaries of prefabs are not always easy to tell once they're dragged into the hierarchy.  
This add-in will highlight the prefabs and display their name right aligned inside the bounding box.
When prefabs become disconnected, it changes the display to show that it is disconnected and allows the user to easily attach back to the prefab.
More importantly, there is also a button to permanantly disconnect the game object from the prefab which can't be done in the built in inspector.


