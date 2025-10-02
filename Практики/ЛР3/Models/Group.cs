using System;
using System.Collections.Generic;

class Group
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();

    public void Print()
    {

    }
}
