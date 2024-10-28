/*
Homer - The Story Flow Editor.
Copyright (c)2021-2024. Open Lab s.r.l - Florence, Italy
Developer: Pupunzi (Matteo Bicocchi)

-------------------------------------------------------------------------------------
C# Variables, Actors, Metadata, Labels and Flows Slug.
Homer project: Antura
Generated on: 2024-10-28 16:06:52
-------------------------------------------------------------------------------------
*/

namespace Homer {

    public static class HomerVars {
    
    public static int TOTAL_COINS = 0;
    public static string CMD = "";
    public static int TOTAL_ITEMS_1 = 0;
    public static bool MET_GUIDE = false; 
    public static bool MET_MAJOR = false; 
    public static bool MET_MONALISA = false;     }

    public static class HomerActors {
        public enum Actors {MUSEUM_GUIDE = 7, NPG_F = 6, NPG_M = 5, COOK = 4, GUIDE = 0, TEACHER = 1, TUTOR = 2, MAJOR = 3}

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
        public enum Label { NEW_LABEL_KEY = 0 }
    }

    public static class HomerColors {

        public enum color { Blue = 1, Orange = 2, Pink = 3, Red = 4, Green = 5 }
    }

    public static class HomerFlowSlugs {
        public enum FlowSlug { TUTORIAL = 0, FR_01_PARIS = 1, FR_02_ANGERS = 2, FR_03_NANTES = 3, PL_01_QUEST_ = 4, FR_04_LE_MANS = 5 }
    }

}
