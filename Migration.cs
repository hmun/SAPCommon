/* Copyright 2016-2019 Hermann Mundprecht
 * This file is licensed under the terms of the license 'CC BY 4.0'. 
 * For a human readable version of the license, see https://creativecommons.org/licenses/by/4.0/
 */
 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using System.Globalization;

namespace SAPCommon
{
    public class Migration
    {
        private MigRulesSection mRS;
        private Mapper mapper = null;
        private CultureInfo cc = CultureInfo.CurrentCulture;

        public Migration(string configFile)
        {
            if (System.IO.File.Exists(configFile))
            {
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                configMap.ExeConfigFilename = configFile;
                System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                mRS = config.GetSection("MigRules") as MigRulesSection;
                if (mRS == null)
                {
                    mRS = new MigRulesSection();
                }
            }
        }

        public Migration()
        {
            this.mRS = new MigRulesSection();
        }

        public MigRulesSection MRS
        {
            get { return (MigRulesSection)this.mRS; }
            set { this.mRS = value; }
        }

        public void AddRule(String postingType, string target, string ruleType, string source)
        {
            mRS.MigRules.Add(new MigRule(postingType, target, ruleType, source));
        }

        public void AddPattern(String postingType, string target, string value)
        {
            mRS.MigPatterns.Add(new MigPattern(postingType, target, value));
        }

        public void AddConstant(String postingType, string target, string value)
        {
            mRS.MigConstants.Add(new MigConstant(postingType, target, value));
        }

        public void AddFormula(String postingType, string target, string value)
        {
            mRS.MigFormulas.Add(new MigFormula(postingType, target, value));
        }

        public void AddMapping(String source, string target, string sourcevalue, string targetvalue)
        {
            mRS.MigMappings.Add(new MigMapping(source, target, sourcevalue, targetvalue));
        }

        public Boolean ContainsSource(string postingtype, string source)
        {
            foreach (MigRule rule in mRS.MigRules)
            {   
                if (rule.PostingType == postingtype && rule.Source == source)
                    return true;
            }
            return false;             
        }
        public Dictionary<string, TField> ApplyRules(Dictionary<string, TField> basis, string postingtype)
        {
            string value;
            TField tfield;
            Dictionary<string, TField> col = new Dictionary<string, TField>();

            if (mapper == null)
                mapper = new Mapper(mRS);

            foreach (MigRule rule in mRS.MigRules)
            {
                value = "";
                tfield = new TField("","");
                if (rule.PostingType == postingtype)
                {
                    if (String.IsNullOrEmpty(rule.RuleType))
                    {
                        // copy the value respecting #=""
                        if (basis.ContainsKey(rule.Source))
                        {
                            value = basis[rule.Source].Value;
                            if (basis[rule.Source].Value == "#" || string.IsNullOrEmpty(basis[rule.Source].Value))
                                value = "";
                        }
                        tfield = new TField(rule.Target, value);
                    }
                    else if (rule.RuleType.Substring(0, 1) == "V" || rule.RuleType.Substring(0, 1) == "R")
                    {
                        try
                        {
                            Double val;
                            String decimals = rule.RuleType.Substring(1, rule.RuleType.Length - 1);
                            if (basis[rule.Source].Value == "#" || string.IsNullOrEmpty(basis[rule.Source].Value))
                            {
                                val = 0;
                            } else { 
                                if (String.IsNullOrEmpty(decimals))
                                    decimals = "2";
                                val = Convert.ToDouble(basis[rule.Source].Value, cc);
                                if (rule.RuleType.Substring(0, 1) == "R")
                                    val *= -1;
                            }
                            value = val.ToString("F" + decimals, cc);
                        }
                        catch (System.Exception)
                        {
                            value = "";
                        }
                        tfield = new TField(rule.Target, value);
                    }
                    else if (rule.RuleType == "C")
                    {
                        try
                        {
                            value = MRS.MigConstants[postingtype + "|" + rule.Target].Value;
                        } catch (System.Exception) {
                            value = "";
                        }
                        tfield = new TField(rule.Target, value);
                    }
                    else if (rule.RuleType == "F")
                    {
                        try
                        {
                            value = MRS.MigFormulas[postingtype + "|" + rule.Target].Value;
                        }
                        catch (System.Exception)
                        {
                            value = "";
                        }
                        tfield = new TField(rule.Target, value, "F");
                    }
                    else if (rule.RuleType == "M")
                    {
                        string sourceval = basis[rule.Source].Value ?? "";
                        value = mapper.Map(rule.Target, rule.Source, sourceval);
                        tfield = new TField(rule.Target, value, "S");
                    }
                    else if (rule.RuleType == "P")
                    {
                        try
                        {
                            string pattern = MRS.MigPatterns[postingtype + "|" + rule.Target].Value;
                            string sourceval = basis[rule.Source].Value ?? "";
                            value = SAPFormat.applyPattern(sourceval, pattern);
                            tfield = new TField(rule.Target, value, "S");
                        }
                        catch (System.Exception)
                        {
                            value = "";
                        }
                        tfield = new TField(rule.Target, value);
                    }
                    col[tfield.Name] = tfield;
                }
            }
            return col;
        }
    }

    public class Mapper
    {
        private Dictionary<string, Dictionary<string,string>> fieldDict = 
            new Dictionary<string, Dictionary<string, string>>();

        public Mapper(MigRulesSection mrs)
        {
            Dictionary<string, string> valueDict;
            foreach (MigMapping mapping in mrs.MigMappings)
            {
                string key = mapping.Target + "|" + mapping.Source;
                string sourceval = mapping.SourceValue ?? "";
                if (!fieldDict.ContainsKey(key))
                    fieldDict[key] = new Dictionary<string, string>();
                valueDict = fieldDict[key];
                if (valueDict.ContainsKey(sourceval))
                    valueDict[sourceval] = mapping.TargetValue;
                else
                    valueDict.Add(sourceval, mapping.TargetValue);
            }
        }

        public string Map(string targetfield, string sourcfield, string sourcevalue)
        {
            Dictionary<string, string> valueDict;
            string key = targetfield + "|" + sourcfield;
            string ret = "";
            if (fieldDict.ContainsKey(key))
            {
                valueDict = fieldDict[key];
                if (valueDict.ContainsKey(sourcevalue))
                    ret = valueDict[sourcevalue];
            }
            return ret;
        }

    }
}
