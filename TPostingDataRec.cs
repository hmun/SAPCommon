using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SAPCommon
{

    public class TPostingDataRec
    {
        private Dictionary<string,TStrRec> tPostingDataRecDic;
        private string[] aNonKeyArray = Array.Empty<string>();
        private string[] aValueArray = Array.Empty<string>();


        public Dictionary<string, TStrRec> aTPostingDataRecDic
        {
            get { return (Dictionary<string, TStrRec>)this.tPostingDataRecDic; }
//            set { this.tPostingDataRecDic = aTPostingDataRecDic; }
        }

        public TPostingDataRec(ref TStr pPar, string pNr)
        {
            tPostingDataRecDic = new Dictionary<string, TStrRec>();
            initArrays(ref pPar, pNr);
        }
        public TPostingDataRec(ref Dictionary<string, TField> pDic, ref TStr pPar, string pNr, bool pEmpty = false, string pEmptyChar = "#")
        {
            TField aTField;
            initArrays(ref pPar, pNr);
            tPostingDataRecDic = new Dictionary<string, TStrRec>();
            foreach (KeyValuePair<string, TField> aKvb in pDic)
            {
                aTField = aKvb.Value;
                setValues(aTField.Name, aTField.Value, "", aTField.FType, pEmpty: pEmpty, pEmptyChar: pEmptyChar);
            }
        }

        private void initArrays(ref TStr pPar, string pNr)
        {
            string aNonKeyFields = !String.IsNullOrEmpty(pPar.value("GEN" + pNr, "NON_KEY_STR")) ? pPar.value("GEN" + pNr, "NON_KEY_STR") : "";
            if (!string.IsNullOrEmpty(aNonKeyFields))
                aNonKeyArray = aNonKeyFields.Split(',');
            string aValueFields = !String.IsNullOrEmpty(pPar.value("GEN" + pNr, "VALUE_STR")) ? pPar.value("GEN" + pNr, "VALUE_STR") : "Amount";
            if (!string.IsNullOrEmpty(aValueFields))
                aValueArray = aValueFields.Split(',');
        }

        public void setValues(string pNAME, string pVALUE, string pCURRENCY, string pFORMAT, bool pEmpty = false, string pEmptyChar = "#", string pOperation = "set")
        {
            TStrRec aTStrRec;
            string[] aNameArray;
            string aKey;
            string aSTRUCNAME = "";
            string aFIELDNAME = "";
            // do not add empty values
            if (!pEmpty & pVALUE == pEmptyChar)
                return;
            if (pNAME.Contains("-"))
            {
                char[] spearator = { '-' };
                aNameArray = pNAME.Split(spearator, 2,StringSplitOptions.None);
                aSTRUCNAME = aNameArray[0];
                aFIELDNAME = aNameArray[1];
            }
            else
            {
                aSTRUCNAME = "";
                aFIELDNAME = pNAME;
            }
            aKey = pNAME;
            if (tPostingDataRecDic.ContainsKey(aKey))
            {
                aTStrRec = tPostingDataRecDic[aKey];
                switch (pOperation)
                {
                    case "add":
                        {
                            aTStrRec.addValues(aSTRUCNAME, aFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
                            break;
                        }

                    case "sub":
                        {
                            aTStrRec.subValues(aSTRUCNAME, aFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
                            break;
                        }

                    case "mul":
                        {
                            aTStrRec.mulValues(aSTRUCNAME, aFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
                            break;
                        }

                    case "div":
                        {
                            aTStrRec.divValues(aSTRUCNAME, aFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
                            break;
                        }

                    default:
                        {
                            aTStrRec.setValues(aSTRUCNAME, aFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
                            break;
                        }
                }
            }
            else
            {
                aTStrRec = new TStrRec();
                aTStrRec.setValues(aSTRUCNAME, aFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
                tPostingDataRecDic.Add(aKey, aTStrRec);
            }
        }

        public void setValues(TPostingDataRec pTPostingDataRec, bool pEmpty = false, string pEmptyChar = "#", string pOperation = "set")
        {
            if (pTPostingDataRec != null)
            { 
                foreach (TStrRec aTStrRec in pTPostingDataRec.aTPostingDataRecDic.Values)
                    setValues(aTStrRec.getKey(), aTStrRec.Value, aTStrRec.Currency, aTStrRec.Format, pEmpty, pEmptyChar, pOperation);
            }
        }

        public void addAmounts(TPostingDataRec pTPostingDataRec, bool pEmpty = false, string pEmptyChar = "#")
        {
            if (pTPostingDataRec != null)
                foreach (TStrRec aTStrRec in pTPostingDataRec.aTPostingDataRecDic.Values)
                {
                    if (isValue(aTStrRec.getKey()))
                        setValues(aTStrRec.getKey(), aTStrRec.Value, aTStrRec.Currency, aTStrRec.Format, pEmpty, pEmptyChar, pOperation: "add");
                }
        }

        public void addValues(TPostingDataRec pTPostingDataRec, bool pEmpty = false, string pEmptyChar = "#")
        {
            if (pTPostingDataRec != null)
            { 
                foreach (TStrRec aTStrRec in pTPostingDataRec.aTPostingDataRecDic.Values)
                {
                    if (isValue(aTStrRec.getKey()))
                        setValues(aTStrRec.getKey(), aTStrRec.Value, aTStrRec.Currency, aTStrRec.Format, pEmpty, pEmptyChar, pOperation: "add");
                    else
                        setValues(aTStrRec.getKey(), aTStrRec.Value, aTStrRec.Currency, aTStrRec.Format, pEmpty, pEmptyChar, pOperation: "set");
                }
            }
        }

        public string getKey()
        {
            string aKey = "";
            foreach (TStrRec aTStrRec in tPostingDataRecDic.Values)
            {
                if (isKey(aTStrRec.getKey()))
                {
                    if (!string.IsNullOrEmpty(aKey))
                        aKey = aKey + "_";
                    aKey = aKey + aTStrRec.getKey() + "|" + aTStrRec.Value;
                }
            }
            return aKey;
        }

        public bool isZero()
        {
            bool _isZero = true;
            foreach (TStrRec aTStrRec in tPostingDataRecDic.Values)
            {
                if (isValue(aTStrRec.getKey()))
                {
                    if (Convert.ToDouble(aTStrRec.Value, CultureInfo.CurrentCulture) != 0)
                    {
                        _isZero = false;
                        break;
                    }
                }
            }
            return _isZero;
        }

        public bool isValue(string pName)
        {
            bool _isValue = false;
            int count;
            for (count = 0; count <= aValueArray.Length - 1; count++)
            {
                if (pName.Contains(aValueArray[count]))
                {
                    _isValue = true;
                    break;
                }
            }
        return _isValue;
        }

        public bool isKey(string pName)
        {
            bool _isKey = true;
            int count;
            for (count = 0; count <= aNonKeyArray.Length - 1; count++)
            {
                if (pName.Contains(aNonKeyArray[count]))
                {
                    _isKey = false;
                    break;
                }
            }
            return _isKey;
        }

    }
}
