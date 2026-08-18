using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_SmartAppraisal.Entities
{
    public class CaseStudySolution
    {
        public int CaseStudySolutionId { get; set; }

        public int CaseStudyId { get; set; }

        public int SolutionNumber { get; set; }

        public string SolutionText { get; set; } = string.Empty;

        public CaseStudy CaseStudy { get; set; } = null!;

        public ICollection<CaseStudyCompetency> Competencies { get; set; }
            = new List<CaseStudyCompetency>();
    }
}
