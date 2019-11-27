# How to export data from Google Sheets

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
