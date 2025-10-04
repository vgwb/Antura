---
title: Retours et support
---

# Retours et support

## Suggestions d’amélioration

Nous accueillons volontiers les retours et idées des parents, enseignants et élèves.

- Signaler problèmes et difficultés
- Proposer de nouveaux sujets, cartes, activités de classe
- Suggérer des améliorations d’ergonomie (commandes, caméra, menus)
- Partager ce qui a bien marché en classe et ce qui pourrait être clarifié

Rejoignez notre communauté : <https://antura.discourse.group>

## Signaler un problème

Chaque quête possède une zone dédiée sur le forum pour vos retours, bugs et idées. Consultez le lien correspondant depuis les [pages de quêtes](../content/quests/index.md)

**À inclure :**

- Appareil/OS (ex. PC Windows 11, iPad iPadOS 15) et version/date du build
- Étapes claires pour reproduire le problème
- Résultat attendu vs. résultat constaté
- Captures ou courte vidéo, si possible

## Envoyez-nous le journal du joueur (Windows)

Lorsqu'un problème survient, le jeu crée un fichier journal que nous pouvons consulter pour diagnostiquer le problème.

### Journal du joueur

1. Appuyez sur **Win + R** sur votre clavier pour ouvrir « Exécuter... ».
2. Collez ceci et appuyez sur **Entrée** :

```shell
%USERPROFILE%\AppData\LocalLow\VGWB\Antura
```

3. Vous devriez voir un fichier appelé `Player.log` (et parfois `Player-prev.log`).
4. Cliquez avec le bouton droit de la souris → Envoyer vers → Dossier compressé (zippé).
5. Envoyez-nous le fichier .zip par e-mail.

Si vous ne voyez pas le dossier « AppData » : dans l'Explorateur de fichiers, cliquez sur Affichage → Afficher → Éléments masqués.

### Journal des plantages

Si le jeu a planté, veuillez vérifier si le dossier « Crashes » existe à l'emplacement suivant et envoyez également le dossier de plantage le plus récent :
1. Appuyez sur **Win + R** sur votre clavier pour ouvrir « Exécuter… ».
2. Collez ceci et appuyez sur **Entrée** :

```shell
%LOCALAPPDATA%\Temp\VGWB\Antura
```

3. Vérifiez s'il existe un dossier appelé « Crashes ».
4. S'il existe, veuillez le compresser et nous envoyer le sous-dossier le plus récent (il contient error.log, crash.dmp, etc.).