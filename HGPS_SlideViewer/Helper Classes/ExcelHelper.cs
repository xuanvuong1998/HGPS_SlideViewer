using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using CsvHelper;

namespace HGPS_SlideViewer
{
    public static class ExcelHelper
    {
        static ExcelHelper() { }

        public static void Create(string filename, List<QuizData> quizDataList)
        {
            using (var writer = new StreamWriter(filename))
            {
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(quizDataList);
                    csv.NextRecord();
                    csv.WriteHeader<QuizRanking>();
                    //csv.NextRecord();
                    //csv.WriteRecords(ResultsHelper.ConsolidateResults(quizDataList));
                }
            }
        }
    }
}
