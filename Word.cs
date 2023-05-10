using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Word
{
    public string ukr { get; set; }
    public string eng { get; set; }
    public List<string> SynonymsUkr { get; set; }
    public List<string> SynonymsEng { get; set; }
    public List<string> AlternateUkrTranslations { get; set; }
    public List<string> AlternateEngTranslations { get; set; }
    public Word(string ukr, string eng, List<string> synonymsUkr = null, List<string> synonymsEng = null)
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
        this.SynonymsUkr = synonymsUkr ?? new List<string>();
        this.SynonymsEng = synonymsEng ?? new List<string>();
        this.AlternateUkrTranslations = new List<string>();
        this.AlternateEngTranslations = new List<string>();
    }

    public bool IsUnique(List<Word> storage)
    {
        return !storage.Any(x => x.ukr == ukr || x.eng == eng ||
                                 x.SynonymsUkr.Contains(ukr) || x.SynonymsUkr.Contains(eng) ||
                                 x.SynonymsEng.Contains(ukr) || x.SynonymsEng.Contains(eng) ||
                                 x.AlternateUkrTranslations.Contains(ukr) || x.AlternateUkrTranslations.Contains(eng) ||
                                 x.AlternateEngTranslations.Contains(ukr) || x.AlternateEngTranslations.Contains(eng));
    }
}

