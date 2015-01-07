using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VocabularyTrainer
{
    public class Vocabulary
    {
        public string german;
        public string japanese;
        public string romaji;
        public string cat;
        public string lection;

        public Vocabulary(string german, string japanese, string romaji, string cat, string lection)
        {
            this.german = german;
            this.japanese = japanese;
            this.romaji = romaji;
            this.cat = cat;
            this.lection = lection;
        }

        public Vocabulary() {
        }
    }
}
