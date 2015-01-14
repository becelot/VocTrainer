using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VocabularyTrainer
{
#pragma warning disable CS0659
    public class Vocabulary
#pragma warning restore CS0659
    {
        public string german { get; set; }
        public string japanese { get; set; }
        public string romaji { get; set; }
        public string cat { get; set; }
        public string lection { get; set; }

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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;


            if (!(obj is Vocabulary))
            {
                return false;
            }
            Vocabulary v = (Vocabulary)obj;
            return v.german == this.german && v.japanese == this.japanese;
        }
    }
}
