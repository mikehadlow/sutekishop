using System;
using System.Collections.Generic;

namespace Suteki.Common.Tests.Spikes
{
    public class NamesSpike
    {
        public void Example()
        {
            var centre = new Centre
            {
                Name = new Text
                {
                    Description = "The centre's name",
                }
            };

            centre.Name.Translations.Add(new Translation
            {
                TranslationText = "Brighton", 
                Language = new Language
                {
                    Name = "English", 
                    IsDefault = true
                }
            });

            Console.WriteLine(centre.Name);
        }
    }

    public class Centre
    {
        public Text Name { get; set; }
    }

    public class Text
    {
        public string Description { get; set; }
        private readonly IList<Translation> translations = new List<Translation>();

        public IList<Translation> Translations
        {
            get { return translations; }
        }

        public override string ToString()
        {
            foreach (var translation in translations)
            {
                if (translation.Language.Name == Language.Current.Name) return translation.ToString();
            }
            throw new ApplicationException("No default translation found");
        }
    }

    public class Translation
    {
        public Text Text { get; set; }
        public Language Language { get; set; }
        public string TranslationText { get; set; }

        public override string ToString()
        {
            return TranslationText;
        }
    }

    public class Language
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public static Language Current
        {
            get
            {
                return new Language {Name = "English", IsDefault = true};
            }
        }
    }
}