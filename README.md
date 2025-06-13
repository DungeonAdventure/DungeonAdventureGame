# Dungeon Adventure - Synopsis

**Total Team Hours:** 423h 32m  
**Team Members:** Alexander Cobble, Vincent Xu, Isaac Ham, Mustafa Abdi  
**Engine:** Unity (2022+)  
**Language:** C#  
**Design Patterns Used:** MVC, Factory, Singleton, Observer  
**Assets:** [Pixel World - Forest and Village](https://assetstore.unity.com/packages/2d/environments/pixel-world-forest-and-village-315158)           
**Save System:** SQLite (JSON prototype attempted)

---

## 🎮 Overview

**Dungeon Adventure** is a Unity-based 2D top-down dungeon crawler. The player controls a hero who must locate the four Pillars of Object-Oriented Programming—**Abstraction, Encapsulation, Inheritance, and Polymorphism**—while navigating hazards and battling monsters.

We applied the MVC architecture and incorporated design patterns like:
- **Singleton** – for global managers (GameController, MusicController, etc.)
- **Factory** – for dynamic hero instantiation
- **Observer-style logic** – for events like pillar collection triggering game logic

---

## 👥 Team Contributions

### **Alexander Cobble** (136h 4m)
- Acted as the team lead and was the go-to person for version control and bug squashing. He was mainly responsible for implementing the dungeon grid system, which tied random scenes to positions on the grid, and created the logic for random pillar spawning and cross-scene tracking. He handled most of the scene management and transition logic and set up persistence for key components like the player, dungeon, music, and camera. He implemented a dynamic hero system that included the Hero Factory and the Warrior, Thief, and Mage subclasses. Finally, he also created two scenes, contributed to the combat logic, and helped the team with technical hurdles like merge conflicts, prefab bugs, and animation setup.

### **Vincent Xu** (111h 48m)
- Built five dungeon scenes (more than the rest of us), handled most of the UI features, including the start menu, player menu, win screen, and in-game music. He also helped connect our SQLite database to Unity, created the story dialog, and contributed to documentation (SRS/UML).

### **Isaac Ham** (95h 40m)
- Created the abstract base classes for DungeonCharacter and Hero, worked on the JSON save/load feature as well as the Health bar UI (which unfortunately wasn’t fully completed), and implemented enemy behaviors and parts of the combat system. He also made two dungeon scenes and helped with testing, documentation (SRS/UML/Presentation), and Unity bugs.  

### **Mustafa Abdi** (80h 00m)
- Focused on the abstract monster class and the child classes skeleton, ogre, and goblin. He created two scenes, made the monster factory, helped initialize the SQLite database, and assisted with the JSON save/load feature (that was not implemented). He built the pillar pickup and persistent pillar tracking system, as well as led the charge on testing the model code. He also updated the UML/SRS documents, added XML comments, and assisted with the presentation.  

---

## ✅ Features Implemented

- Dynamic hero instantiation based on class selection (Warrior, Thief, Mage)
- Class-specific stats, portraits, and animations
- Start menu with name entry and class selection
- Real-time combat system 
- Pillar tracking and collection across scenes
- Procedurally constructed dungeon using scene-grid mapping
- Persistent scene objects using additive scene loading
- Cinemachine-based dynamic camera follow system
- SQLite save/load feature capturing dungeon and player state
- JSON save/load attempt (partially implemented)
- Scene transition system using Unity’s SceneManager with persistent data management
- Modular multi-scene architecture, with grid-based room positioning and randomization
- Player UI (Health bar (not fully implemented), menu system, etc.) 

---

## 🧠 Major Technical Challenges

- **SQLite Integration** – Synchronizing scene/player state with async data loading  
- **Testing** – Unit testing in Unity was difficult due to real-time scene dependencies  
- **Animation Assignment** – Dynamically assigning correct animator and sprites  
- **Version Control** – Resolved multiple Unity scene merge conflicts  
- **Asset Syncing** – Inconsistencies across dev environments early on  

---

## 🌟 Features Beyond Core Requirements

- Persistent saving/loading via SQLite  
- Procedural dungeon via grid-scene mapping  
- Additive scene loading with persistent camera/player  
- Full UI (menus, dialog, name input, class selection)  
- Real-time combat  
- Factory pattern for Hero creation  
- Observer-style event handling (e.g., pillar triggers)  
- Audio and SFX integration  
- Dynamic character animations  
- Third-party asset customization  

---

## 🔮 Future Work Plans

- Finalize combat system & polish enemy AI  
- Fix hit detection and movement issues  
- Load monster data from SQLite into Monster Factory  
- Expand inventory system by adding consumables (potions, buffs, temporary effects)  
- Fully implement UI (health bar, damage numbers, death animations)  
- Support game restarts after win/loss  
- Add minimap or full map in player menu  

---

> _“Find the four pillars. Embrace object-oriented glory.”_
