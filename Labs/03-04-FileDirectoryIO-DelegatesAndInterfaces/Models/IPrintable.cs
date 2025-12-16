using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPrintable
{
    string ToFormattedString(string str);
    void Print(string str);
}