using System;
using System.Collections.Generic;

namespace DL_SmartAppraisal.Entities
{
    public class CaseStudy
    {
        public int CaseStudyId { get; set; }

        public string Designation { get; set; } = string.Empty;

        public string CaseStudyText { get; set; } = string.Empty;

        //public bool Status { get; set; }
        public CaseStudyStatus Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<CaseStudySolution> Solutions { get; set; }
            = new List<CaseStudySolution>();

        public UserDetail? CreatedByUser { get; set; }

        public string? ReviewComment { get; set; }

        public int? ReviewedBy { get; set; }

        public DateTime? ReviewedDate { get; set; }
    }
}