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

namespace Homer {

public static class HomerVars {

    public static bool AGE = false; 
    public static int TOTAL_COINS = 0;
    public static string CMD = "";
    public static int TOTAL_ITEMS = 0;
    public static bool MET_GUIDE = false; 
    public static bool MET_MAJOR = false; 
    public static bool MET_MONALISA = false; 

//------- ACTORS PROPERTIES


//KID_FEMALE
    public static int KID_FEMALE_LEVEL = 0;

//KID_MALE
    public static int KID_MALE_LEVEL = 0;

//MUSEUM_GUIDE
    public static int MUSEUM_GUIDE_LEVEL = 0;

//WOMAN
    public static int WOMAN_LEVEL = 0;

//MAN
    public static int MAN_LEVEL = 0;

//COOK
    public static int COOK_LEVEL = 0;

//GUIDE
    public static int GUIDE_LEVEL = 0;

//TEACHER
    public static int TEACHER_LEVEL = 0;

//TUTOR
    public static int TUTOR_LEVEL = 0;

//MAJOR
    public static int MAJOR_LEVEL = 0;

}

public static class HomerActors {
    public enum Actors {
              KID_FEMALE = 9, 
              KID_MALE = 8, 
              MUSEUM_GUIDE = 7, 
              WOMAN = 6, 
              MAN = 5, 
              COOK = 4, 
              GUIDE = 0, 
              TEACHER = 1, 
              TUTOR = 2, 
              MAJOR = 3
}

}

public static class HomerMeta {
          public enum NATIVE {}
          public enum ACTION_POST {}
          public enum IMAGE {}
          public enum NEXTTARGET {}
          public enum ACTION {}
          public enum MOOD {NEUTRAL, HAPPY, VERY_HAPPY, SAD, ANGRY, AMAZED}
          public enum EXPRESSION {RELAXED, GRINNING, PENSIVE, FROWNING, CRYING, ENRAGED}
          public enum BALLOON_TYPE {SPEECH, WHISPER, THOUGHT, SCREAM, SLEEP, SING_SONG}
          public enum LOCATION {}
          public enum CAMERA_DIRECTION {}
          public enum LIGHTINING {}
          public enum FLOW_STATE {IDEA, NOTES, DRAFT, EDITOR, FINAL}
          public enum FixedTypes { NOTE, IMAGE }
}

public static class HomerLabels {
public enum Label { 
              NEW_LABEL_KEY = 0
}
}


public static class HomerTags {
public enum Tag { 
}
}

public static class HomerColors {

public enum color { 
              Blue = 1, 
              Orange = 2, 
              Pink = 3, 
              Red = 4, 
              Green = 5
}
}

public static class HomerFlowSlugs {
public enum FlowSlug { 
              TUTORIAL = 0, 
              FR_01_PARIS = 1, 
              FR_02_ANGERS = 2, 
              FR_03_NANTES = 3, 
              PL_01_QUEST_ = 4, 
              FR_04_LE_MANS = 5, 
              FR_01B_PARIS___SEINE = 6, 
              FR_05_CASTLES = 7, 
              FR_06A_CARNAC___MENHIRS = 8, 
              FR_06B_PIRATES___MONEY = 9, 
              FR_07_NORMANDIE = 10, 
              FR_08_MONT_BLANC___MOUNTAINS = 11, 
              FR_09_COTES_DAZUR = 12
}
}

}
