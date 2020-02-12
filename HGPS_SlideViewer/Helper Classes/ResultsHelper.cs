using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    public static class ResultsHelper
    {
        static ResultsHelper() { }

        public static async void GenerateReport(LessonStatus status)
        {
            var quizDataList = await WebHelper.GetResults();
            var studentsResults = ConsolidateResults(quizDataList);

            if (quizDataList != null)
            {
                PdfDocument document = new PdfDocument();

                #region Layout
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont fontHeader = new XFont("Verdana", 24, XFontStyle.Bold);
                XFont font = new XFont("Verdana", 10, XFontStyle.Bold);
                var formatter = new XTextFormatter(gfx);

                XRect rectHeader = new XRect(145, 20, page.Width, 100);
                XRect rectID = new XRect(0, 100, 50, page.Height - 150);
                XRect rectName = new XRect(50, 100, page.Width - 100, page.Height - 150);
                XRect rectMarks = new XRect(page.Width - 50, 100, 50, page.Height - 150);
                XRect rectFooter = new XRect(0, page.Height - 50, page.Width, 50);
                XRect rectLessonName = new XRect(0, 60, page.Width, 25);
                XRect rectLessonDate = new XRect(0, 75, page.Width, 25);

                gfx.DrawRectangle(XBrushes.White, rectHeader);
                gfx.DrawRectangle(XBrushes.White, rectID);
                gfx.DrawRectangle(XBrushes.White, rectName);
                gfx.DrawRectangle(XBrushes.White, rectMarks);
                gfx.DrawRectangle(XBrushes.White, rectFooter);
                gfx.DrawRectangle(XBrushes.White, rectLessonName);
                gfx.DrawRectangle(XBrushes.White, rectLessonDate);
                #endregion


                var mainRect = new XRect(0, 100, page.Width, page.Height - 100);
                var names = "  Name\n";
                var ids = "   No\n";
                var marks = " Marks\n";
                int id = 0;
                foreach (var pair in studentsResults)
                {
                    id += 1;
                    ids += ("   " + id.ToString() + "\n");
                    names += ("  " + pair.Key + "\n");
                    marks += ("    " + pair.Value.ToString() + "\n");
                }

                #region Drawing
                formatter.DrawString("OFFICIAL RESULTS SLIP", fontHeader, XBrushes.Black, rectHeader);
                formatter.DrawString("  Lesson: " + status.LessonName, font, XBrushes.Black, rectLessonName);
                formatter.DrawString("  Date: " + DateTime.Now.ToString("dd/MM/yyyy"), font, XBrushes.Black, rectLessonDate);
                formatter.DrawString(names, font, XBrushes.Black, rectName);
                formatter.DrawString(ids, font, XBrushes.Black, rectID);
                formatter.DrawString(marks, font, XBrushes.Black, rectMarks);

                XPen lineBlack = new XPen(XColors.Black, 1);
                gfx.DrawLine(lineBlack, 0, 100, page.Width, 100); //header line
                gfx.DrawLine(lineBlack, 0, 113, page.Width, 113); //data type line
                gfx.DrawLine(lineBlack, 0, page.Height - 50, page.Width, page.Height - 50); //footer line
                gfx.DrawLine(lineBlack, 50, 100, 50, page.Height - 50); //vertical line 1
                gfx.DrawLine(lineBlack, page.Width - 50, 100, page.Width - 50, page.Height - 50); //vertical line 2
                #endregion

                var fileLocation = FileHelper.BasePath + $@"{status.LessonName}\Results";
                if (Directory.Exists(fileLocation) == false) Directory.CreateDirectory(fileLocation);
                var file = status.LessonId.Replace("/", "-").Replace(":", ".");
                var filename = fileLocation + $@"\{file}.pdf";
                document.Save(filename);
                Process.Start(filename);
            }
        }

        private static Dictionary<string, int> ConsolidateResults(List<QuizData> quizDataList)
        {
            var studentsResults = new Dictionary<string, int>();

            for (int i = 0; i < quizDataList.Count; i++)
            {
                var studentName = quizDataList[i].StudentName;
                var marks = quizDataList[i].MarksAwarded;

                if (studentsResults.ContainsKey(studentName) == false)
                    studentsResults.Add(studentName, marks);
                else
                {
                    studentsResults[studentName] = studentsResults[studentName] + marks;
                }
            }
            studentsResults = studentsResults.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return studentsResults;
        }
    }
}
