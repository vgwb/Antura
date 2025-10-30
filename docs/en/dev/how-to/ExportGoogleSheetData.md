---
title: Export from Sheets
nav_order: 0
---
# How to export data from Google Sheets
> Note: from 2020 the Sheets are imported directly from the Unity Editor.
> These docs are good to export the JSON manually.

We store all static data as JSON files.
These can be exported from Google Sheets by an open source addon: "Export Sheet Data".

## Install this addo to Google Sheets:

- in a Google Sheet, click "Add-ons/Get Add-ons"
- find "Export Sheet Data" and add it

## Export JSON data:

- Open the Sheet you want to export
- click Add-ons/Export Sheet Data/Open Sidebar
- keep everything at the default settings, but:
  - select "Current sheet only" for "Select Sheet(s)" under "Format"
  - tick "Force string values" under "JSON Options"
  - tick "Export sheet arrays" under "JSON Options"
- click the Export button
- download the file
