﻿using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Dtos;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientRegistryController : BaseTransactionController
    {
        private readonly IPatientRegistryService _patientRegistryService;
        protected override int ModuleId => (int)ModuleEnum.PatientRegistry;

        public PatientRegistryController(ITransactionFlowHistoryService _transactionFlowHistoryService, IPatientRegistryService patientRegistryService) : base(_transactionFlowHistoryService)
        {
            _patientRegistryService = patientRegistryService;
        }

        // Save or update a Patient Registry
        [HttpPost("SavePatientRegistry")]
        public async Task<IActionResult> SavePatientRegistry(PatientRegistryDto patientRegistry, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Call the updated service method which now returns the saved entity.
                //    Assuming 'isWithValidation' should be true when called from the controller.
                var savedRegistry = await _patientRegistryService.SavePatientRegistryAsync(patientRegistry, cancellationToken);

                // 2. Return a success response including the ID from the object we got back.
                return Ok(new
                {
                    success = true,
                    message = patientRegistry.PatientRegistryId == Guid.Empty
                        ? "Patient Registry created successfully."
                        : "Patient Registry updated successfully.",
                    Id = savedRegistry.PatientRegistryId // <-- The returned ID
                });
            }
            catch (ModelValidationException ex)
            {
                foreach (var kvp in ex.Errors)
                {
                    foreach (var error in kvp.Value)
                    {
                        ModelState.AddModelError(kvp.Key, error);
                    }
                }

                var modelErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(new { success = false, errors = modelErrors });
            }
        }

        // Create a new Patient Registry
        [HttpPost("CreatePatientRegistry")]
        public async Task<IActionResult> CreatePatientRegistry(
            AcceptReferralDto acceptReferralDto, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Call the service method to create a new PatientRegistry
                var newPatientRegistry = await _patientRegistryService.CreatePatientRegistryAsync(
                    acceptReferralDto,  cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = "Patient Registry created successfully.",
                    data = newPatientRegistry
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a bad request response
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // Get a specific Patient Registry by ID
        [HttpGet("GetPatientRegistry/{id}")]
        public async Task<IActionResult> GetPatientRegistry(Guid id, CancellationToken cancellationToken)
        {
            var patientRegistry = await _patientRegistryService.GetAsync(id, cancellationToken);
            if (patientRegistry == null)
            {
                return NotFound(new { success = false, message = "Patient Registry not found." });
            }

            return Ok(patientRegistry);
        }

        // Get all Patient Registry with paging
        [HttpGet("GetPagedPatientRegistries")]
        public async Task<IActionResult> GetPagedPatientRegistries(CancellationToken cancellationToken, int pageNo, int limit, byte? statusId = null, string? keyword = null)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var patientRegistries = await _patientRegistryService.GetAllWithPagingAsync(pageNo, limit, statusId, keyword, cancellationToken);
            var totalRecords = await _patientRegistryService.CountAsync(statusId, keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<PatientRegistry>(patientRegistries, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }
    }
}
