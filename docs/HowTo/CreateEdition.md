# How to create a new edition

1. add edition enum to AppEditionID
2. add content enum to LearningContentID
3. add used language enums to LanguageCode
4. create `edition_?` folder in `_config` (copy it from a similar one, payattention to .meta)
5. configure the edition and content configs with the new languages
6. create language folder in `/_lang_bundles` and configure

7. create in `json_data` a Google Sheet ref for every data to sync (for example `Polish_Vocabulary`) and set its google sheed uid and filename

