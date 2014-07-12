using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace LoadCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string csv_file_path = @"C:\Users\Zhiyu\Desktop\1.csv";
            DataTable csvData = GetDataTabletFromCSVFile(csv_file_path);
            //Console.WriteLine(csvData.Rows[1]["User"].ToString);

            List<Person> list = GetPersonList(csvData);
            Console.WriteLine(list.Count);
            Person p = list[1];
            string json = JsonConvert.SerializeObject(p);
            Console.WriteLine(json);
            Console.ReadKey();
        }

        public static List<Person> GetPersonList(DataTable dt)
        {
            List<Person> list = new List<Person>();
            // materialize row data to person objects
            foreach (DataRow row in dt.Rows)
            {
                Person p = new Person();
                p.User = row[1].ToString();
                p.Project = row[3].ToString();
                p.Hours = Convert.ToDouble(row[4]);
                p.BillingStatus = row[5].ToString();
                list.Add(p);
            }

            return list;
        }

         private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
              using(TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                 {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
        }
    }
}
