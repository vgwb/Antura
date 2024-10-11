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
}