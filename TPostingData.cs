using System;
using System.Collections.Generic;
using System.Text;

namespace SAPCommon
{
    public class TPostingData
    {
        private Dictionary<string, TPostingDataRec> tPostingDataDic;
        private TStr aPar;
        private string aNr = "";

        public Dictionary<string, TPostingDataRec> aTPostingDataDic
        {
            get { return (Dictionary<string, TPostingDataRec>)this.tPostingDataDic; }
//            set { this.aTPostingDataDic = pTPostingDataDic; }
        }

        public TPostingData(ref TStr pPar, string pNr)
        {
            tPostingDataDic = new Dictionary<string, TPostingDataRec>();
            aPar = pPar;
            aNr = pNr;
        }

        public void addTPostingDataRec(string pKey, TPostingDataRec pTPostingDataRec)
        {
            TPostingDataRec aTPostingDataRec;
            if (tPostingDataDic.ContainsKey(pKey))
            {
                aTPostingDataRec = tPostingDataDic[pKey];
                aTPostingDataRec.addAmounts(pTPostingDataRec);
            }
            else
            {
                aTPostingDataRec = new TPostingDataRec(ref aPar, aNr);
                aTPostingDataRec.addValues(pTPostingDataRec);
                tPostingDataDic.Add(pKey, aTPostingDataRec);
            }
        }

        public TPostingDataRec newTPostingDataRec(ref Dictionary<string, TField> pDic, bool pEmpty = false, string pEmptyChar = "#")
        {
            return new TPostingDataRec(pDic: ref pDic, pPar: ref aPar, pEmpty: pEmpty, pEmptyChar: pEmptyChar, pNr: aNr);
        }

        public void addValue(string pKey, string pNAME, string pVALUE, string pCURRENCY, string pFORMAT, bool pEmty = false, string pEmptyChar = "#", string pOperation = "set")
        {
            TPostingDataRec aTPostingDataRec;
            if (tPostingDataDic.ContainsKey(pKey))
            {
                aTPostingDataRec = tPostingDataDic[pKey];
                aTPostingDataRec.setValues(pNAME, pVALUE, pCURRENCY, pFORMAT, pEmty, pEmptyChar, pOperation);
            }
            else
            {
                aTPostingDataRec = new TPostingDataRec(ref aPar, aNr);
                aTPostingDataRec.setValues(pNAME, pVALUE, pCURRENCY, pFORMAT, pEmty, pEmptyChar, pOperation);
                tPostingDataDic.Add(pKey, aTPostingDataRec);
            }
        }

        public void addValue(string pKey, TStrRec pTStrRec, bool pEmty = false, string pEmptyChar = "#", string pOperation = "set", string pNewStrucname = "")
        {
            TPostingDataRec aTPostingDataRec;
            string aName;
            if (pTStrRec != null)
            {
                if (!String.IsNullOrEmpty(pNewStrucname))
                    aName = pNewStrucname + "-" + pTStrRec.Fieldname;
                else
                    aName = pTStrRec.Strucname + "-" + pTStrRec.Fieldname;
                if (tPostingDataDic.ContainsKey(pKey))
                {
                    aTPostingDataRec = tPostingDataDic[pKey];
                    aTPostingDataRec.setValues(aName, pTStrRec.Value, pTStrRec.Currency, pTStrRec.Format, pEmty, pEmptyChar, pOperation);
                }
                else
                {
                    aTPostingDataRec = new TPostingDataRec(ref aPar, aNr);
                    aTPostingDataRec.setValues(aName, pTStrRec.Value, pTStrRec.Currency, pTStrRec.Format, pEmty, pEmptyChar, pOperation);
                    tPostingDataDic.Add(pKey, aTPostingDataRec);
                }
            }
        }

        public void delData(string pKey)
        {
            tPostingDataDic.Remove(pKey);
        }
    }
}
