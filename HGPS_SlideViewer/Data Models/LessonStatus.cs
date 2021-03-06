﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    public class LessonStatus
    {
        public string LessonId { get; set; }
        public string LessonState { get; set; }
        public string LessonName { get; set; }
        public int? LessonSlide { get; set; }
        public int? AskQuestionNumber { get; set; }
        public string MediaPath { get; set; }
        public string MediaCompleted { get; set; }
        public Quiz CurQuiz { get; set; }
        public string AccessToken { get; set; }
    }
}
