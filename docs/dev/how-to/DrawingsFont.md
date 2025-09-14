---
layout: default
title: Drawings Glyphs (Words Images)
parent: HowTo
nav_order: 0
---
# How to create the Drawings font

## How to Draw a word
NOTE for the artists:
the vectorial drawings are Black and White and must be saved in SVG format with all paths closed, exploded and with no hidden layer.
In this repo you can find current used drawings: [Antura_assets](https://github.com/vgwb/Antura_assets/tree/master/Drawings/used)

## Create the Font

1. Open it with Affinity Designer (maybe we could use Inkscape but i'm not sure it exports slices). check that every layer is named as the word_id in the Sheet.
2. Export all groups/layers as slices as SVG (for web) into the SVG/todo
3. Import all these single SVG files into Glyph (could be any OTF font maker), assigning the Unicode to the Sheet and moving the SVG file into the "used" folder
4. make sure that every unicode id in Google Sheet WordData is unique
5. Export the OTF font into `/Assets/_app/Fonts/AnturaWordDrawings-Regular.otf`
6. open TextMesh Pro Font Asset Creator using these parameters:

```text
Font Source: AnturaWordDrawings-Regular.otf
Font Size: Auto Sizing
Font padding: 5
Packing Method: Optimum
Atlas res: 2048x2048
Font Render Mode: Distance Field 32
```

## Hex Character Sequences
## Common:
22-23,26,28,2A,2F-31,33-36,39-3B,3F,42-4B,4D-54,56,58-5B,5D-5E,61-65,68,6B-6C,6E-71,73,75-7B,91,98,9A,A1-A7,AB,AE,B7,BF-C0,C2,C4-D6,D8-DF,E2,E4-E5,E7-EB,ED-F1,F3-F5,F8,FB,FD-FE,100,103-104,10C-10F,112-113,116,118-11A,11E-123,126-127,12A-12B,12E-131,136-137,139-13E,141-148,14A-14D,150-15B,15E-167,16A-16B,16E-17E,218-21B,300-302,307-308,391-398,3A6,402-404,407,409-40C,40E-40F,452-454,457,473,4CF,4E2-4E3,4E8-4E9,1E80-1E85,1EA4-1EA7,1EAA,1EF2-1EF3,2019,2020-2022,2026,2030,2044,2191-2192,2196-2198,2265,2669-266F,3A7,3A8,3A9,388,386,38C,38A,38E,3AA,38F,389

## Arabic addon:
24-25,27,29,2B-2C,2E,31-32,37-39,40-41,4C,55,57,5C,5F,64,67,69-6A,6D,70,72-74,77,79-7D,80,82,90,92-97,99,9B-9F,A9,B0,B6-B7,BB,C0-C1,C5,C8-C9,CB,CD-CF,D4,D8,E0,E3,E6,EC,F2,F6,F9-FA,FF,101-102,105-10B,10D,114-115,11B,494-495,4A8-4AB,4B2-4B3,4B6-4B8,4BC-4BE,4CB,4D0,4D2,4F4,4F6,2018,201C-201D,2039-203A,20AC,2190,2193-2195,2199,2265

## French addon:
//392-398,399-3A1,3A3-3A5

## Spanish addon:
//413,414,416,417,418,419,41A,423,424,41B,41F,427,426,428,429,42E,42F,431,437


## NOT USED

## ARABIC (TOTAL: 291 glyphs with size 221)
22-5F,61-7E,80,82,90-9F,A1-A7,A9,AB-AC,AE,B0-B1,B6-B7,BB,BF-C2,C4-10F,112-115,118-11B,122-123,12A-12B,12E-12F,136-137,14A-14B,152-153,175,177,300-302,307-308,391,494-495,4A8-4AB,4B2-4B3,4B6-4B8,4BC-4BE,4CB,4D0,4D2,4F4,4F6,1EA7,2018-2019,201C-201D,2020-2022,2026,2030,2039-203A,20AC,2190-2199,2202,2205,220F,2211-2212,221A,221E,222B,2248,2260,2264-2265,2669-266F

## LearnEnglish (TOTAL: 265 glyphs with size 231)
LearnFrench: 268 glyphs a 218
Character Set: Unicode Range (Hex) with this Sequence:

011E,011F,014D,015A,015B,015E,015F,016A,016B,016E,016F,0176,017A,017B,017C,017D,017E,021A,021B,04F7,100,103,104,10D,10E,10F,112,113,116,118,119,11A,120,121,122,123,126,127,12A,12B,12E,12F,130,131,136,137,139,13A,13B,13C,13D,13E,141,142,143,144,145,146,147,148,14A,14C,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,170,171,172,173,174,177,178,179,1E80,1E81,1E82,1E83,1E84,1E85,1EA4,1EA5,1EA6,1EA7,1EAA,1EF2,1EF3,2019,2022,2026,2030,2044,218,219,2191,2192,2196,2197,2198,22,2202,2205,220F,2211,2212,221A,221E,222B,2248,2260,2264,26,2669,266A,266B,266C,266D,266E,266F,2A,2F,30,31,33,34,35,36,39,3A,3C,3D,3E,3F,42,43,44,45,46,47,49,4A,4B,4D,4E,50,51,52,53,54,56,58,59,5A,5B,5D,61,62,63,64,65,68,6B,6C,6E,6F,70,71,73,75,77,79,7A,7B,7E,98,A1,A2,A3,A5,A7,AB,AC,B1,BF,C0,C2,C4,C5,C6,C7,C8,C9,CA,CB,CC,CE,CF,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,DB,DD,DE,DF,E2,E4,E5,E7,E8,E9,EA,EB,ED,EE,EF,F0,F1,F3,F4,F5,F7,F8,FB,FD,FE

EN calendar: ,3C,3D,3E,7E,AC,B1,D7,F7,2202,2205,220F,2211,2212,221A,221E,222B,2248,2260,2264

FR calendar: 392,393,394,395,396,397,398,039B,039F,03A4,399,398,039E,039D,039A,039C,03A3,03A1,03A0

