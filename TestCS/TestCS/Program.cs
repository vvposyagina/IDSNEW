namespace DataContractSerializerExample
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;

    // You must apply a DataContractAttribute or SerializableAttribute
    // to a class to have it serialized by the DataContractSerializer.


    public sealed class Test
    {
        private Test() { }

        public static void Main()
        {
            string path = @"E:\Диплом\Прога\AnalyzerTest\logOutput.txt";
            int n = 860;
            using (StreamWriter writer = File.AppendText(path))
            {
                    int i = 0;
                    while (i < n)
                    {
                        writer.WriteLine("1");
                        i++;
                    }
                
            }
        }
       
    }
}