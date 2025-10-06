using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Grade
{
    public Subject Subject { get; set; }
    public List<int> Scores { get; set; } = new List<int>();

    public double Average => Scores.Count > 0 ? Scores.Average() : 0;
}
