using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingTranslate
{
    class Word
    {
        public string ukr { get; set; }
        public string eng { get; set; }
        public Word(string ukr, string eng)
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
        }

    }
}
