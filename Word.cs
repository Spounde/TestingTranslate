using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Word
{
    public string ukr { get; set; }
    public string eng { get; set; }
    public List<string> Synonyms { get; set; }
    public List<string> Homonyms { get; set; }
    public List<string> AlternateUkrTranslations { get; set; } 
    public List<string> AlternateEngTranslations { get; set; } 
    public Word(string ukr, string eng, List<string> synonyms = null, List<string> homonyms = null)
    {
        if (string.IsNullOrEmpty(ukr))
        {
            throw new ArgumentException("ukr cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(eng))
        {
            throw new ArgumentException("eng cannot be null or empty.");
        }

        this.ukr = ukr;
        this.eng = eng;
        this.Synonyms = synonyms ?? new List<string>();
        this.Homonyms = homonyms ?? new List<string>();
        this.AlternateUkrTranslations = new List<string>();
        this.AlternateEngTranslations = new List<string>();
    }

    public bool IsUnique(List<Word> storage)
    {
        return !storage.Any(x => x.ukr == ukr || x.eng == eng ||
                                 x.Synonyms.Contains(ukr) || x.Synonyms.Contains(eng) ||
                                 x.Homonyms.Contains(ukr) || x.Homonyms.Contains(eng) ||
                                 x.AlternateUkrTranslations.Contains(ukr) || x.AlternateUkrTranslations.Contains(eng) ||
                                 x.AlternateEngTranslations.Contains(ukr) || x.AlternateEngTranslations.Contains(eng));
    }
}
