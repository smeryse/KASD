using System;
using System.IO;
using Task10.Collection;


class Program
{
    static bool IsValidOctet(string str)
    {
        if (str.Length == 0) return false;
        if (str.Length > 1 && str[0] == '0') return false;

        int val = 0;
        foreach (char c in str)
        {
            if (!char.IsDigit(c)) return false;
            val = val * 10 + (c - '0');
        }
        return val >= 0 && val <= 255;
    }

    static bool TryParseIpAt(string line, int start, out string ip, out int endPos)
    {
        ip = null;
        endPos = start;

        string[] parts = new string[4];
        int idx = 0;
        int i = start;

        while (i < line.Length && idx < 4)
        {
            int numStart = i;
            while (i < line.Length && char.IsDigit(line[i]))
                i++;

            if (numStart == i) return false;

            parts[idx] = line.Substring(numStart, i - numStart);

            if (idx < 3)
            {
                if (i >= line.Length || line[i] != '.')
                    return false;
                i++; 
            }

            idx++;
        }

        if (idx != 4) return false;

        foreach (var p in parts)
            if (!IsValidOctet(p)) return false;

        if (i < line.Length && char.IsDigit(line[i])) return false;
        if (start > 0 && char.IsDigit(line[start - 1])) return false;

        ip = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
        endPos = i;
        return true;
    }

    static void ExtractIpsFromLine(string line, MyVector<string> outVec)
    {
        int i = 0;

        while (i < line.Length)
        {
            if (char.IsDigit(line[i]))
            {
                if (TryParseIpAt(line, i, out string ip, out int endPos))
                {
                    outVec.Add(ip);
                    i = endPos;
                    continue;
                }
            }
            i++;
        }
    }

    static void Main(string[] args)
    {
        MyVector<string> lines = new MyVector<string>();
        MyVector<string> ips   = new MyVector<string>();

        using (var sr = new StreamReader("input.txt"))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
                lines.Add(s);
        }

        for (int i = 0; i < lines.Size(); i++)
            ExtractIpsFromLine(lines.Get(i), ips);

        using (var sw = new StreamWriter("output.txt"))
        {
        for (int i = 0; i < ips.Size(); i++)
            sw.WriteLine(ips.Get(i));
        }
    }
}
