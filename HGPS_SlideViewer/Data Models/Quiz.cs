using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    public class Quiz
    {
        public string QuizFormat { get; set; }
        public int QuestionNumber { get; set; }
        public string Answer { get; set; }
        public string TimeOut { get; set; }

        public Quiz() { }
    }
}
