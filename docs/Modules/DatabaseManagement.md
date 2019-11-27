# Database Info and Management

Here is a description of the Antura Database and proposal for the web application to deal with the upload of the db files from the Antura app to the servers and their management.

## Overview

Antura app stores all the data in two datasets:
A **static** set of tables with all the vocabularies and game settings (like the journey data, letters, words)... These data should not change and is fixed for all players.
A **dynamic** set of tables, storing all the player’s performances and informations. These tables are stored in a SQLite database.
When a player starts player, a UUID (Universal Unique Identifier) and a new SQLite database are created. This UUID is the key to identify the player. The SQLite file itself is named Antura_Player_UUID value (like `Antura_Player_e7e8d59a-b148-48b0-a848-4fa6fb4116cb.sqlite3` )
(Going into the Reserved Area of the app, selecting a player we can see its UUID.)

The app is going to allow the export of the sqlite db, and we were thinking that the best way was to upload the file directly into a web server, injecting the SQLite data into a bigger “master collector” mysql db, to allow comparative analysis and queries.

This document describes all the Database related info needed for the app developers, the web developers and the data analysts.

## Web upload DB project

The web app receiving the db files should be an independent app, maybe accessible through a static domain like: `https://db.antura.org` (if possible it’s much better a third level domain to separate this db app from the generic wordpress website at antura.org )

We think that the Antura App should just access that page, handshake a security token, and then upload its SQLite file.
The application can be developed with any web technology, but we assume to use LAMP (Linux / Apache / Mysql / PHP) since it’s a very common stack.

As first step, we imagine a script like `https://db.antura.org/upload.php` where we can upload our file as a multi-part form.
The file won’t be bigger than 1Mb (but let’s support file sizes of at least 8Mb)
The script should save this file into a named folder, like `/uploads/2017/04/filename`
(since we expect several thousands of files, it’s better to organize them by month)

When the upload finishes with success, an log entry should be added to the databases, like “file x has been uploaded at this time from , it’s location is y”.
Just after the uploading a new script should be called that opens the sqlite file, processes it and injects its data into the main mysql database (we’ll describe the exact procedure later)
An email should be sent to a list of addresses to notify the new file entry.

## MySql database

We store the app data as sqlite because it’s simpler and easier (and faster).
But the main database should be mysql, which allow greater flexibility and features, and has major clients.
As a first very easy and powerful tool to manage the master Mysql db, we should have PhpMyAdmin ( <https://www.phpmyadmin.net/> ) installed into the server. From there we can export the db and make basic queries without writing anything custom.

## Parsing the SQLite files

The PHP has native SQLite3 functions (since 5.3.0, see <http://php.net/manual/en/sqlite3.installation.php> ) and should be safe to open the single sqlite files.
The SQLite -> Mysql migration need to take care of these points:

1. To inject the UUID (found in the sqlite filename or in the PlayerProfileData table) into every uuid field of every table.
2. Obviously the SQLIte indexes shouldn’t be migrated (and aren’t used in any case)
3. The data structure is quite the same in both DB, so no data conversion is needed.

## Databases
App SQLite file

## Db filename

Filename: `Antura_Player_e7e8d59a-b148-48b0-a848-4fa6fb4116cb.sqlite3`
The “e7e8d59a-b148-48b0-a848-4fa6fb4116cb” is the uuid (universal identifier) that defines the player

## Procedures

```sql
update DatabaseInfoData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update JourneyScoreData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update LogInfoData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update LogMiniGameScoreData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update LogMoodData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update LogPlayData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update LogPlaySessionScoreData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update LogVocabularyScoreData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update RewardPackUnlockData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
update VocabularyScoreData set Uuid = "e7e8d59a-b148-48b0-a848-4fa6fb4116cb";
```

## Queries

These are the first db queries supported by current data model

1. Curve of Duration of play sessions (one player, a group)
2. Curve of Duration of play per day (one player, a group)
3. Nb of mini-game/assessment per play session (one player, a group)
4. Nb of mini-game/assessment per day (one player, a group)
5. Learning blocks ID or T0+x date of drop out (one player, a group)
6. Antura Space use duration per play session and per day (one player, a group)
7. Score evolution per mini game (the rough data that we use to create the 3 stars score) (one player, a group)
8. The average evolution of score, all MiniGame (in stars) (one player, a group)
9. Evaluate correlation between dropout/non play and score variation/new game apparition (a group)
10. Average assessment tries per learning block (to identify potential more difficult learning blocks) (one player, a group)
11. Ranking of words associated to failure in mini game (one player, a group)
12. Mod indicator curve (one player, a group)
13. Replayed learning blocks (one player, a group)
14. More used outfits, colors and accessories, on Antura (one player, a group)
