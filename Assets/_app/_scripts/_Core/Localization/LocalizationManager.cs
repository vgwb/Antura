using Antura.Database;
using Antura.Profile;

namespace Antura.Core
{
    /// <summary>
    /// Static class that helps in localizing strings.
    /// </summary>
    public class LocalizationManager
    {

        private static PlayerGender CurrentPlayerGender
        {
            get {
                if (AppManager.I.Player == null) {
                    return AppManager.I.PlayerProfileManager.TemporaryPlayerGender;
                }
                return AppManager.I.Player.Gender;
            }
        }

        public static string GetTranslation(LocalizationDataId id)
        {
            return GetLocalizationData(id).GetLocalizedText(CurrentPlayerGender);
        }

        public static string GetTranslation(string id)
        {
            return GetLocalizationData(id).GetLocalizedText(CurrentPlayerGender);
        }

        public static string GetLocalizedAudioFileName(string id)
        {
            return GetLocalizationData(id).GetLocalizedAudioFileName(CurrentPlayerGender);
        }

        public static string GetLocalizedAudioFileName(string id, PlayerGender forcedGender)
        {
            return GetLocalizationData(id).GetLocalizedAudioFileName(forcedGender);
        }

        public static LocalizationData GetLocalizationData(LocalizationDataId id)
        {
            return AppManager.I.DB.GetLocalizationDataById(id.ToString());
        }

        public static LocalizationData GetLocalizationData(string id)
        {
            return AppManager.I.DB.GetLocalizationDataById(id);
        }

        public static LocalizationData GetWordCategoryData(WordDataCategory cat)
        {
            LocalizationDataId loc = LocalizationDataId.UI_None;
            switch (cat) {
                case WordDataCategory.Adjectives:
                    loc = LocalizationDataId.UI_WordCat_Adjectives;
                    break;
                case WordDataCategory.Animal:
                    loc = LocalizationDataId.UI_Animals;
                    break;
                case WordDataCategory.BodyPart:
                    loc = LocalizationDataId.UI_BodyParts;
                    break;
                case WordDataCategory.Clothes:
                    loc = LocalizationDataId.UI_Clothes;
                    break;
                case WordDataCategory.Color:
                    loc = LocalizationDataId.UI_Colors;
                    break;
                case WordDataCategory.Conjunctions:
                    loc = LocalizationDataId.UI_Conjunctions;
                    break;
                case WordDataCategory.Direction:
                    loc = LocalizationDataId.UI_Directions;
                    break;
                case WordDataCategory.Expressions:
                    loc = LocalizationDataId.UI_WordCat_Expressions;
                    break;
                case WordDataCategory.FamilyMember:
                    loc = LocalizationDataId.UI_FamilyMembers;
                    break;
                case WordDataCategory.Feeling:
                    loc = LocalizationDataId.UI_Feelings;
                    break;
                case WordDataCategory.Food:
                    loc = LocalizationDataId.UI_Food;
                    break;
                case WordDataCategory.Furniture:
                    loc = LocalizationDataId.UI_Furniture;
                    break;
                case WordDataCategory.General:
                    loc = LocalizationDataId.UI_General;
                    break;
                case WordDataCategory.Greetings:
                    loc = LocalizationDataId.UI_WordCat_Greetings;
                    break;
                case WordDataCategory.Verbs:
                    loc = LocalizationDataId.UI_WordCat_Verbs;
                    break;
                case WordDataCategory.Job:
                    loc = LocalizationDataId.UI_Jobs;
                    break;
                case WordDataCategory.Names:
                    loc = LocalizationDataId.UI_WordCat_Names;
                    break;
                case WordDataCategory.Nature:
                    loc = LocalizationDataId.UI_Nature;
                    break;
                case WordDataCategory.Number:
                    loc = LocalizationDataId.UI_Numbers;
                    break;
                case WordDataCategory.NumberOrdinal:
                    loc = LocalizationDataId.UI_NumbersOrdinal;
                    break;
                case WordDataCategory.People:
                    loc = LocalizationDataId.UI_People;
                    break;
                case WordDataCategory.Place:
                    loc = LocalizationDataId.UI_Places;
                    break;
                case WordDataCategory.Position:
                    loc = LocalizationDataId.UI_Positions;
                    break;
                case WordDataCategory.Question:
                    loc = LocalizationDataId.UI_Phrases_Questions;
                    break;
                case WordDataCategory.Shape:
                    loc = LocalizationDataId.UI_Shapes;
                    break;
                case WordDataCategory.Size:
                    loc = LocalizationDataId.UI_Size;
                    break;
                case WordDataCategory.Sport:
                    loc = LocalizationDataId.UI_Sports;
                    break;
                case WordDataCategory.Thing:
                    loc = LocalizationDataId.UI_Things;
                    break;
                case WordDataCategory.Time:
                    loc = LocalizationDataId.UI_Time;
                    break;
                case WordDataCategory.Vehicle:
                    loc = LocalizationDataId.UI_Vehicles;
                    break;
                case WordDataCategory.Weather:
                    loc = LocalizationDataId.UI_WordCat_Weather;
                    break;
            }
            return GetLocalizationData(loc);
        }

        public static LocalizationData GetPhraseCategoryData(PhraseDataCategory cat)
        {
            LocalizationDataId loc = LocalizationDataId.UI_None;
            switch (cat) {
                case PhraseDataCategory.Question:
                    loc = LocalizationDataId.UI_Phrases_Questions;
                    break;
                case PhraseDataCategory.Reply:
                    loc = LocalizationDataId.UI_Phrases_Replies;
                    break;
                case PhraseDataCategory.Greetings:
                    loc = LocalizationDataId.UI_Phrases_Greetings;
                    break;
                case PhraseDataCategory.Year:
                    loc = LocalizationDataId.UI_Phrases_Years;
                    break;
                case PhraseDataCategory.Sentence:
                    loc = LocalizationDataId.UI_Phrases_Sentences;
                    break;
                case PhraseDataCategory.Expression:
                    loc = LocalizationDataId.UI_Phrases_Expressions;
                    break;
            }
            return GetLocalizationData(loc);
        }
    }
}