---
title: Create Edition
nav_order: 0
---
# How to create a new edition

1. add edition enum to AppEditionID
2. add content enum to LearningContentID
3. add used language enums to LanguageCode
4. create `edition_?` folder in `_config` (copy it from a similar one, payattention to .meta)
5. configure the edition and content configs with the new languages
6. create language folder in `/_lang_bundles` and configure
7. create in `json_data` a Google Sheet ref for every data to sync (for example `Polish_Vocabulary`) and set its google sheed uid and filename
8. open Manage Database scene and import the google data...

## Git
there are tre ways to manage a custom edition:
1. clone the project and keep it separated, like a custom product
2. fork the project, work on a branch and then submit the changes to the main repository wtih Pull Requests
3. ask for write access to the main repository if we can manage to collaborate.

In anyway you should create an `edition_name` branch

## Unity Cloud
- create new project (every app has a different project_id)
- setup Cloud Build
- setup Analytics

## Publish
We don't cover here how to publish on Google Play and Apple App Store.  
It would be bettere if we do it together and if the app is an official Antura Edition, we can publish it for you.
