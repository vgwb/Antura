namespace Antura.Discover
{
    public enum Countries
    {
        Global = 0,
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
        Anthropology = 18,
        Architecture = 25,
        Art = 5,
        Community = 38,
        CulinaryArts = 31,
        Culture = 3,
        Dance = 27,
        Design = 30,
        Diversity = 36,
        Economy = 11,
        Education = 20,
        Environment = 9,
        Ethics = 15,
        Fashion = 29,
        Film = 26,
        Geography = 2,
        Health = 10,
        Heritage = 39,
        History = 1,
        Inclusion = 37,
        Language = 6,
        Law = 19,
        Literature = 21,
        Music = 22,
        PerformingArts = 24,
        Philosophy = 13,
        Photography = 28,
        Politics = 12,
        Religion = 14,
        Science = 4,
        Society = 7,
        Sociology = 17,
        Sports = 32,
        Technology = 8,
        Tradition = 34,
        Travel = 33,
        VisualArts = 23,
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
}
