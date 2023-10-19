using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Data.Models;
using TranslationManagement.Api.Data.Repository;
using TranslationManagement.Api.Infrastructure.Enums;
using TranslationManagement.Api.Infrastructure.Extensions;
using TranslationManagement.Api.Infrastructure.Helpers;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly ITranslationJobRepository _translationJobRepository;
        private readonly ILogger<TranslationJobController> _logger;

        public TranslationJobController(ITranslationJobRepository translationJobRepository, ILogger<TranslationJobController> logger)
        {
            _translationJobRepository = translationJobRepository;
            _logger = logger;
        }

        [HttpGet]
        public TranslationJob[] GetJobs()
        {
            return _translationJobRepository.GetJobs();
        }

        [HttpPost]
        public IActionResult CreateJob(TranslationJob job)
        {
            _logger.LogInformation($"Job creation request received with id {job.Id}.");

            job.InitializeJob();
            bool isAddingSuccessful = _translationJobRepository.AddJob(job);

            if (isAddingSuccessful)
            {
                _logger.LogInformation($"Job with id {job.Id} successfully created.");
                var notificationSvc = new UnreliableNotificationService();
                while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
                {
                }

                _logger.LogInformation("New job notification sent");


                return Ok(isAddingSuccessful);
            }
            else
            {
                return BadRequest("Job creation not successful");
            }
        }

        [HttpPost]
        public IActionResult CreateJobWithFile(IFormFile file, string customer)
        {
            _logger.LogInformation($"Job creation request from file received.");
            string content;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                if (file.FileName.EndsWith(".txt"))
                {
                    content = reader.ReadToEnd();
                }
                else if (file.FileName.EndsWith(".xml"))
                {
                    var xdoc = XDocument.Parse(reader.ReadToEnd());
                    content = xdoc.Root.Element("Content").Value;
                    customer = xdoc.Root.Element("Customer").Value.Trim();
                }
                else
                {
                    throw new NotSupportedException("unsupported file");
                }
            }

            var newJob = new TranslationJob()
            {
                OriginalContent = content,
                TranslatedContent = string.Empty,
                CustomerName = customer,
            };

            ControllerHelper.SetJobPrice(newJob);

            return CreateJob(newJob);
        }

        [HttpPost]
        public IActionResult UpdateJobStatus(int jobId, int translatorId, string newStatus = "")
        {
            _logger.LogInformation($"Job status update request received: {newStatus} for job {jobId}");

            if (string.IsNullOrEmpty(newStatus) || !ControllerHelper.IsJobStatusValid(newStatus))
            {
                throw new ArgumentException("invalid status");
            }

            var job = _translationJobRepository.GetJobById(jobId);

            if (job == null)
                return BadRequest($"Job with id {jobId} not existing");

            bool isStatusChangeValid = (job.Status == JobStatus.New.ToString() && 
                                        newStatus == JobStatus.Completed.ToString()) ||
                                        job.Status == JobStatus.Completed.ToString() || 
                                        newStatus == JobStatus.New.ToString();

            if (isStatusChangeValid)
            {
                return BadRequest("invalid status change");
            }

            bool isJobStatusUpdateSuccessful = _translationJobRepository.UpdateJobStatus(job, newStatus);

            if(isJobStatusUpdateSuccessful)
            {
                return Ok(ProcessFlow.Updated.GetEnumDescription());
            }
            else
            {
                return BadRequest(ProcessFlow.UpdatingFailed.GetEnumDescription());
            }
        }
    }
}