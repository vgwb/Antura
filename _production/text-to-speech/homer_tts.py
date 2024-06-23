import csv
import argparse
import os
from elevenlabs import play, save
from elevenlabs.client import ElevenLabs

client = ElevenLabs(
  # api_key="", # Defaults to ELEVEN_API_KEY
  # or use an environment variable!
  api_key = os.getenv('ELEVEN_API_KEY')
)

def synthesize_speech(dialogue_text, language, actor, output_file):
  if (dialogue_text == "--- to be translated ---"):
    return

  if actor == "Major":
    audio = client.generate(
      text = dialogue_text,
      voice = "Dave",
      model = "eleven_multilingual_v1",
    )
  elif actor == "Guide":
    audio = client.generate(
      text = dialogue_text,
      voice = "Rachel",
      model = "eleven_multilingual_v2",
    )
  elif actor == "Teacher":
    audio = client.generate(
      text = dialogue_text,
      voice = "Charlotte",
      model = "eleven_multilingual_v1",
    )
  elif actor == "Cook":
    audio = client.generate(
      text = dialogue_text,
      voice = "Jessie",
      model = "eleven_multilingual_v1",
    )
  else:
    audio = client.generate(
      text = dialogue_text,
      voice = "Adam",
      model = "eleven_multilingual_v2",
    #  settings=VoiceSettings(stability=0.71, similarity_boost=0.5, style=0.0, use_speaker_boost=True)
    )
  # Ensure the directory exists
  os.makedirs(os.path.dirname(output_file), exist_ok=True)
  save(audio, output_file)

def process_csv(lang_code):
  file_langcode = lang_code
  # English is taken from EN column of the FR csv
  if (lang_code == "EN"):
    file_langcode = "FR"
  with open(f"csv/Antura-{file_langcode}.csv", newline='', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile, delimiter=';')
    for row in reader:
      dialogue_text = row[lang_code]
      dialogue_id = row['id']
      dialoge_actor = row['actor']
      output_filename = f"audiofiles/{lang_code}/{lang_code}_{dialogue_id}.mp3"
      synthesize_speech(dialogue_text, lang_code, dialoge_actor, output_filename)

def parse_arguments():
    parser = argparse.ArgumentParser(description="Convert Homer CSV file dialogues to audio.")
    parser.add_argument("lang_code", help="The language code, like FR, AR, RO, UK...")
    return parser.parse_args()

# Main function to run the script
if __name__ == "__main__":
    args = parse_arguments()
    if args.lang_code:
        print("Please wait a few minutes...")
        process_csv(args.lang_code)
        print("DONE!!")
    else:
        print("Error: You must provide the lang_code.")
        parser.print_help()
