namespace Antura.Minigames.DiscoverCountry.Interaction
{
    public enum InteractionLayer
    {
        None,
        Changing, // Layer takes a frame to change, this indicates it's in the frame
        World,
        Map,
        Dialogue
    }

    public enum InteractionType
    {
        None = 0,
        HomerCommand = 1,
        HomerNode = 2,
        UnityAction = 3
    }
}
