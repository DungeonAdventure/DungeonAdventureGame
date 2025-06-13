# Dungeon Adventure - Synopsis

**Total Team Hours:** 423h 32m  
**Team Members:** Alexander Cobble, Vincent Xu, Isaac Ham, Mustafa Abdi  
**Engine:** Unity (2022+)  
**Language:** C#  
**Design Patterns Used:** MVC, Factory, Singleton, Observer  
**Assets:** Forest Pixel Land  
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
Team lead and version control point person.  
- Built the dungeon grid system and scene management logic  
- Created random pillar spawning with cross-scene tracking  
- Developed persistence logic for player, dungeon, music, and camera  
- Implemented dynamic hero system and Hero Factory (Warrior, Thief, Mage)  
- Contributed to combat, prefab fixes, and animation setup  

### **Vincent Xu** (111h 48m)
- Created 5 dungeon scenes  
- Built UI (start menu, win screen, player menu)  
- Integrated SQLite with Unity  
- Authored story dialog and documentation (SRS/UML)  

### **Isaac Ham** (95h 40m)
- Developed abstract `DungeonCharacter` and `Hero` classes  
- Worked on JSON save/load and Health bar UI  
- Designed enemy behavior and partial combat system  
- Helped with scenes, testing, and documentation  

### **Mustafa Abdi** (80h 00m)
- Created abstract `Monster` class and skeleton, ogre, goblin subclasses  
- Built the Monster Factory and assisted with SQLite setup  
- Developed pillar tracking and persistence  
- Focused on model testing, documentation, and XML comments  

---

## ✅ Features Implemented

- 🎭 **Dynamic Hero Instantiation** – Warrior, Thief, Mage with unique stats/animations  
- 🏁 **Start Menu** – Name entry and class selection  
- ⚔️ **Real-Time Combat System**  
- 🏛️ **Pillar Collection & Tracking** – across dungeon scenes  
- 🧭 **Procedural Dungeon** – scene-grid mapping  
- 🎥 **Cinemachine Camera System**  
- 💾 **SQLite Save/Load System** – player/dungeon state  
- 🧪 **Partial JSON Save System** (prototype)  
- 🔁 **Scene Transition System** – with persistent data  
- 🧱 **Multi-Scene Architecture** – grid-based room layout  
- 🩸 **UI Elements** – health bar (WIP), menus, dialog  
- 🕹️ **Controller Support and Modular Prefabs**  

---

## 🧠 Major Technical Challenges

- **SQLite Integration** – Synchronizing scene/player state with async data loading  
- **Testing** – Unit testing in Unity was difficult due to real-time scene dependencies  
- **Animation Assignment** – Dynamically assigning correct animator and sprites  
- **Version Control** – Resolved multiple Unity scene merge conflicts  
- **Asset Syncing** – Inconsistencies across dev environments early on  

---

## 🌟 Extra Credit Features

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
- Add consumables (potions, buffs, temporary effects)  
- Fully implement UI (health bar, damage numbers, death animations)  
- Support game restarts after win/loss  
- Add minimap or full map in player menu  

---

> _“Find the four pillars. Embrace object-oriented glory.”_
