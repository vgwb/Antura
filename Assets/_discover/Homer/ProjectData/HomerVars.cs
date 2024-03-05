/*
C# Variables, Actors and Metadata for the Homer project:
Antura
Homer - The Story Flow Editor.
Copyright (c)2021-2024. Open Lab s.r.l - Florence, Italy
Developer: Pupunzi (Matteo Bicocchi)

*/

namespace Homer {

public static class HomerVars {
}

public static class HomerActors {
    public enum Actors {TUTOR, MAJOR, YOU}

}

public static class HomerMeta {

    public enum MOOD {NEUTRAL, HAPPY, VERY_HAPPY, SAD, ANGRY, AMAZED}
    public enum EXPRESSION {RELAXED, GRINNING, PENSIVE, FROWNING, CRYING, ENRAGED}
    public enum BALLOON_TYPE {SPEECH, WHISPER, THOUGHT, SCREAM, SLEEP, SING_SONG}
    public enum LOCATION {HOME, OUTSIDE, OFFICE, PARK, SEA, MOUNTAIN}
    public enum CAMERA_DIRECTION {CLOSE_UP, ANGLE_ON, OFF_SCREEN, FADE_OUT, FADE_IN, VOICE_OVER}
    public enum LIGHTINING {KEY, BACK, PRACTICAL, HARD, SOFT, CHIAROSCURO}
    public enum FLOW_STATE {IDEA, NOTES, DRAFT, EDITOR, FINAL}
    public enum FixedTypes { NOTE, IMAGE }
}

public static class HomerLabels {

     public enum Label { NEW_LABEL_KEY }
}

public static class HomerFlowSlugs {
    public enum FlowSlug { FR_01_TOUR_EIFFEL, FR_02_PAN_AU_CHOCOLATE, PL_01_QUEST_ }
}

}
