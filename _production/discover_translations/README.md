# Discover TTS

## Configuration
to show current Evniroment Variables:
`env`

to save a new environment variable:
`export ELEVEN_API_KEY=value`

## Create audio files
save the Homer translations files into the `csv` directory.
then use:  
`python homer_tts.py lang_code --quest=flow_name --preview --flows`

examples:
### generate single language
python homer_tts.py FR --quest "Tutorial"
python homer_tts.py FR --quest "FR-01b Paris - Seine"

### generate all languages for a single quest
python homer_tts.py --quest "Tutorial"
python homer_tts.py --quest "FR-01 Paris"
python homer_tts.py --quest "FR-01b Paris - Seine"
python homer_tts.py --quest "FR-03 Nantes"


where lang_code is:

- AR (arabic)
- EN (english)
- ES (spanish)
- FR (french)
- IT (italian)
- PL (polish)
- RO (romanian)
- RU (russian)
- UK (ukrainian)

--quest parameters filters only the selected flow

--preview doesn't generate the audio files

to list all the available flows:

`homer_tts.py --flows EN`

## Tool

**ElevenLabs**

- https://elevenlabs.io/docs/introduction
- https://github.com/elevenlabs/elevenlabs-python
- https://elevenlabs.io/app/voice-library
- https://elevenlabs.io/app/speech-synthesis
- https://elevenlabs.io/docs/voices/premade-voices

## Install
pip install elevenlabs
pip install tqdm

## Test message

English
this is a test message to hear the quality of this syntetized voice.

ROmania
acesta este un mesaj de testare pentru a auzi calitatea acestei voci sintetizate.

Ukrainian
це тестове повідомлення, щоб почути якість цього синтезованого голосу.

Polish
to jest wiadomość testowa, aby usłyszeć jakość tego syntetycznego głosu.

Italiano
questo è un messaggio di prova per sentire la qualità di questa voce sintetizzata.

French
ceci est un message test pour entendre la qualité de cette voix synthétisée.

Arabic
هذه رسالة اختبارية لسماع جودة هذا الصوت المركب

