using System;
using System.IO;
using System.Linq;

namespace SudokuAutoTest
{
    class Program
    {
        public static string ProjectDir = Environment.CurrentDirectory;

        //Gen files
        public static string ResultFile = @"./Scores.txt";
        public static string WrongFile = @"./Wrong.txt";

        //Max Limit
        public static int MaxLimitTime = 600;

        public static string Number;

        public static void Main(string[] args)
        {
            try
            {
                for (int i = 1; i < args.Length; i += 2)
                {
                    switch (args[i - 1])
                    {
                        case "-limit":
                            MaxLimitTime = int.Parse(args[i]);
                            break;
                        case "-number":
                            Number = args[i];
                            break;
                    }
                }
                TestProject();
            }
            catch (Exception e)
            {
                Hint();
                Console.Read();
            }
        }

        public static void Hint()
        {
            Console.WriteLine("Usages: \n" +
                              "\t-limit [max limit second] -number [number id]\n\n" +
                              "\t- 本功能用于给学生的作业进行评分,并记录每份作业在不同测试数据下耗费的时间。最终生成的评分文件为 Scores.txt, 可直接复制到Excel中使用。\n\n" +
                              "\t- 数字 [limit second] 指定效率测试运行的最大时长, 默认为 600秒。\n\n" +
                              "\t- 学号 [number id] (可选参数)提供单个学号, 当本参数存在时，将只测试单个同学的工程，并将结果存储至 学号-score.txt中。\n\n" +
                              "使用时将本程序复制到学生代码仓库中使用");
        }


        //单次测试某位同学的成绩,生成文件到 学号-score.txt 中
        public static void TestProject()
        {
            var writePath = $"./{Number}-Score.txt";
            SudokuTester tester = new SudokuTester(ProjectDir, Number);
            using (var writer = new StreamWriter(writePath, false))
            {
                try
                {
                    tester.GetCorrectScore();
                    var arguments = tester.Scores.Select(i => i.Item1).ToList();
                    writer.Write("NumberID\t");
                    writer.WriteLine(string.Join("\t", arguments));
                    writer.Write(tester.NumberId + "\t");
                    writer.WriteLine(string.Join("\t", tester.Scores.Select(i => i.Item2)));
                    writer.Flush();
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, tester._logFile);
                }
            }
            using (var writer = new StreamWriter(WrongFile, false))
            {
                try
                {
                    tester.GetWrongScore();
                    var arguments = tester.Wrongs.Select(i => i.Item1).ToList();
                    writer.WriteLine();
                    writer.Write("NumberID\t");
                    writer.WriteLine(string.Join("\t", arguments));
                    writer.Write(tester.NumberId + "\t");
                    writer.WriteLine(string.Join("\t", tester.Wrongs.Select(i => i.Item2)));
                    writer.Flush();
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, tester._logFile);
                }

            }
        }
    }
}
