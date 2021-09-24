using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace SAPCommon
{
    public class SAPFormat
    {

        private TStr wbsMask;

        public SAPFormat(TStr intPar)
        {
            this.wbsMask = new TStr();
            if (intPar != null)
            { 
                foreach(KeyValuePair<string,TStrRec> kvP in intPar.getData())
                {
                    TStrRec par = kvP.Value;
                    if (par.Strucname == "WBS_MASK")
                    {
                        wbsMask.add(par.Strucname, par.Fieldname, par.Value, "", "");
                    }
                }
            }
        }

        public SAPFormat()
        {
            this.wbsMask = new TStr();
        }

        public string dec(string val, int dec)
        {
            string aRet = "";
            string aDecFormat = "F";
            int aDec = dec;
            aDecFormat += aDec.ToString(CultureInfo.CurrentCulture);
            if (!String.IsNullOrEmpty(val))
                // aRet = Convert.ToDouble(Value).ToString(aDecFormat, CultureInfo.CreateSpecificCulture("en-US"));
                aRet = Convert.ToDouble(val, CultureInfo.CurrentCulture).ToString(aDecFormat, CultureInfo.CurrentCulture);
            else
                // aRet = Convert.ToDouble("0").ToString(aDecFormat, CultureInfo.CreateSpecificCulture("en-US"));
                aRet = Convert.ToDouble("0", CultureInfo.CurrentCulture).ToString(aDecFormat, CultureInfo.CurrentCulture);
            return aRet;
        }

        public string unpack(string val, int length)
        {
            string aRet = val;
            if (!String.IsNullOrEmpty(val))
            {
                aRet = val.ToUpper(CultureInfo.CurrentCulture);
                string formatString = "D";
                var isNumeric = int.TryParse(val, out _);
                if (isNumeric)
                {
                    if (length - val.Length > 0)
                    {
                        formatString += Convert.ToString(length - val.Length, CultureInfo.CurrentCulture);
                        aRet = Convert.ToUInt16("0", CultureInfo.CurrentCulture).ToString(formatString, CultureInfo.CurrentCulture) + val;
                    }
                }
            }
            return aRet;
        }

        public string text(string val, int length)
        {
            string aRet = val;
            int len = length - val.Length;
            if (!String.IsNullOrEmpty(val))
            {
                if (len > 0)
                    aRet = val + new string(' ', len);
                else
                    aRet = val.Substring(0, length);
            } else {
                aRet = new string(' ', length);
            }
            return aRet;
        }

        public string text_right(string val, int length)
        {
            string aRet = val;
            int len = length - val.Length;
            if (!String.IsNullOrEmpty(val))
            {
                if (len > 0)
                    aRet = new string(' ', len) + val;
                else
                    aRet = val.Substring(0, length);
            }
            else
            {
                aRet = new string(' ', length);
            }
            return aRet;
        }

        public string pspid(string val, int length)
        {
            string aRet = val;
            string formatString = "D";
            String aDot = ".";
            String aEmpty = "";
            bool useMask = false;
            string maskStr = "";
            if (!String.IsNullOrEmpty(val))
            {
                String[] parts = val.Split('.');
                if (this.wbsMask.getData().Count != 0)
                {
                    if (!String.IsNullOrEmpty(this.wbsMask.value("WBS_MASK", parts[0])))
                    {
                        useMask = true;
                        maskStr = this.wbsMask.value("WBS_MASK", parts[0]);
                    }
                }
                aRet = aRet.Replace(aDot, aEmpty).ToUpper(CultureInfo.CurrentCulture);
                if (val.IndexOfAny(new char[] { '-', '_', ',' }) != -1)
                    return val.ToUpper(CultureInfo.CurrentCulture);
                if (useMask)
                {
                    int ncount = maskStr.Length - aRet.Length;
                    if (ncount > 0)
                        aRet += maskStr.Substring(maskStr.Length - ncount, ncount); //add right ncount chars of mask to aRet
                }
                else
                {
                    if (length - aRet.Length > 0)
                    {
                        formatString += Convert.ToString(length - aRet.Length, CultureInfo.CurrentCulture);
                        aRet = aRet + Convert.ToUInt16("0", CultureInfo.CurrentCulture).ToString(formatString, CultureInfo.CurrentCulture);
                    }
                }
            }
            return aRet;
        }

    }
}
