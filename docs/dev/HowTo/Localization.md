## New Localization System
(Unity Localization package)

we put all UI / common strings in the Common sheet

### As Game Component:
add `Localize String Event` component to the TMPro GO, then add the event `Update String` linking the relative TMPro component > `text` dynamic string 

### In scripts:

use  `LocalizationManager.GetNewLocalized("localization_id")`
