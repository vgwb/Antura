namespace Antura.Assessment
{
    public static class Localization
    {
        public static Database.LocalizationDataId Random(params Database.LocalizationDataId[] ids)
        {
            return ids[UnityEngine.Random.Range(0, ids.Length)];
        }
    }
}
