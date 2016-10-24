using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VDS.RDF;
using VDS.RDF.Query;

namespace InconsistencyRemoval {
    public class Manager {
        private IList<Person> QueryList;
        private string File;
        ExcelWorksheet ws;
        private int counter = 2;
        Label status;
        ExcelPackage pck;
        MainWindow mw;
        int names;
        int current = 0;
        public Manager(string File, MainWindow mw, int names) {
            this.QueryList = new List<Person>();
            this.File = File;
            this.mw = mw;
            this.names = names;
        }

        public void Run() {
            //Load the people to be queried
            string[] lines = System.IO.File.ReadAllLines(@"PersonOccupation.txt");

            //Create the excel file
            FileInfo newFile = new FileInfo(File + ".xlsx");
            pck = new ExcelPackage(newFile);
            ws = pck.Workbook.Worksheets.Add("Results");
            ws.Cells["A1"].Value = "Name";
            ws.Cells["B1"].Value = "Occupation";
            ws.Cells["C1"].Value = "Number of results";
            ws.Cells["D1"].Value = "Result";
            ws.Cells["E1"].Value = "Queried name";
            ws.Cells["F1"].Value = "Queried abstract";
            ws.Cells["G1"].Value = "Number of abstracts";
            ws.Cells["H1"].Value = "Number of externals";

            foreach (string line in lines) { 
                current++;
                if (current < 155)
                    continue;
                if (counter > 223)
                    break;
                string name = line.Replace("\n", "").Trim().Split(':')[0];
                string occupation = line.Replace("\n", "").Trim().Split(':')[1];
                if (name.Split(' ').Length != names)
                    continue;
                if (name.Contains("Zola"))
                    continue;
                Person pers = new Person(name, occupation);
                Query(pers);
                Console.WriteLine("{0} - {1} - {2}", counter, name, occupation);
            }

            pck.Save();
            pck.Dispose();
        }
        public void Stop() {
            pck.Save();
            pck.Dispose();
        }
        public void Query(Person person) {
            IDictionary<INode,SparqlResult> dictionary;
            int resultsNumber;
            SparqlResult queried;
            string URI;
            string name;
            string abs;
            string numAbs;
            string numExt;
                person.Load();
                dictionary = person.Query1();
                resultsNumber = dictionary.Count;
                queried = person.RunQuery2();
                if (queried != null) {
                    URI = queried.Value("person").ToString();
                    name = queried.Value("name").ToString();
                    abs = queried.Value("abstract").ToString();
                    numAbs = queried.Value("abstractCount").ToString();
                    numExt = queried.Value("externalCount").ToString();
                }

                else {
                    URI = "Result not found";
                    name = "";
                    abs = "";
                    numAbs = "0";
                    numExt = "0";
                }

                ws.Cells["A" + counter].Value = person.Name;
                ws.Cells["B" + counter].Value = person.Occupation;
                ws.Cells["C" + counter].Value = resultsNumber;
                ws.Cells["D" + counter].Value = URI;
                ws.Cells["E" + counter].Value = name.Replace("@en","");
                ws.Cells["F" + counter].Value = abs.Replace("@en", "");
                ws.Cells["G" + counter].Value = numAbs.Split('^')[0];
                ws.Cells["H" + counter].Value = numExt.Split('^')[0];
                counter++;
            
         }
    }
}
