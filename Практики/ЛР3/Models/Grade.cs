using System;
using System.Collections.Generic;
using System.Linq;

class Grade // Класс, представляющий оценки студента по одному предмету
{
    public Subject Subject { get; set; }
    public List<int> Scores { get; set; } = new List<int>();

    // Средний балл
    public double Average => Scores.Count > 0 ? Scores.Average() : 0;

    public Grade(Subject subject)
    {
        Subject = subject;
    }

    public void AddScore(int score)
    {
        if (score < 1 || score > 5)
            throw new Exception("Оценка должна быть в диапазоне 1–5");
        Scores.Add(score);
    }

    public void RemoveScore(int score)
    {
        if (!Scores.Remove(score))
            throw new Exception("Такой оценки нет в списке");
    }

    public string ToFormattedString(string indent = "")
    {
        string scores = Scores.Count > 0 ? $"[{string.Join(", ", Scores)}]" : "нет оценок";
        return $"{indent}{Subject.Title}: {scores} → ср. {Average:F2}";
    }

    public override string ToString() => ToFormattedString();
    public void Print(string indent = "") => Console.WriteLine(ToFormattedString(indent));
}
