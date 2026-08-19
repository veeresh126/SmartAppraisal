using BL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Entities;
using DL_SmartAppraisal.Interfaces;

namespace BL_SmartAppraisal.Services
{
    public class CaseStudyService : ICaseStudyService
    {
        private readonly ICaseStudyRepository _caseStudyRepository;

        public CaseStudyService(
            ICaseStudyRepository caseStudyRepository)
        {
            _caseStudyRepository = caseStudyRepository;
        }


        // =========================================================
        // GET ALL CASE STUDIES
        // =========================================================

        public async Task<List<CaseStudy>> GetAllAsync()
        {
            return await _caseStudyRepository.GetAllAsync();
        }


        // =========================================================
        // GET CASE STUDY BY ID
        // =========================================================

        public async Task<CaseStudy?> GetByIdAsync(int id)
        {
            return await _caseStudyRepository.GetByIdAsync(id);
        }


        // =========================================================
        // GET CASE STUDIES BY USER
        // =========================================================

        public async Task<List<CaseStudy>> GetByUserIdAsync(
            int userId)
        {
            return await _caseStudyRepository
                .GetByUserIdAsync(userId);
        }


        // =========================================================
        // CREATE CASE STUDY
        // =========================================================

        public async Task<CaseStudy> CreateAsync(
            CaseStudy caseStudy)
        {
            // Every new case study starts as Pending
            caseStudy.Status =
                CaseStudyStatus.Pending;

            // Set creation/modification dates
            caseStudy.CreatedDate =
                DateTime.UtcNow;

            caseStudy.ModifiedDate =
                DateTime.UtcNow;

            // CreatedBy is already set by
            // CaseStudyController from session

            return await _caseStudyRepository
                .CreateAsync(caseStudy);
        }


        // =========================================================
        // REVIEW CASE STUDY
        // =========================================================

        public async Task<bool> ReviewAsync(
            int caseStudyId,
            int reviewerId,
            bool approved,
            string? reviewComment)
        {
            return await _caseStudyRepository
                .ReviewAsync(
                    caseStudyId,
                    reviewerId,
                    approved,
                    reviewComment);
        }
    }
}