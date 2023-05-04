using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestingTranslate
{
    static class WordsDictionary
    {
        public static List<Word> storage = LoadData();

        public static void Add(string ukr, string eng)
        {
            storage.Add(new Word(ukr, eng));
            SaveData();
        }

        public static void SaveData()
        {
            if (storage == null)
            {
                throw new InvalidOperationException("Storage cannot be null.");
            }

            string jsonString = JsonSerializer.Serialize(storage);
            File.WriteAllText("storage.json", jsonString);
        }


        static List<Word> LoadData()
        {
            if (!File.Exists("storage.json"))
            {
                return new List<Word>();
            }

            string jsonString = File.ReadAllText("storage.json");
            List<Word> words = JsonSerializer.Deserialize<List<Word>>(jsonString);

            if (words == null)
            {
                throw new InvalidOperationException("Deserialized words list cannot be null.");
            }

            return words;
        }


    }
}
