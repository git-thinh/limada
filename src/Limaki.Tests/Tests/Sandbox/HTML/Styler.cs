using System;

using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Limaki.Common.Text.HTML {


    /**Styler arrangiert eine Menge von Styles zu einem - vern�nftigen - Style-string und eliminiert idente Styles*/
    public class Styler {
        /**Liste der styles*/
        private List<Style> styles = new List<Style>();

        /**Konstruktor ohne Parameter - Styles k�nnen nachtr�glich angef�gt werden*/
        public Styler() { }

        /**Konstruktor mit �bergabe einer List an Style-Objekten*/
        public Styler(List<Style> styles) {
            this.styles.AddRange(styles);
        }
        
        /**Anf�gen eines Style-Objekts an die Styles-Liste*/
        public void AddStyle(Style style) {
            this.styles.Add(style);
        }
        /**Anf�gen einer Styles-Liste an die Styles-Liste*/
        public void AddStyles(IEnumerable<Style> styles) {
            this.styles.AddRange(styles);
        }

        /**Gibt die Liste der Styles zur�ck*/
        public IEnumerable<Style> Styles {
            get { return this.styles; }
        }
        /**Gibt die Styles als CSS-Text zur�ck*/
        public string CSS(bool sorted, bool noidents) {
            IEnumerable<Style> list = this.styles;
            if (noidents) {
                list = list.Distinct();
            }

            if (sorted) {
                list = list.OrderBy(s => s.Name);
            }
            
            var result = new StringBuilder("");
            foreach (var style in list) {
                result.Append(style.Name);
                result.Append("{\r\n");
                foreach(var att in style.AttributesAndValues){
                    result.Append("\t");
                    result.Append(att.Key);
                    result.Append(":");
                    result.Append(att.Value);
                    result.Append(";\r\n");
                }
                result.Append("}\r\n");
            }
            return result.ToString();
        }
    }
}