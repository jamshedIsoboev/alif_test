using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace alif_test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int operationType;
            string fileName;
          
            Console.WriteLine("Выберите вид операции \n" +
              "1 - (*)\n" +
              "2 - (/)\n" +
              "3 - (-)\n" +
              "4 - (+)\n");

            operationType = int.Parse(Console.In.ReadLine());
            switch (operationType)
            {
                case 1:
                    fileName= file_dialog();
                    rw_file(fileName,"*");
                    break;
                case 2:
                    fileName = file_dialog();
                    rw_file(fileName, "/");
                    break;
                case 3:
                    fileName = file_dialog();
                    rw_file(fileName, "-");
                    break;
                case 4:
                    fileName = file_dialog();
                    rw_file(fileName, "+");
                    break;
                default:
                    Console.WriteLine("Неправильный вид операции!");
                    Console.In.ReadLine();
                    Environment.Exit(0);
                    break;
            }
            Console.WriteLine("Press any key to exit.");
            Console.In.ReadLine();

           
        }

        private static string file_dialog()
        {
            string fileName;
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Выберите txt Файл",
                Filter = "TXT Файл|*.txt;"
            };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = dialog.FileName;
                    return fileName;
                }
            }
            return null;
            
        }

        private static void rw_file(string fileName, string operation)
        {
            string[] lines = File.ReadAllLines(fileName);
            string eval;
            double result;
            string negativeFile = @"D:\negative.txt";
            string positiveFile = @"D:\positive.txt";


            foreach (string line in lines)
            {
                string[] numbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                 eval = numbers[0]+operation+numbers[1];

                 result = ExpressionEvaluator.Eval(eval);

                if (result > 0)
                {
                   
                    if (!File.Exists(positiveFile))
                    {
                        using (var txtFile = File.AppendText(positiveFile))
                        {
                            txtFile.WriteLine(result);
                        }
                    }
                    else if (File.Exists(positiveFile))
                    {
                        using (var txtFile = File.AppendText(positiveFile))
                        {
                            txtFile.WriteLine(result);
                        }
                    }
                }
                else
                {
                    if (!File.Exists(negativeFile))
                    {
                        using (var txtFile = File.AppendText(negativeFile))
                        {
                            txtFile.WriteLine(result);
                        }
                    }
                    else if (File.Exists(negativeFile))
                    {
                        using (var txtFile = File.AppendText(negativeFile))
                        {
                            txtFile.WriteLine(result);
                        }
                    }
                }


            }
        }

       

    }

    public class ExpressionEvaluator
    {
        public static double Eval(string expression)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerResults results =
                codeProvider
                .CompileAssemblyFromSource(new CompilerParameters(), new string[]
                {
                string.Format(@"
                    namespace MyAssembly
                    {{
                        public class Evaluator
                        {{
                            public double Eval()
                            {{
                                return {0};
                            }}
                        }}
                    }}

                ",expression)
                });

            Assembly assembly = results.CompiledAssembly;
            dynamic evaluator =
                Activator.CreateInstance(assembly.GetType("MyAssembly.Evaluator"));
            return evaluator.Eval();
        }
    }
}
