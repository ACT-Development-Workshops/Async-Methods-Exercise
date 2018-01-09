﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TutsUniversity.ViewModels
{
    public class DailyEnrollmentTotals
    {
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }

        public int StudentCount { get; set; }
    }
}