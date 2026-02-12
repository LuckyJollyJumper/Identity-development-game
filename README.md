# Identity-development-game

Unity prototype game made to help adolescents in Dutch practical schools with identity development for School courses and leisure time activities.

The current prototype is not a full game and merely demonstrates how an indentity development game could look like for the target audience. The game includes only the beginning of the quests and story and a few mini-games. The game can be played at: ...

## Documentation

All the information below is for developers who wish to continue this project.

### Scenes

The ScenesManager uses the enum `scenes` in order as in `files > Build Profiles > Scene List`.

The main scene used in the game is the `SchoolMap` scene and is presumed to be the first scene to be launched. This scene also sets up the Gamemanager that will be persistent throughout all scenes.

### Managers

The GameManager: 

## TODO

#### Needed
- Make the gamemenager and its questprogressions until the first mini-game
- Make player move with physics instead of transform
- Make the charactercreation
- More of the Persona games
    - Make the prediction of these personas
- Create sound for speaking the text

#### Optimisations
- Make puzzle mini-game such that you input an image and a size and the game handles the rest

#### Aesthetics
- Update all UI
- Animations to UI (lean tween)?
- Create the outside world with border
    - Create sportfields
    - Create roads and neighbouring buildings
    - Create a gate around the school
    - Create a carpark and bikestall
    - Hide a special activity or items to find
- Add typing effect for popups and the NPC's
- Player animations

#### Optional
- Create the homequests interactableobject
- Create github issues instead of this file
- Instantiate from serializefield all prefabs instead of Resources folder



## Questions

Be sure to contact me for any further questions or difficulties at https://github.com/LuckyJollyJumper or using my email luc.schrau@gmail.com.