using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using ProjectJson;


namespace LoadCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string csv_file_path = @"C:\Users\Zhiyu\Desktop\Project.csv";
            DataTable csvData = GetDataTabletFromCSVFile(csv_file_path);
            //Console.WriteLine(csvData.Rows[1]["User"].ToString);

            List<Project> list = GetProjectList(csvData);
            Console.WriteLine(list.Count);
            Papa papa = new Papa();
            papa.Name = "project";
            papa.children = list;

      

            foreach (Project p in list)
            {
                Console.WriteLine(p.Name+"--------"+p.children.Count);
            }
            string result = JsonConvert.SerializeObject(papa);
            Console.WriteLine(result);

            System.IO.StreamWriter swOut = new System.IO.StreamWriter(@"C:\Users\Zhiyu\Desktop\2.txt", false, System.Text.Encoding.Default);
            swOut.WriteLine(result);
            swOut.Flush();
            swOut.Close();

            Console.ReadKey();
        }

        public static List<Project> GetProjectList(DataTable dt)
        {
            List<Project> list = new List<Project>();
            Project temp = new Project(); 
            temp.Name="";
            // materialize row data to person objects
            foreach (DataRow row in dt.Rows)
            {
                if(temp.Name.Equals("")){
                    Project p = new Project();
                    p.Name = row[0].ToString();
                    User u = new User();
                    u.Name = row[1].ToString();
                    u.Hours = Convert.ToDouble(row[2]);
                    p.children.Add(u);
                    temp = p;
                    continue;
                }
                if (!temp.Name.Equals(row[0].ToString()))
                {
                    list.Add(temp);
                    Project p = new Project();
                    p.Name = row[0].ToString();
                    User u = new User();
                    u.Name = row[1].ToString();
                    u.Hours = Convert.ToDouble(row[2]);
                    p.children.Add(u);
                    temp = p;
                }
                else {
                    User u = new User();
                    u.Name = row[1].ToString();
                    u.Hours = Convert.ToDouble(row[2]);
                    temp.children.Add(u);
                }               
                
            }
            list.Add(temp);

            return list;
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
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
