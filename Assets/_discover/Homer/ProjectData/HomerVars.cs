/**
* Homer - The Story Flow Editor.
* https://homer.open-lab.com
* Doc: https://homer.open-lab.com/doc
*
* Copyright (c)2021-2025. Open Lab s.r.l - Florence, Italy
* Developer: Pupunzi (Matteo Bicocchi)
*
* -------------------------------------------------------------------------------------
* C# Variables, Actors, Metadata, Labels, Colors, Tags and Flows Slug.
* Homer project: Antura*
* -------------------------------------------------------------------------------------
*/
using UnityEngine;

namespace Homer {

public static class HomerVars {

    public static int MAX_PROGRESS = 0;
    public static int CURRENT_PROGRESS = 0;
    public static bool IS_DESKTOP = false; 
    public static int QUEST_ITEMS = 0;
    public static bool EASY_MODE = false; 
    public static int TOTAL_COINS = 0;
    public static string CMD = "";
    public static int COLLECTED_ITEMS = 0;
    public static string CURRENT_ITEM = "";
    public static bool MET_GUIDE = false; 
    public static bool MET_MAJOR = false; 
    public static bool MET_MONALISA = false; 

//------- ACTORS PROPERTIES


//KID_FEMALE
    public static int KID_FEMALE_LEVEL = 0;

//KID_MALE
    public static int KID_MALE_LEVEL = 0;

//CRAZY_WOMAN
    public static int CRAZY_WOMAN_LEVEL = 0;

//WOMAN
    public static int WOMAN_LEVEL = 0;

//MAN
    public static int MAN_LEVEL = 0;

//CRAZY_MAN
    public static int CRAZY_MAN_LEVEL = 0;

//GUIDE
    public static int GUIDE_LEVEL = 0;

//OLD_WOMAN
    public static int OLD_WOMAN_LEVEL = 0;

//TUTOR
    public static int TUTOR_LEVEL = 0;

//OLD_MAN
    public static int OLD_MAN_LEVEL = 0;

}

public static class HomerActors {
    public enum Actors {
              KID_FEMALE = 9, 
              KID_MALE = 8, 
              CRAZY_WOMAN = 7, 
              WOMAN = 6, 
              MAN = 5, 
              CRAZY_MAN = 4, 
              GUIDE = 0, 
              OLD_WOMAN = 1, 
              TUTOR = 2, 
              OLD_MAN = 3
}

}

public static class HomerMeta {
          public enum TASK {}
          public enum NATIVE {}
          public enum ACTION_POST {}
          public enum IMAGE {}
          public enum NEXTTARGET {}
          public enum ACTION {}
          public enum MOOD {NEUTRAL, HAPPY, VERY_HAPPY, SAD, ANGRY, AMAZED}
          public enum EXPRESSION {}
          public enum BALLOON_TYPE {PANEL, QUIZ, SPEECH, WHISPER, THOUGHT}
          public enum LOCATION {}
          public enum CAMERA_DIRECTION {}
          public enum LIGHTINING {}
          public enum FLOW_STATE {IDEA, NOTES, DRAFT, EDITOR, FINAL}
          public enum FixedTypes { NOTE, IMAGE }
}

public static class HomerLabels {
public enum Label { 
//            GENERIC 
}
}


public static class HomerTags {
public enum Tag { 
}
}

public static class HomerColors {

//D63762
public static Color32 book = new Color32(214, 55, 98, 255);
//0DB0DD
public static Color32 blue = new Color32(13, 176, 221, 255);
//F89501
public static Color32 orange = new Color32(248, 149, 1, 255);
//EB89C8
public static Color32 pink = new Color32(235, 137, 200, 255);
//47B58B
public static Color32 green = new Color32(71, 181, 139, 255);

public enum color { 
              book = 1, 
              blue = 2, 
              orange = 3, 
              pink = 4, 
              green = 5
}
}

public static class HomerFlowSlugs {
public enum FlowSlug { 
              TUTORIAL = 0, 
              FR_01_PARIS = 1, 
              FR_02_ANGERS___SCHOOL = 2, 
              FR_03_NANTES = 3, 
              PL_01_WARSAW___CHOPIN = 4, 
              FR_04_LE_MANS = 5, 
              FR_01B_PARIS___SEINE = 6, 
              FR_05_CASTLES = 7, 
              FR_06A_CARNAC___MENHIRS = 8, 
              FR_06B_PIRATES___MONEY = 9, 
              FR_07_NORMANDIE = 10, 
              FR_08_MONT_BLANC___MOUNTAINS = 11, 
              FR_09_COTES_DAZUR = 12, 
              DEV = 13, 
              PL_03_WROCLAW_RIVER = 14, 
              PL_04_ZOO = 15, 
              PL_02_WROCLAW_DWARVES = 16, 
              FR_100_GEOGRAPHY = 17, 
              FR_101_MUSIC = 18, 
              PL_100_GEOGRAPHY = 19, 
              PL_05_BALTIC_SEA = 20, 
              PL_06_TORUN_MARKET = 21
}
}

}
