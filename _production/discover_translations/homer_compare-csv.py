import csv
import os
import re
import argparse

def compare_csv_files(original_dir, new_dir, output_dir, preview=False):
    # Create output directory if it doesn't exist
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    # Get list of CSV files in original directory
    original_files = [f for f in os.listdir(original_dir) if f.startswith('Antura-') and f.endswith('.csv')]
    
    # Process each file
    for filename in original_files:
        # Extract language code from filename (e.g., 'AR' from 'Antura-AR.csv')
        match = re.search(r'Antura-(\w{2})\.csv', filename)
        if not match:
            print(f"Skipping {filename} - invalid filename format")
            continue
        lang_code = match.group(1)
        
        original_path = os.path.join(original_dir, filename)
        new_path = os.path.join(new_dir, filename)
        output_path = os.path.join(output_dir, filename)  # Changed from f"diff_{filename}" to just filename
        
        # Check if the file exists in both directories
        if not os.path.exists(new_path):
            print(f"Skipping {filename} - not found in new directory")
            continue
            
        # Read original file into a dictionary with id as key
        original_data = {}
        with open(original_path, 'r', newline='') as orig_file:
            reader = csv.DictReader(orig_file, delimiter=';')
            # Check if required columns exist
            if 'id' not in reader.fieldnames:
                print(f"Skipping {filename} - no 'id' column found")
                continue
            if lang_code not in reader.fieldnames:
                print(f"Skipping {filename} - no '{lang_code}' column found")
                continue
                
            for row in reader:
                # Trim whitespace from the language code column value
                trimmed_row = {k: v.strip() if k == lang_code else v for k, v in row.items()}
                original_data[row['id']] = trimmed_row
                
        # Compare with new file and collect differences
        differences = []
        with open(new_path, 'r', newline='') as new_file:
            reader = csv.DictReader(new_file, delimiter=';')
            fieldnames = reader.fieldnames
            
            for row in reader:
                # Trim whitespace from the language code column value
                trimmed_row = {k: v.strip() if k == lang_code else v for k, v in row.items()}
                row_id = row['id']
                
                # If ID doesn't exist in original, it's new
                if row_id not in original_data:
                    if preview:
                        print(f"{filename} - ID {row_id}: New row added")
                    differences.append(trimmed_row)
                # If ID exists, check if trimmed language code column value changed
                elif original_data[row_id][lang_code] != trimmed_row[lang_code]:
                    if preview:
                        print(f"{filename} - ID {row_id}: '{lang_code}' changed from '{original_data[row_id][lang_code]}' to '{trimmed_row[lang_code]}'")
                    differences.append(trimmed_row)
        
        # Write differences to new file if there are any
        if differences:
            with open(output_path, 'w', newline='') as output_file:
                writer = csv.DictWriter(output_file, fieldnames=fieldnames, delimiter=';')
                writer.writeheader()
                writer.writerows(differences)
            print(f"Created diff file for {filename} with {len(differences)} changes")
        else:
            print(f"No differences found in {filename}")

if __name__ == "__main__":
    # Set up argument parser
    parser = argparse.ArgumentParser(description="Compare CSV files and generate diff files")
    parser.add_argument("--preview", action="store_true", help="Preview differences in console")
    
    # Parse arguments
    args = parser.parse_args()
    
    # Directory paths
    original_directory = "csv"
    new_directory = "csv-new"
    output_directory = "csv-differences"
    
    # Run the comparison with preview option
    compare_csv_files(original_directory, new_directory, output_directory, preview=args.preview)