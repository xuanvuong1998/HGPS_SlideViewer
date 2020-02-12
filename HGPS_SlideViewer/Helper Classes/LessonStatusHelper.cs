using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HGPS_SlideViewer
{
    public static class LessonStatusHelper
    {
        public static LessonStatus LessonStatus
        {
            get
            {
                return _lessonStatus;
            }
            set
            {
                _lessonStatus = value;
            }
        }
        private static LessonStatus _lessonStatus;
        static LessonStatusHelper()
        {
            _lessonStatus = new LessonStatus();
        }

        public static void Initialise(string lessonId)
        {
            _lessonStatus.LessonId = lessonId;
        }
    }
}
