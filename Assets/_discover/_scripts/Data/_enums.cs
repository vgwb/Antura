namespace Antura.Discover
{
    public enum Countries
    {
        International = 0,
        Italy = 1,
        France = 2,
        Poland = 3,
        Spain = 4,
        Germany = 5,
        Portugal = 6,
        Greece = 7,
        UnitedKingdom = 8
    }

    public enum Difficulty
    {
        Tutorial = 1,
        Easy = 2,
        Normal = 3,
        Difficult = 4
    }

    public enum CardCategory
    {
        None = 0,
        Achievement = 10,
        Art = 3,
        Fauna = 6,
        Flora = 5,
        Food = 4,
        Humanity = 9,
        Natural = 2,
        Object = 8,
        People = 7,
        Place = 1,
    }

    public enum KnowledgeTopic
    {
        None = 0,
        // Core subjects (simplified)
        History = 1,           // Keep - kids love stories from the past
        Geography = 2,         // Keep - where things are
        Science = 4,           // Keep - how things work
        Art = 5,              // Keep - making and creating
        Culture = 3,          // Keep - how people live

        // Kid-friendly specifics
        Animals = 40,          // Instead of complex "Fauna"
        Plants = 41,           // Instead of "Flora"
        Space = 42,            // Kids love astronomy
        Weather = 43,          // Observable science
        Transportation = 44,   // Cars, planes, boats

        // Creative & Active
        Music = 22,            // Keep
        Sports = 32,           // Keep
        Cooking = 45,          // Instead of "CulinaryArts"
        Dancing = 46,          // Instead of "Dance/PerformingArts"

        // Social concepts (simplified)
        Family = 47,           // Instead of "Sociology"
        Community = 38,        // Keep - good for this age
        Friendship = 48,       // Important social concept

        // Practical life
        Education = 20,
        Health = 10,           // Keep
        Safety = 49,           // Important for kids
        Environment = 9,       // Keep - caring for nature
        Money = 11,            // Keep - basic financial literacy
        Time = 12              // Keep - understanding clocks/calendars
    }

    public enum GameplayType
    {
        Orientation,
        Seek,
        Parkour,
        Puzzles,
        Deduction,
        Journey,
        Arcade,
        Memory,
        Story
    }

    public enum DevStatus
    {
        Inactive = 0,
        Development = 1,
        Testing = 2,
        Ready = 3,
        Validated = 4,
    }

    public enum AssetType
    {
        Image,
        Audio,
        Model3D
    }

    public enum LicenseType
    {
        CC0,        // Public Domain equivalent
        CC_BY,      // Attribution required
        CC_BY_SA,   // Attribution + Share Alike
        Licensed    // Proprietary or custom license
    }

    /// <summary>
    /// Broad competence categories — each activity/minigame should have at least one.
    /// These are high-level groupings
    /// </summary>
    public enum ActivityCategory
    {
        None = 0,
        Attention = 1,               // Focusing on relevant info and ignoring distractions.
        Coordination = 11,           // Motor precision and timing.
        Emotions = 2,                // Recognizing, managing, and expressing emotions; empathy.
        Flexibility = 3,             // Adapting strategies; switching tasks or rules.
        Knowledge = 14,              // Factual knowledge from various subjects.
        Language = 8,                // Reading, vocabulary, and verbal expression.
        Math = 4,                    // Formal mathematical thinking and problem solving.
        Memory = 5,                  // Holding and recalling information (short or long term).
        Music = 10,                  // Perception and production of sounds, notes, rhythms.
        Numeracy = 13,               // Applying numbers and money in everyday contexts.
        Observation = 12,            // Detecting details, differences, and patterns.
        ProblemSolving = 6,          // Analyzing situations and finding effective solutions.
        ProcessingSpeed = 7,         // Working quickly and accurately under time pressure.
        Sequencing = 9,              // Ordering events or elements logically/chronologically.
    }

    /// <summary>
    /// Specific skills — fine-grained abilities trained or required by the activity.
    /// You can tag multiple skills per activity.
    /// </summary>
    public enum ActivitySkill
    {
        None = 0,

        // --- Emotional & Social
        Emotions_Sense = 1,                  // Identifying feelings in self and others.
        Emotions_Inclusiveness = 2,          // Acting kindly and including everyone.

        // --- Attention
        Attention_Divided = 3,               // Handling multiple tasks or inputs simultaneously.
        Attention_Selective = 4,             // Focusing on target while ignoring distractions.
        Attention_TaskSwitching = 5,         // Shifting focus quickly between rules/goals.
        Sustained_Attention = 6,             // Maintaining focus over extended time.

        // --- Motor & Perception ---
        Coordination_Timing = 7,             // Acting precisely at the right moment.
        Fine_Motor_Control = 8,              // Making small, precise hand/finger movements.
        FieldOfView = 9,                      // Noticing objects/hazards at the edges of vision.
        Visual_Discrimination = 10,          // Spotting differences; matching similar images.

        // --- Thinking & Reasoning
        Logical_Reasoning = 11,              // Using rules/evidence to reach conclusions.
        PatternRecognition = 12,             // Detecting regularities, sequences, repetitions.
        Planning = 13,                       // Deciding steps and order to reach a goal.
        Response_Inhibition = 14,            // Suppressing impulsive responses.

        // --- Processing
        ProcessingSpeed = 15,                // Working quickly while maintaining accuracy.

        // --- Memory
        Memory_Working = 16,                 // Holding small info chunks for short time.
        Memory_Sequencing = 17,              // Remembering order of items/events.
        Spatial_Recall = 18,                 // Remembering where things are in space.
        Auditory_Memory = 19,                // Remembering and repeating sounds/sequences.

        // --- Spatial Skills
        Spatial_Orientation = 20,            // Understanding directions/position in space.
        Spatial_Reasoning = 21,              // Mentally rotating/assembling shapes.
        Visualization = 22,                  // Imagining objects/actions mentally.

        // --- Language
        Reading_Comprehension = 23,          // Understanding meaning of written text.
        Vocabulary_Proficiency = 24,         // Knowing and using many words correctly.
        Word_VerbalFluency = 25,             // Speaking/naming quickly and clearly.

        // --- Math & Quantitative
        Math_NumericalCalculation = 26,      // Adding, subtracting, multiplying, dividing.
        Math_NumericalEstimation = 27,       // Approximating quantities/results.
        Math_ProbabilisticReasoning = 28,    // Judging chances; deciding under uncertainty.
        ProportionalReasoning = 29,          // Comparing ratios; scaling amounts.
        Math_Currency_Calculation = 30,      // Combining coins/notes to reach a target.

        // --- Association & Ordering ---
        Pairing_Association = 31,            // Matching items that belong together.
        Sequencing_Order = 32,               // Arranging steps/events correctly.

        // --- Music
        Rhythm_Sense = 33,                   // Keeping a beat; reproducing rhythms.

        // --- Knowledge
        General_Knowledge = 34               // Recalling facts from culture, science, daily life.
    }

}
