using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using UserJson;

namespace LoadCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string csv_file_path = @"C:\Users\Zhiyu\Desktop\User.csv";
            DataTable csvData = GetDataTable(csv_file_path);

            //package list into an object
            List<User> list = GetPersonList(csvData);
            Console.WriteLine(list.Count);
            var pa = new { Name = "User", children = list };
            
            //use json.net to generate json file
            string result = JsonConvert.SerializeObject(pa);
            Console.WriteLine(result);

            //write file
            System.IO.StreamWriter swOut = new System.IO.StreamWriter(@"C:\Users\Zhiyu\Desktop\1.txt", false, System.Text.Encoding.Default);
            swOut.WriteLine(result);
            swOut.Flush();
            swOut.Close();

            Console.ReadKey();
        }

        public static List<User> GetPersonList(DataTable dt)
        {
            List<User> list = new List<User>();

            // materialize row data to person objects
            foreach (DataRow row in dt.Rows)
            {
                User p = new User();
                p.Name = row[0].ToString();
                p.Hours = Convert.ToDouble(row[1]);        
                list.Add(p);
            }

            return list;
        }

         private static DataTable GetDataTable(string csv_file_path)
        {
            DataTable dt = new DataTable();
            try
            {
              using(TextFieldParser Reader = new TextFieldParser(csv_file_path))
                 {
                    Reader.SetDelimiters(new string[] { "," });
                    Reader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] cols = Reader.ReadFields();
                    foreach (string column in cols)
                    {
                        DataColumn datecol = new DataColumn(column);
                        datecol.AllowDBNull = true;
                        dt.Columns.Add(datecol);
                    }
                    while (!Reader.EndOfData)
                    {
                        string[] fieldData = Reader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        dt.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return dt;
        }
    }
}
