//using System;
//using System.Diagnostics;
//using System.Windows.Forms;


//public partial class MainForm : Form
//{
//    public MainForm()
//    {
//        InitializeComponent();
//        comboAlgorithms.SelectedIndex = 0;
//    }

//    private void btnRun_Click(object sender, EventArgs e)
//    {
//        int size = (int)numericSize.Value;
//        int[] array = GenerateRandomArray(size);
//        int[] copy = (int[])array.Clone();

//        Stopwatch sw = Stopwatch.StartNew();

//        switch (comboAlgorithms.SelectedItem.ToString())
//        {
//            case "Bubble Sort":
//                BubbleSort(copy);
//                break;

//            case "Insertion Sort":
//                InsertionSort(copy);
//                break;

//            case "Quick Sort":
//                QuickSort(copy, 0, copy.Length - 1);
//                break;
//        }

//        sw.Stop();
//        listResults.Items.Add($"{comboAlgorithms.SelectedItem} | {size} элементов | {sw.ElapsedMilliseconds} ms");
//    }

//    // -------- Генерация массива ----------
//    private int[] GenerateRandomArray(int size)
//    {
//        Random rnd = new Random();
//        int[] arr = new int[size];
//        for (int i = 0; i < size; i++)
//            arr[i] = rnd.Next(0, 100000);
//        return arr;
//    }


//    // ------------ СОРТИРОВКИ ----------------

//}