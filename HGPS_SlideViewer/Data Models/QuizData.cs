using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    public class QuizData
    {
        public int Id { get; set; }
        public string QuizId { get; set; }
        public string LessonName { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int QuestionNumber { get; set; }
        public int MarksAwarded { get; set; }
    }

    public class QuizRanking
    {
        public string Name { get; set; }
        public string Marks { get; set; }
    }
}
