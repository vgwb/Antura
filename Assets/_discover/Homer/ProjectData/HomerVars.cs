/*
C# Variables, Actors and Metadata for the Homer project:
Antura
Homer - The Story Flow Editor.
Copyright (c)2021-2024. Open Lab s.r.l - Florence, Italy
Developer: Pupunzi (Matteo Bicocchi)

*/

namespace Homer {

public static class HomerVars {

    public static int TOTAL_ITEMS_1 = 0;

    public static string AUDIO_ID = "";

    public static bool TEACHER_MET = false; 

    public enum ACTION_TYPE { OPEN_DOOR, CLOSE_DOOR };
    public static string ACTION; 

    public static string CMD = "";

    public static int TOTAL_COINS = 0;
}

public static class HomerActors {
    public enum Actors {NPG_F = 6, NPG_M = 5, COOK = 4, GUIDE = 0, TEACHER = 1, TUTOR = 2, MAJOR = 3}

}

public static class HomerMeta {

    public enum ACTION_POST {}
    public enum IMAGE {}
    public enum NEXTTARGET {}
    public enum ACTION {}
    public enum MOOD {NEUTRAL, HAPPY, VERY_HAPPY, SAD, ANGRY, AMAZED}
    public enum EXPRESSION {RELAXED, GRINNING, PENSIVE, FROWNING, CRYING, ENRAGED}
    public enum BALLOON_TYPE {SPEECH, WHISPER, THOUGHT, SCREAM, SLEEP, SING_SONG}
    public enum LOCATION {HOME, OUTSIDE, OFFICE, PARK, SEA, MOUNTAIN}
    public enum CAMERA_DIRECTION {}
    public enum LIGHTINING {}
    public enum FLOW_STATE {IDEA, NOTES, DRAFT, EDITOR, FINAL}
    public enum FixedTypes { NOTE, IMAGE }
}

public static class HomerLabels {

public enum Label { NEW_LABEL_KEY = 0 }
}

public static class HomerFlowSlugs {
public enum FlowSlug { TUTORIAL = 0, FR_01_EIFFEL_TOWER = 1, FR_02_PAN_AU_CHOCOLATE = 2, FR_03_THE_SCHOOL = 3, PL_01_QUEST_ = 4    }
}

}
