using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UserInterface
{
    [Serializable]
    [XmlRoot("NNItem")]
    public class NNItem
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Date")]
        public string Date { get; set; }

        [XmlAttribute("Goal")]
        public string Goal { get; set; }

        [XmlAttribute("Samples")]
        public int Samples { get; set; }

        [XmlAttribute("Sgood")]
        public int Sgood { get; set; }

        [XmlAttribute("Sbad")]
        public int Sbad { get; set; }

        [XmlAttribute("EpochCount")]
        public int EpochCount { get; set; }

        [XmlAttribute("Test")]
        public int Test { get; set; }

        [XmlAttribute("Accuracy")]
        public double Accuracy { get; set; }

        [XmlAttribute("FirstMistake")]
        public double FirstMistake { get; set; }

        [XmlAttribute("SecondMistake")]
        public double SecondMistake { get; set; }

         public NNItem() { }

        public NNItem(string[] bdentry)
        {
            if (bdentry.Length == 11)
            {
                ID = Convert.ToInt32(bdentry[0]);
                Date = bdentry[1];
                Goal = bdentry[2];
                Samples = Convert.ToInt32(bdentry[3]);
                Sgood = Convert.ToInt32(bdentry[4]);
                Sbad = Convert.ToInt32(bdentry[5]);
                EpochCount = Convert.ToInt32(bdentry[6]);
                Test = Convert.ToInt32(bdentry[7]);
                Accuracy = Convert.ToDouble(bdentry[8]);
                FirstMistake = Convert.ToDouble(bdentry[9]);
                SecondMistake = Convert.ToDouble(bdentry[10]);
            }
        }        
    }
}
