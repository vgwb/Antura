CAT UPGRADE NOTES

This documents highlights the changes implemented with the Cat Update.

# Useful links
[Cat reward system definition](https://docs.google.com/spreadsheets/d/1GlJdBhl66x5CB0Lgt5N3O_JGkGwTveCe5PQLl-FAHLc)

[Antura integration step by step](https://docs.google.com/document/d/1kg49N5FazWJmktR4iAXE2HVI2Rong0s3jsyCMBkPXIk)

# General Changes
The Cat update introduces the possibility for Antura to be replaced with another pet in all the game scenes.
Once you unlock the cat, you can select the pet to play with inside the Antura Space by clicking on the pet in the background.
Each pet has its own rewards.

Here are the general changes to the system
- Replaced Antura with a special "Antur Pet Switcher" Prefab that loads the correct pet
- Flow changed to incorporate the new pet
- Reward system redone to support shopping of props and textures
- Added support for more advanced characters

# Antura Pet Switcher

The new Antura prefab has logic to select and spawn the correct pet based on the context, and functions as an entry point to the pet's capabilities.
It supports loading the current selected pet, or forcing showing a pet (for example, if we want to show both pets at once).
All scenes containing Antura now use this new prefab.
An enumerator named AnturaPetType defines what pet to use.

# Flow changes
All gifts are now sent as biscuits.
The Antura Space tutorial now has new steps that show how to buy items. It now has these entry points:

- Before the first play session: Tutorial on how to Shop Items
- After the first play session: Tutorial on how to Shop Decorations
- After the first assessment: Tutorial on how to take photos
- After the second assessment: New pet is introduced

## Reward System Redone
The new Reward System allows players to gain biscuits and spend them to unlock new props. The major changes are as follows:

- Players are now rewarded with biscuits only, instead of props
- Props can be purchased in a shop inside the Antura Space, and they show item costs
- Your bones are shown separately in Antura Space (no more tied to the Decorations)
- Customization is now less hardcoded, allowing separate item classes with variable numbers of props
- Ears are now merged into a single shop item
- Depending on the selected Pet, the biscuit icon changes throughout the application

For retrocompatibility, if a Reward was already unlocked, it is kept as unlocked.Rewards with a cost of 0 (zero) count as already unlocked.

# Advanced characters support
The update introduces some upgrades to the character controller.

- An updated shader to handle multiple textured materials instead of only one 
- Support for Skinned Mesh Renderers, allowing pets to employ aniamted props.
- A new glass material to improve visual fidelity

# Data Entry Changes
Each pet now has a separate configuration table. The item cost is added to each entry in the table.

[Antura Dog Rewards configuration](https://docs.google.com/spreadsheets/d/19noVtlaRO93bnSVEHkFY0vFdJgRpwLeQQLmMyb1yXxo)

[Antura Cat Rewards configuration](https://docs.google.com/spreadsheets/d/14Bl1XBLUx6oFLnI2XYfZJRcLE4Od_FT1tm48bQ87zh4)

# Saved Data Changes

Profiles now also contain a PetData serializable structure that currently holds the last used Pet, and whether the Cat is unlocked or not.
