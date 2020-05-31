/* Copyright 2016-2019 Hermann Mundprecht
 * This file is licensed under the terms of the license 'CC BY 4.0'. 
 * For a human readable version of the license, see https://creativecommons.org/licenses/by/4.0/
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SAPCommon
{

    public class MigRulesSection : ConfigurationSection
    {
        [ConfigurationProperty("mig_rules", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MigRulesCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public MigRulesCollection MigRules
        {
            get { MigRulesCollection migRulesCollection = (MigRulesCollection)base["mig_rules"];
                  return migRulesCollection; }
            set { MigRulesCollection migRulesCollection = value; }
        }

        [ConfigurationProperty("mig_constants", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MigConstantsCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public MigConstantsCollection MigConstants
        {
            get { MigConstantsCollection migConstantsCollection = (MigConstantsCollection)base["mig_constants"];
                  return migConstantsCollection; }
            set { MigConstantsCollection migConstantsCollection = value; }
        }

        [ConfigurationProperty("mig_formulas", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MigFormulasCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public MigFormulasCollection MigFormulas
        {
            get { MigFormulasCollection migFormulasCollection = (MigFormulasCollection)base["mig_formulas"];
                  return migFormulasCollection; }
            set { MigFormulasCollection migFormulasCollection = value; }
        }

        [ConfigurationProperty("mig_mappings", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MigMappingsCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public MigMappingsCollection MigMappings
        {
            get { MigMappingsCollection migMappingsCollection = (MigMappingsCollection)base["mig_mappings"];
                  return migMappingsCollection; }
            set { MigMappingsCollection migMappingsCollection = value; }
        }

        // Create a new instance of the MigRulesSection.
        // This constructor creates a configuration element 
        // using the MigRule default values.
        // It assigns this element to the collection.
        public MigRulesSection()
        {
            // MigRule migRule = new MigRule();
            // MigRules.Add(migRule);
            // MigConstant migConstant = new MigConstant();
            // MigConstants.Add(migConstant);
            // MigFormula migFormula = new MigFormula();
            // MigFormulas.Add(migFormula);
            // MigMapping MigMapping = new MigMapping();
            // MigMappings.Add(MigMapping);
        }
    }

    public class MigRulesCollection : ConfigurationElementCollection
    {

        public MigRulesCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MigRule();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MigRule)element).Key;
        }

        public MigRule this[int index]
        {
            get
            {
                return (MigRule)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public MigRule this[string Key]
        {
            get
            {
                return (MigRule)BaseGet(Key);
            }
        }


        public int IndexOf(MigRule migRule)
        {
            return BaseIndexOf(migRule);
        }

        public void Add(MigRule migRule)
        {
            BaseAdd(migRule);
            // Your custom code goes here.
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            // Your custom code goes here.
        }

        public void Remove(MigRule migRule)
        {
            if (BaseIndexOf(migRule) >= 0)
            {
                BaseRemove(migRule.Key);
                // Your custom code goes here.
                Console.WriteLine("MigRulesCollection: {0}", "Removed collection element!");
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
            // Your custom code goes here.
        }

        public void Remove(string name)
        {
            BaseRemove(name);
            // Your custom code goes here.
        }

        public void Clear()
        {
            BaseClear();
            // Your custom code goes here.
            Console.WriteLine("MigRulesCollection: {0}", "Removed entire collection!");
        }

    }

    public class MigRule : System.Configuration.ConfigurationElement
    {

        public MigRule(String postingType, string target, string ruleType, string source)
        {
            this.Key = postingType + "|" + target;
            this.PostingType = postingType;
            this.Target = target;
            this.RuleType = ruleType;
            this.Source = source;
        }

        public MigRule()
        {
        }

        [ConfigurationProperty("Key", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["Key"]; }
            set { this["Key"] = value; }
        }

        [ConfigurationProperty("PostingType", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string PostingType
        {
            get { return (string)this["PostingType"]; }
            set { this["PostingType"] = value; }
        }

        [ConfigurationProperty("Target", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Target
        {
            get { return (string)this["Target"]; }
            set { this["Target"] = value; }
        }

        [ConfigurationProperty("RuleType", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string RuleType
        {
            get { return (string)this["RuleType"]; }
            set { this["RuleType"] = value; }
        }

        [ConfigurationProperty("Source", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Source
        {
            get { return (string)this["Source"]; }
            set { this["Source"] = value; }
        }
    }

    public class MigConstantsCollection : ConfigurationElementCollection
    {

        public MigConstantsCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MigConstant();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MigConstant)element).Key;
        }

        public MigConstant this[int index]
        {
            get
            {
                return (MigConstant)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public MigConstant this[string Key]
        {
            get
            {
                return (MigConstant)BaseGet(Key);
            }
        }


        public int IndexOf(MigConstant migConstant)
        {
            return BaseIndexOf(migConstant);
        }

        public void Add(MigConstant migConstant)
        {
            BaseAdd(migConstant);
            // Your custom code goes here.
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            // Your custom code goes here.
        }

        public void Remove(MigConstant migConstant)
        {
            if (BaseIndexOf(migConstant) >= 0)
            {
                BaseRemove(migConstant.Key);
                // Your custom code goes here.
                Console.WriteLine("MigConstantsCollection: {0}", "Removed collection element!");
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
            // Your custom code goes here.
        }

        public void Remove(string name)
        {
            BaseRemove(name);
            // Your custom code goes here.
        }

        public void Clear()
        {
            BaseClear();
            // Your custom code goes here.
            Console.WriteLine("MigConstantsCollection: {0}", "Removed entire collection!");
        }

    }

    public class MigConstant : System.Configuration.ConfigurationElement
    {

        public MigConstant(String postingType, string target, string value)
        {
            this.Key = postingType + "|" + target;
            this.PostingType = postingType;
            this.Target = target;
            this.Value = value;
        }

        public MigConstant()
        {
        }

        [ConfigurationProperty("Key", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["Key"]; }
            set { this["Key"] = value; }
        }

        [ConfigurationProperty("PostingType", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string PostingType
        {
            get { return (string)this["PostingType"]; }
            set { this["PostingType"] = value; }
        }

        [ConfigurationProperty("Target", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Target
        {
            get { return (string)this["Target"]; }
            set { this["Target"] = value; }
        }

        [ConfigurationProperty("Value", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Value
        {
            get { return (string)this["Value"]; }
            set { this["Value"] = value; }
        }
    }

    public class MigFormulasCollection : ConfigurationElementCollection
    {

        public MigFormulasCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MigFormula();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MigFormula)element).Key;
        }

        public MigFormula this[int index]
        {
            get
            {
                return (MigFormula)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public MigFormula this[string Key]
        {
            get
            {
                return (MigFormula)BaseGet(Key);
            }
        }


        public int IndexOf(MigFormula migFormula)
        {
            return BaseIndexOf(migFormula);
        }

        public void Add(MigFormula migFormula)
        {
            BaseAdd(migFormula);
            // Your custom code goes here.
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            // Your custom code goes here.
        }

        public void Remove(MigFormula migFormula)
        {
            if (BaseIndexOf(migFormula) >= 0)
            {
                BaseRemove(migFormula.Key);
                // Your custom code goes here.
                Console.WriteLine("MigFormulasCollection: {0}", "Removed collection element!");
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
            // Your custom code goes here.
        }

        public void Remove(string name)
        {
            BaseRemove(name);
            // Your custom code goes here.
        }

        public void Clear()
        {
            BaseClear();
            // Your custom code goes here.
            Console.WriteLine("MigFormulasCollection: {0}", "Removed entire collection!");
        }

    }

    public class MigFormula : System.Configuration.ConfigurationElement
    {

        public MigFormula(String postingType, string target, string value)
        {
            this.Key = postingType + "|" + target;
            this.PostingType = postingType;
            this.Target = target;
            this.Value = value;
        }

        public MigFormula()
        {
        }

        [ConfigurationProperty("Key", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["Key"]; }
            set { this["Key"] = value; }
        }

        [ConfigurationProperty("PostingType", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string PostingType
        {
            get { return (string)this["PostingType"]; }
            set { this["PostingType"] = value; }
        }

        [ConfigurationProperty("Target", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Target
        {
            get { return (string)this["Target"]; }
            set { this["Target"] = value; }
        }

        [ConfigurationProperty("Value", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Value
        {
            get { return (string)this["Value"]; }
            set { this["Value"] = value; }
        }
    }

    public class MigMappingsCollection : ConfigurationElementCollection
    {

        public MigMappingsCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MigMapping();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MigMapping)element).Key;
        }

        public MigMapping this[int index]
        {
            get
            {
                return (MigMapping)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public MigMapping this[string Key]
        {
            get
            {
                return (MigMapping)BaseGet(Key);
            }
        }


        public int IndexOf(MigMapping migMapping)
        {
            return BaseIndexOf(migMapping);
        }

        public void Add(MigMapping migMapping)
        {
            BaseAdd(migMapping);
            // Your custom code goes here.
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            // Your custom code goes here.
        }

        public void Remove(MigMapping migMapping)
        {
            if (BaseIndexOf(migMapping) >= 0)
            {
                BaseRemove(migMapping.Key);
                // Your custom code goes here.
                Console.WriteLine("MigMappingsCollection: {0}", "Removed collection element!");
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
            // Your custom code goes here.
        }

        public void Remove(string name)
        {
            BaseRemove(name);
            // Your custom code goes here.
        }

        public void Clear()
        {
            BaseClear();
            // Your custom code goes here.
            Console.WriteLine("MigMappingsCollection: {0}", "Removed entire collection!");
        }

    }

    public class MigMapping : System.Configuration.ConfigurationElement
    {

        public MigMapping(String source, string target, string sourcevalue, string targetvalue)
        {
            this.Key = source + "|" + target + "|" + sourcevalue;
            this.Source = source;
            this.Target = target;
            this.SourceValue = sourcevalue;
            this.TargetValue = targetvalue;
        }

        public MigMapping()
        {
        }

        [ConfigurationProperty("Key", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["Key"]; }
            set { this["Key"] = value; }
        }

        [ConfigurationProperty("Source", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Source
        {
            get { return (string)this["Source"]; }
            set { this["Source"] = value; }
        }

        [ConfigurationProperty("Target", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Target
        {
            get { return (string)this["Target"]; }
            set { this["Target"] = value; }
        }

        [ConfigurationProperty("SourceValue", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string SourceValue
        {
            get { return (string)this["SourceValue"]; }
            set { this["SourceValue"] = value; }
        }

        [ConfigurationProperty("TargetValue", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string TargetValue
        {
            get { return (string)this["TargetValue"]; }
            set { this["TargetValue"] = value; }
        }
    }

}
