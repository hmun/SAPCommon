using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;

namespace SAPCommon
{

    public class SapExtension
    {
        private TExt extFields;
        private string extString;
        public SapExtension(TStr intPar)
        {
            extString = new string(' ', 960);
            this.extFields = new TExt();
            if (intPar != null) { 
                foreach (KeyValuePair<string, TStrRec> kvP in intPar.getData())
                {
                    TStrRec par = kvP.Value;
                    if (par.Strucname == "EXT")
                    {
                        extFields.add(par);
                    }
                }
            }
        }

        public void addField(TStrRec aStrRec)
        {
            if (aStrRec != null) { 
                TExtRec aTExtRec = extFields.get(aStrRec.Strucname,aStrRec.Fieldname);
                if (!String.IsNullOrEmpty( aTExtRec.Fieldname))
                {
                    extString = strReplaceAt(extString, aTExtRec.Pos, aTExtRec.Len, aStrRec.formated());
                }
            }
        }

        public void addString(string str, int pos, int len) 
        {
            SAPFormat sapFormat = new SAPFormat();
            if (str != null)
            {
                 extString = strReplaceAt(extString, pos, len, sapFormat.text(str,len));
            }
        }

        public IEnumerable<string> getArray(int partSize = 240, int fullLength = 960)
        {
            string rest = "";
            if (extString.Length < fullLength) {  
                rest = new string(' ', fullLength - extString.Length);
            }
            string outStr = extString + rest;
            IEnumerable<string> aArray = Enumerable.Range(0, outStr.Length / partSize).Select(i => outStr.Substring(i * partSize, partSize));
            return aArray;
        }

        private string strReplaceAt(string str, int pos, int len, string replace)
        {
            return str.Remove(pos, Math.Min(len, str.Length - pos)).Insert(pos, replace);
        }


    }

}
