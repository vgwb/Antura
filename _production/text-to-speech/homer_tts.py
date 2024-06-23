import csv
import argparse
import os
from tqdm import tqdm
from pathlib import Path
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

  voice_mapping = {
    "Major": ("Dave", "eleven_multilingual_v1"),
    "Guide": ("Rachel", "eleven_multilingual_v2"),
    "Teacher": ("Charlotte", "eleven_multilingual_v1"),
    "Cook": ("Jessie", "eleven_multilingual_v1"),
  }
  voice, model = voice_mapping.get(actor, ("Adam", "eleven_multilingual_v2"))
  # settings=VoiceSettings(stability=0.71, similarity_boost=0.5, style=0.0, use_speaker_boost=True)

  try:
    audio = client.generate(text=dialogue_text, voice=voice, model=model)
    output_file.parent.mkdir(parents=True, exist_ok=True)
    save(audio, str(output_file))
  except Exception as e:
    print(f"Error generating audio for {output_file}: {e}")

def process_csv(lang_code):
  file_langcode = "FR" if lang_code == "EN" else lang_code
  csv_file = Path(f"csv/Antura-{file_langcode}.csv")
  output_dir = Path(f"audiofiles/{lang_code}")

  with csv_file.open(newline='', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile, delimiter=';')
    total_lines = sum(1 for row in csv.DictReader(open(csv_file)))
    csvfile.seek(0)  # Reset file position to the beginning after counting
    reader = csv.DictReader(csvfile, delimiter=';')  # Reinitialize reader
    for row in tqdm(reader, total=total_lines, desc=f"Processing {lang_code}"):
      dialogue_text = row[lang_code]
      dialogue_id = row['id']
      dialoge_actor = row['actor']
      output_filename = output_dir / f"{lang_code}_{dialogue_id}.mp3"
      synthesize_speech(dialogue_text, lang_code, dialoge_actor, output_filename)

def parse_arguments():
  parser = argparse.ArgumentParser(description="Convert CSV file dialogues to audio.")
  parser.add_argument("lang_code", help="The language code, like FR, AR, RO, UK...")
  args = parser.parse_args()
  if not args.lang_code:
      parser.print_help()
      exit("Error: You must provide the lang_code.")
  return args.lang_code

# Main function to run the script
if __name__ == "__main__":
    lang_code = parse_arguments()
    print("Please wait a few minutes...")
    process_csv(lang_code)
    print("DONE!")
