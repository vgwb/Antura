using Antura.Core;
using Antura.LivingLetters;
using System;
using UnityEngine;

namespace Antura.Assessment
{
    public enum CategoryType
    {
        SunMoon,
        SingularDualPlural,
        WithOrWithoutArticle
    }

    /// <summary>
    /// This class is used to spawn LivingLetters representing a category
    /// (Sun/Moon Singular/Dual/Plural With/WithoutArticle).
    ///
    /// Refactoring Notes:
    /// There's actually only one concrete implementation for the interface,
    /// A interface can be added later for implementing LanguageXCategoryProvider
    ///
    /// You can reintroduce the interface later.. if needed.
    /// </summary>
    public class ArabicCategoryProvider
    {
        CategoryType categoryType;
        ILivingLetterData sun;
        ILivingLetterData moon;

        ILivingLetterData article;
        ILivingLetterData noArticle;
        ILivingLetterData singular;
        ILivingLetterData dual;
        ILivingLetterData plural;

        ILivingLetterData sunImage;
        ILivingLetterData moonImage;

        ILivingLetterData number1;
        ILivingLetterData number2;
        ILivingLetterData number3;

        const string sunString = "the_sun";
        const string moonString = "the_moon";
        const string articleString = "with_article";
        const string noArticleString = "without_article";
        const string singluarString = "singular";
        const string dualString = "dual";
        const string pluralString = "plural";

        const string sunString_Image = "the_sun";
        const string moonString_Image = "the_moon";

        const string oneString = "number_01";
        const string twoString = "number_02";
        const string threeString = "number_03";

        public ArabicCategoryProvider(CategoryType type)
        {
            categoryType = type;
            sun = GatherData(sunString);
            moon = GatherData(moonString);
            noArticle = GatherData(noArticleString);
            article = GatherData(articleString);
            singular = GatherData(singluarString);
            dual = GatherData(dualString);
            plural = GatherData(pluralString);

            sunImage = GatherImage(sunString_Image);
            moonImage = GatherImage(moonString_Image);

            number1 = GatherNumber(oneString);
            number2 = GatherNumber(twoString);
            number3 = GatherNumber(threeString);
        }

        private ILivingLetterData GatherData(string id)
        {
            var db = AppManager.I.DB;
            return db.GetWordDataById(id).ConvertToLivingLetterData();
        }

        private ILivingLetterData GatherImage(string id)
        {
            var db = AppManager.I.DB;
            return new LL_ImageData(db.GetWordDataById(id));
        }

        private ILivingLetterData GatherNumber(string id)
        {
            var db = AppManager.I.DB;
            return new LL_ImageData(db.GetWordDataById(id));
        }

        //finally decouple showed images from values expected in question builders
        // (otherwise add to attach a different view for certain characters)
        public bool Compare(int currentCategory, ILivingLetterData fromQuestionBuilder)
        {
            switch (categoryType)
            {
                case CategoryType.SunMoon:
                    if (currentCategory == 0)
                    {
                        return fromQuestionBuilder.Equals(sun);
                    }
                    else
                    {
                        return fromQuestionBuilder.Equals(moon);
                    }
                case CategoryType.SingularDualPlural:
                    switch (currentCategory)
                    {
                        case 0:
                            return fromQuestionBuilder.Equals(singular);
                        case 1:
                            return fromQuestionBuilder.Equals(dual);
                        default:
                            return fromQuestionBuilder.Equals(plural);
                    }

                case CategoryType.WithOrWithoutArticle:
                    if (currentCategory == 0)
                    {
                        return fromQuestionBuilder.Equals(article);
                    }
                    else
                    {
                        return fromQuestionBuilder.Equals(noArticle);
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public int GetCategories()
        {
            switch (categoryType)
            {
                case CategoryType.SunMoon:
                    return 2;
                case CategoryType.SingularDualPlural:
                    return 3;
                case CategoryType.WithOrWithoutArticle:
                    return 2;
                default:
                    throw new NotImplementedException();
            }
        }

        private GameObject QuestionView(ILivingLetterData data)
        {
            return ItemFactory.Instance.SpawnQuestion(data).gameObject;
        }

        public GameObject SpawnCustomObject(int currentCategory)
        {

            switch (categoryType)
            {
                case CategoryType.SunMoon:
                    if (currentCategory == 0)
                    {
                        return QuestionView(sunImage);
                    }
                    else
                    {
                        return QuestionView(moonImage);
                    }
                case CategoryType.SingularDualPlural:
                    switch (currentCategory)
                    {
                        case 0:
                            return QuestionView(number1);
                        case 1:
                            return QuestionView(number2);
                        default:
                            return QuestionView(number3);
                    }

                case CategoryType.WithOrWithoutArticle:
                    if (currentCategory == 0)
                    {
                        return QuestionView(article);
                    }
                    else
                    {
                        return QuestionView(noArticle);
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
