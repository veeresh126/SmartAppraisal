using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_SmartAppraisal.Entities
{
    public class CaseStudyCompetency
    {
        public int CaseStudyCompetencyId { get; set; }

        public int CaseStudySolutionId { get; set; }

        public string CompetencyName { get; set; } = string.Empty;

        public CaseStudySolution CaseStudySolution { get; set; } = null!;

    }
}
