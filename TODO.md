# Learn with Antura TODO
> here is a list of bugs and improvements


# Polish quests

## PL 00

**low**

- [ ] Check that some final stars remain black when appropriate.

## PL 01 Warsaw

**medium**

- [ ] show the tram target before the destination target when a tram is required.

**low**

- [ ] Review the characters' birth years.
- [ ] Reduce the walking distance.
- [ ] Make the stadium puzzle easier.
- [ ] Resize the LL tube hats; they are too large.
- [x] Replace/update the zloty coins.

## PL 02

**high**

- [x] cathedral key: the player must not be able to talk repeatedly and fetch the key more than once.

**medium**

- [x] make it clear that the player must talk to the LL to receive the cathedral key.
- [x] Make the last Old Town book easier to find.
- [ ] improve the map zoom.

**low**

- [ ] Put a cookie inside the empty chest.
- [ ] Add an effect to notes so they are easier to see from a distance.

## PL 03 Odra River

**high**

- [x] bridge terrain gap and make the bridge connect correctly to the ground.
- [ ] cat behaviour during tasks: stop it walking into the river and respawning.
- [x] danger zone: add a 3-second delay before respawning.

**medium**

- [ ] the minimap must rotate correctly and not force the player to walk backwards.
- [ ] make distant tasks visible/noticed by children.
- [ ] move the first ship page to a more visible position, such as the stairs.

**low**

- [ ] Improve the papers prefab: visible from below, more 3D, and with some colour.
- [ ] Move the papers closer to the stairs.
- [ ] Allow walking during tasks where appropriate.
- [ ] Check the Polish number audio rendering.
- [ ] Correct “sto dwa” to “sto dwie”.
- [ ] Correct the pronunciation of “sto pięćdziesięciu”.
- [ ] Fix wrong names/pronunciation in the memory game.
- [ ] Fix “Tumski” being pronounced as “chumski”.
- [ ] Fix “most Grunwaldzki” being pronounced partly in English.
- [ ] Replace “więc może się ukryć przed krasnoludami” with “Aby mogła ukryć się przed krasnalami”.
- [ ] Fix the pronunciation of “rower” in the matching game.
- [ ] Replace the old/unhelpful word “pylon” where it is used.
- [ ] Replace “latający” with “pływający” in “Nawet krasnoludy pokochałyby pływający dom!”.
- [ ] Translate the untranslated corner text and make that task easier to finish.
- [ ] Consider replacing “łódź dla ludzi” with “łódź turystyczna”.

## PL 04 Zoo

**high**

- [x] prevent every minigame from being completed by pressing X, exiting, or failing twice.
- [x] verify minigame results before allowing the quest to continue.

**low**

- [ ] Fix the identified missing/incorrect dialogue and the remaining unspecified language issue.
- [ ] Pronounce “zoo” in Polish, not English.
- [ ] When referring to the flag, use “ją” instead of “to”.
- [ ] Complete the chimpanzee dialogue: “Czy potrafisz zgadnąć, na które drzewo muszę się wspiąć, aby dostać…”.
- [ ] Replace “pomarańczowa roślina” with “drzewo pomarańczowe”.
- [ ] Correct “FLAGĘ” to “FLAGI”.
- [ ] Correct “FLAG” to “FLAGI”.
- [ ] Correct “DZIEWIĘĆDZIĘCIU” to “DZIEWIĘĆDZIESIĘCIU”.
- [ ] Pronounce “lew” in Polish, not English.
- [ ] Correct the penguin's “PTAKIEM” pronunciation.
- [ ] Correct “FLAGI” being pronounced as “FLAGA”.
- [ ] Fix the zoo manager's flag pronunciation.
- [ ] Replace “gdyby wszystkie zwierzęta są niewinne” with the correct “jeżeli…” construction.

## PL 06 Toruń Market

**high**

- [x] activity flow: after an activity is completed, do not repeat it; go directly to the next point.

**medium**

- [ ] fix the top card positions in Activity Match.
- [ ] improve the highlight of selected cards, frame, and sign in Activity Match.
- [ ] fix the ESC key behaviour so it pauses instead of respawning.
- [ ] add SOS actions to respawn and show the latest task.

**low**

- [x] Update the fountain LL dialogue to say the word “water” if that is the intended clue.

# French quests

## FR 00
https://docs.google.com/document/d/1U0FWqUzAjLKTZ_R69nXcr_R93yQoyMKNnYycK4nP7bU/edit?tab=t.110v11ailozl

## FR 02
https://docs.google.com/document/d/1U0FWqUzAjLKTZ_R69nXcr_R93yQoyMKNnYycK4nP7bU/edit?tab=t.ovadpgfgytv

## FR 03
https://docs.google.com/document/d/1U0FWqUzAjLKTZ_R69nXcr_R93yQoyMKNnYycK4nP7bU/edit?tab=t.7crlcfe41i6t

## FR 09
https://docs.google.com/document/d/1U0FWqUzAjLKTZ_R69nXcr_R93yQoyMKNnYycK4nP7bU/edit?tab=t.xevfzzaiz9y1

## FR 10
https://docs.google.com/document/d/1U0FWqUzAjLKTZ_R69nXcr_R93yQoyMKNnYycK4nP7bU/edit?tab=t.vli2rcr99me4


# Docs

- [ ] Put the Italian quests in the document history.

# Bugs

- [ ] Opening card skips dialogue

# Maintenance
- [ ] Remove all `Assets/_discover/Prefabs/3D Common (DO NOT USE)` assets.
- [ ] remove all colormap materials

# Improvements
## map and inventory
- [x] Add 2D colours to the globe.
- [x] Create the 2D coloured map in the old cartoon style.
- [x] Remove Antura from the map.
- [x] Remove Italy, Spain, Germany, and the North Pole from the map.
- [x] Keep only the labels FRANCE and POLAND.
- [x] Fix the Polish town pins.

- [ ] Add the Antura space with the cat to the map.
- [ ] Add the book icon.

## Discover UI
- [ ] The Module Selector direction on PC (invert it if required). #medium
- [ ] Create an in-game alert system for player warnings and important feedback.
- [ ] Add a reusable 3D toast message that appears near the player or in the gameplay HUD.
- [ ] Support configurable message text, duration, position, and alert style.
- [ ] Replace the temporary killzone `WARNING` log with the in-game alert system.
- [ ] respawn button + restart quest
- [ ] New Target System, with circular indicators

## Dialogues UI
- [ ] add Baloon type dialogue when speaking a simple WORD #top

- [ ] Add the `interactable_icon` command to the narrative engine.
- [ ] Support exclamation-point and question-mark interactable icons.
- [ ] Set the icon from `interactable.status`.
- [ ] Add the related interactable icon animation.

- [ ] show a Knowledge as small graph tree?

- [ ] Make `inventory_add` trigger the tagged collect-item task.
- [ ] Action to activate animations through Yarn
- [ ] disable interactable icon


## Discover Card Arcade
- [ ] enable Card Arcade to play all activities in the quests #major

## Discover Inventory

- [ ] inventory_add must trigger task collect item (with tag)
- [ ] inventory select item (currentItem): improve UI


## Command Party 

- [ ] Party: bugfix distance


## Discover Compass

- [ ] Compass: optimize

## LivingLetters
- [ ] spawn LL areas
- [ ] display image / card data



## Player controller
- [ ] Ride vehicle / antura

# Activities

## Activity Hidden Objects
- [ ] Find hidden objects in the Nantes map, including the museum location.

## Activity group items
- [ ] Separate activity item groups, such as school objects from other objects.

## Activity audio memory
- [ ] Use one native-language audio and one learning-language audio.
- [ ] Add country names.
- [ ] Add school objects.
- [ ] Add foods.

## Activity TraceLine

- [ ] Finish the TraceLine activity.

## Activity Piano
- [ ] Finish the Piano activity.

## activity memory
- [ ] cards + grandi, speak al match
