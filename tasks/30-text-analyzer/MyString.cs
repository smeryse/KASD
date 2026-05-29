using System;
using System.Text;

namespace Task30
{
    public class MyString
    {
        private char[] value;
        private int length;

        public MyString()
        {
            value = new char[0];
            length = 0;
        }

        public MyString(char[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            this.value = new char[value.Length];
            Array.Copy(value, this.value, value.Length);
            this.length = value.Length;
        }

        public MyString(MyString original)
        {
            if (original == null)
                throw new ArgumentNullException(nameof(original));
            this.value = new char[original.length];
            Array.Copy(original.value, this.value, original.length);
            this.length = original.length;
        }

        public int Length() => length;

        public char CharAt(int index)
        {
            if (index < 0 || index >= length)
                throw new IndexOutOfRangeException("Index out of range");
            return value[index];
        }

        public MyString Substring(int beginIndex, int endIndex)
        {
            if (beginIndex < 0 || endIndex > length || beginIndex > endIndex)
                throw new ArgumentOutOfRangeException("Invalid substring indices");
            int len = endIndex - beginIndex;
            char[] sub = new char[len];
            Array.Copy(value, beginIndex, sub, 0, len);
            return new MyString(sub);
        }

        public MyString Concat(MyString str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            char[] result = new char[length + str.length];
            Array.Copy(value, result, length);
            Array.Copy(str.value, 0, result, length, str.length);
            return new MyString(result);
        }

        public bool Equals(MyString str)
        {
            if (str == null) return false;
            if (length != str.length) return false;
            for (int i = 0; i < length; i++)
                if (value[i] != str.value[i]) return false;
            return true;
        }

        public bool EqualsIgnoreCase(MyString str)
        {
            if (str == null) return false;
            if (length != str.length) return false;
            for (int i = 0; i < length; i++)
                if (char.ToLower(value[i]) != char.ToLower(str.value[i])) return false;
            return true;
        }

        public MyString ToLowerCase()
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = char.ToLower(value[i]);
            return new MyString(result);
        }

        public MyString ToUpperCase()
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = char.ToUpper(value[i]);
            return new MyString(result);
        }

        public MyString Trim()
        {
            int start = 0;
            int end = length - 1;
            while (start <= end && char.IsWhiteSpace(value[start])) start++;
            while (end >= start && char.IsWhiteSpace(value[end])) end--;
            int len = end - start + 1;
            if (len <= 0) return new MyString();
            char[] result = new char[len];
            Array.Copy(value, start, result, 0, len);
            return new MyString(result);
        }

        public MyString Replace(char oldChar, char newChar)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = value[i] == oldChar ? newChar : value[i];
            return new MyString(result);
        }

        public bool Contains(MyString substr)
        {
            if (substr == null) throw new ArgumentNullException(nameof(substr));
            if (substr.length == 0) return true;
            if (substr.length > length) return false;
            for (int i = 0; i <= length - substr.length; i++)
            {
                bool match = true;
                for (int j = 0; j < substr.length; j++)
                    if (value[i + j] != substr.value[j]) { match = false; break; }
                if (match) return true;
            }
            return false;
        }

        public int IndexOf(MyString substr)
        {
            if (substr == null) throw new ArgumentNullException(nameof(substr));
            if (substr.length == 0) return 0;
            if (substr.length > length) return -1;
            for (int i = 0; i <= length - substr.length; i++)
            {
                bool match = true;
                for (int j = 0; j < substr.length; j++)
                    if (value[i + j] != substr.value[j]) { match = false; break; }
                if (match) return i;
            }
            return -1;
        }

        public MyString[] Split(char delimiter)
        {
            int count = 1;
            for (int i = 0; i < length; i++)
                if (value[i] == delimiter) count++;

            MyString[] result = new MyString[count];
            int startIdx = 0;
            int idx = 0;
            for (int i = 0; i <= length; i++)
            {
                if (i == length || value[i] == delimiter)
                {
                    int len = i - startIdx;
                    char[] part = new char[len];
                    Array.Copy(value, startIdx, part, 0, len);
                    result[idx++] = new MyString(part);
                    startIdx = i + 1;
                }
            }
            return result;
        }

        public bool StartsWith(MyString prefix)
        {
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));
            if (prefix.length > length) return false;
            for (int i = 0; i < prefix.length; i++)
                if (value[i] != prefix.value[i]) return false;
            return true;
        }

        public bool EndsWith(MyString suffix)
        {
            if (suffix == null) throw new ArgumentNullException(nameof(suffix));
            if (suffix.length > length) return false;
            int offset = length - suffix.length;
            for (int i = 0; i < suffix.length; i++)
                if (value[offset + i] != suffix.value[i]) return false;
            return true;
        }

        public MyString Reverse()
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = value[length - 1 - i];
            return new MyString(result);
        }

        public static MyString ValueOf(int i)
        {
            return new MyString(i.ToString().ToCharArray());
        }

        public static MyString ValueOf(double d)
        {
            return new MyString(d.ToString(System.Globalization.CultureInfo.InvariantCulture).ToCharArray());
        }

        public static MyString ValueOf(bool b)
        {
            return new MyString(b.ToString().ToCharArray());
        }

        public override string ToString()
        {
            return new string(value, 0, length);
        }

        public override bool Equals(object obj)
        {
            if (obj is MyString other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < length; i++)
                hash = hash * 31 + value[i];
            return hash;
        }
    }
}
