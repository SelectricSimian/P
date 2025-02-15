﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Plang.PrtSharp.Values
{
    [Serializable]
    public class PrtTuple : IPrtValue
    {
        public readonly List<IPrtValue> fieldValues;

        public PrtTuple()
        {
            fieldValues = new List<IPrtValue>();
        }


        public PrtTuple(params IPrtValue[] elems)
        {
            fieldValues = new List<IPrtValue>(elems.Count());
            foreach (var elem in elems) fieldValues.Add(elem?.Clone());
        }


        public IPrtValue this[int key]
        {
            get => fieldValues[key];
            set => fieldValues[key] = value;
        }


        public IPrtValue Clone()
        {
            var clone = new PrtTuple();
            foreach (var val in fieldValues) clone.fieldValues.Add(val?.Clone());
            return clone;
        }

        public bool Equals(IPrtValue val)
        {
            if (val is PrtNamedTuple) return false;
            var tupValue = val as PrtTuple;
            if (tupValue == null) return false;
            if (tupValue.fieldValues.Count != fieldValues.Count) return false;
            for (var i = 0; i < fieldValues.Count; i++)
                if (!PrtValues.SafeEquals(fieldValues[i],tupValue.fieldValues[i]))
                    return false;
            return true;
        }

        public void Update(int index, IPrtValue val)
        {
            fieldValues[index] = val;
        }

        public override int GetHashCode()
        {
            var hashCode = HashHelper.ComputeHash<IPrtValue>(fieldValues);
            return hashCode;
        }

        public override string ToString()
        {
            var retStr = "<";
            foreach (var field in fieldValues) retStr = retStr + field + ",";
            retStr += ">";
            return retStr;
        }
    }

    [Serializable]
    public class PrtNamedTuple : IPrtValue
    {
        public readonly List<IPrtValue> fieldValues;
        public List<string> fieldNames;

        public PrtNamedTuple()
        {
            fieldNames = new List<string>();
            fieldValues = new List<IPrtValue>();
        }


        public PrtNamedTuple(string[] fieldNames, params IPrtValue[] fieldValues)
        {
            this.fieldNames = fieldNames.ToList();
            this.fieldValues = fieldValues.ToList();
        }

        public IPrtValue this[string name]
        {
            get => fieldValues[fieldNames.IndexOf(name)];
            set => fieldValues[fieldNames.IndexOf(name)] = value;
        }

        public IPrtValue Clone()
        {
            var clone = new PrtNamedTuple();
            foreach (var name in fieldNames) clone.fieldNames.Add(name);
            foreach (var val in fieldValues) clone.fieldValues.Add(val.Clone());
            return clone;
        }

        public bool Equals(IPrtValue val)
        {
            if (!(val is PrtNamedTuple tup)) return false;
            if (tup.fieldValues.Count != fieldValues.Count) return false;
            for (var i = 0; i < tup.fieldValues.Count; i++)
            {
                if (fieldNames[i] != tup.fieldNames[i]) return false;
                if (!fieldValues[i].Equals(tup.fieldValues[i])) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return fieldValues.GetHashCode();
        }

        public override string ToString()
        {
            var retStr = "<";
            for (var i = 0; i < fieldValues.Count; i++) retStr += fieldNames[i] + ":" + fieldValues[i] + ", ";
            retStr += ">";
            return retStr;
        }
    }
}