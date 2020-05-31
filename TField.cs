/* Copyright 2016-2019 Hermann Mundprecht
 * This file is licensed under the terms of the license 'CC BY 4.0'. 
 * For a human readable version of the license, see https://creativecommons.org/licenses/by/4.0/
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SAPCommon
{
    public class TField
    {
        private string name;
        private string value;
        private string fType;

        public TField()
        {
            name = "";
            value = "";
            fType = "S";
        }

        public TField(string Name, string Value, string FType = "S")
        {
            name = Name ?? "";
            value = Value ?? "";
            fType = FType;
        }

        public void SetValues(string Name, string Value, string FType = "S")
        {
            name = Name ?? "";
            value = Value ?? "";
            fType = FType;
        }

        public void Add(double Val)
        {
            Double val = Convert.ToDouble(value);
            val += Val;
            value = val.ToString();
        }

        public void Sub(double Val)
        {
            Double val = Convert.ToDouble(value);
            val -= Val;
            value = val.ToString();
        }

        public void Mul(double Val)
        {
            Double val = Convert.ToDouble(value);
            val *= Val;
            value = val.ToString();
        }

        public void Div(double Val)
        {
            Double val = Convert.ToDouble(value);
            val /= Val;
            value = val.ToString();
        }

        public string Name
        {
            get { return (string)this.name; }
            set { this.name = value; }
        }

        public string Value
        {
            get { return (string)this.value; }
            set { this.value = value; }
        }

        public string FType
        {
            get { return (string)this.fType; }
            set { this.fType = value; }
        }
    }

}
