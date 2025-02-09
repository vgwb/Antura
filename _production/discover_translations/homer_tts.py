import csv
import argparse
import os
from tqdm import tqdm
from pathlib import Path
from elevenlabs import play, save
from elevenlabs.client import ElevenLabs

client = ElevenLabs(
  api_key=os.getenv('ELEVEN_API_KEY')
)

def synthesize_speech(dialogue_text, language, actor, output_file, preview=False):
  if dialogue_text == "--- to be translated ---":
    return

  # Default values
  voice = "Alex Wright"
  voice_id = "GzE4TcXfh9rYCU9gVgPp"
  model = "eleven_multilingual_v2"

  if language == "FR":
    voice = "Louis Boutin"
    voice_id = "j9RedbMRSNQ74PyikQwD"
  elif language == "IT":
    voice = "Francesco"
    voice_id = "GOAZNavLupajyL3YafaD"
  elif language == "RO":
    voice = "Cristi Romana"
    voice_id = "sGcPNcpR5PikknzyXcy7"
  elif language == "AR":
    voice = "Haytham - Conversation"
    voice_id = "IES4nrmZdUBHByLBde0P"
  elif language == "UK":
    voice = "Alex Nekrasov"
    voice_id = "9Sj8ugvpK1DmcAXyvi3a"
  elif language == "RU":
    voice = "Felix - calm, friendly"
    voice_id = "sRk0zCqhS2Cmv0bzx5wA"    
  elif language == "PL":
    voice = "Alex - Professional Narration Polish"
    voice_id = "H5xTcsAIeS5RAykjz57a"    

  if actor == "man" or actor == "Museum Guide" or actor == "Guide":
    if language == "EN":
      voice = "Cody"
      voice_id = "9XfYMbJVZqPHaQtYnTAO"
  elif actor == "woman" or actor == "kid_female" or actor == "teacher":
    if language == "FR":
      voice = "Mademoiselle - French "
      voice_id = "dYjOkSQBPiH2igolJfeH"
    elif language == "IT":
      voice = "Sami - Italian female "
      voice_id = "kAzI34nYjizE0zON6rXv"
    elif language == "RO":
      voice = "Corina Ioana"
      voice_id = "gbLy9ep70G3JW53cTzFC"
    elif language == "AR":
      voice = "Mona"
      voice_id = "tavIIPLplRB883FzWU0V"
    elif language == "UK":
      voice = "Yaroslava"
      voice_id = "0ZQZuw8Sn4cU0rN1Tm2K"
    elif language == "RU":
      voice = "Soft Female Russian voice"
      voice_id = "ymDCYd8puC7gYjxIamPt"    
    elif language == "PL":
      voice = "Maria"
      voice_id = "d4Z5Fvjohw3zxGpV8XUV" 
    elif language == "ES":
      voice = "Ninoska"
      voice_id = "zl1Ut8dvwcVSuQSB9XkG"
    else:
      voice = "Shelley"
      voice_id = "4CrZuIW9am7gYAxgo2Af"
  elif actor == "kid_male":
      voice = "Kid Male Voice"
#        voice_id = "YourKidMaleVoiceID"
  elif actor == "kid_female":
      voice = "Kid Female Voice"
#        voice_id = "YourKidFemaleVoiceID"
  elif actor == "Cook":
      voice = "Cook Voice"
#        voice_id = "YourCookVoiceID"

  if preview:
      print(f"Preview: {language} {actor} ({voice}): '{dialogue_text}' to file {output_file}")
      return
  else:
      print(f"AUdio: {language} {actor} ({voice}): '{dialogue_text}' to file {output_file}")

  try:
    audio = client.text_to_speech.convert(
      voice_id=voice_id,
      output_format="mp3_44100_64",
      text=dialogue_text,
      model_id=model,
    )
    output_file.parent.mkdir(parents=True, exist_ok=True)
    save(audio, str(output_file))
  except Exception as e:
    print(f"Error generating audio for {output_file}: {e}")

def process_csv(lang_code, quest=None, preview=False):
  file_langcode = "FR" if lang_code == "EN" else lang_code
  csv_file = Path(f"csv/Antura-{file_langcode}.csv")
  output_dir = Path(f"audiofiles/{lang_code}")

  with csv_file.open(newline='', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile, delimiter=';')
    total_lines = sum(1 for row in csv.DictReader(open(csv_file)))
    csvfile.seek(0)  # Reset file position to the beginning after counting
    reader = csv.DictReader(csvfile, delimiter=';')  # Reinitialize reader
    for row in tqdm(reader, total=total_lines, desc=f"Processing {lang_code}"):
      if quest and row['flow'] != quest:
        continue
      dialogue_text = row[lang_code]
      dialogue_id = row['id']
      dialoge_actor = row['actor']
      output_filename = output_dir / f"{lang_code}_{dialogue_id}.mp3"
      synthesize_speech(dialogue_text, lang_code, dialoge_actor, output_filename, preview)

def parse_flows(lang_code):
  file_langcode = "FR" if lang_code == "EN" else lang_code
  csv_file = Path(f"csv/Antura-{file_langcode}.csv")
  
  flows = set()

  with csv_file.open(newline='', encoding='utf-8') as csvfile:
      reader = csv.DictReader(csvfile, delimiter=';')
      for row in reader:
          flows.add(row['flow'])
  
  print("Distinct flows found:")
  for flow in sorted(flows):
      print(flow)

def parse_arguments():
  parser = argparse.ArgumentParser(description="Convert CSV file dialogues to audio.")
  parser.add_argument("lang_code", help="The language code, like FR, AR, RO, UK...")
  parser.add_argument("--quest", help="The quest parameter to filter dialogues.", required=False)
  parser.add_argument("--preview", action="store_true", help="Preview the dialogues to be processed without generating audio files.")
  parser.add_argument("--flows", action="store_true", help="Output the distinct flows from the CSV.")
  args = parser.parse_args()
  if not args.lang_code:
      parser.print_help()
      exit("Error: You must provide the lang_code.")
  return args

# Main function to run the script
if __name__ == "__main__":
  args = parse_arguments()
  if args.flows:
      parse_flows(args.lang_code)
  else:
      print("Please wait a few minutes...")
      process_csv(args.lang_code, args.quest, args.preview)
      print("DONE!")

