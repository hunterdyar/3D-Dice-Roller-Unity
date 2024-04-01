# About
This is two projects...
## 3D Dice roller for Unity
The goal is to be able to call for a dice roll, have it happen in whatever visually interesting way, and get back a result. It will dynamically load a new additive scene which renders it's own camera on top of the rest. It does not have to work this way, and can be easily modified. The dice can stand alone. Adding a grabbable from some VR interaction toolkit, for example, wouldn't be much work.

## Dice Notation Parser
Written as pure C# - nothing about it requires unity. The parser could be ripped out (see the 'rollCodeParser' folder) and used elsewhere.
It supports common "xdy+m" syntax, and can chain multiple comma or space separated rolls into a sequence of root expressions (the rolls, the modifiers).

## Licenses
ses [Low Poly 3D Dice](https://opengameart.org/content/low-poly-3d-dice) by Facehunter2003, released under CC0 (public domain) license.
