using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Api.Infrastructure.Enums;
using TranslationManagement.Api.Infrastructure.Extensions;
using TranslationManagement.Api.Infrastructure.Helpers;
using TranslationManagement.Api.Infrastructure.Models;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TranslatorManagementController> _logger;

        public TranslationJobController(AppDbContext dbContext, ILogger<TranslatorManagementController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public TranslationJob[] GetJobs()
        {
            return _dbContext.TranslationJobs.ToArray();
        }

        [HttpPost]
        public bool CreateJob(TranslationJob job)
        {
            _logger.LogInformation($"Job creation request received with id {job.Id}.");

            job.InitializeJob();
            _dbContext.TranslationJobs.Add(job);

            bool success = _dbContext.SaveChanges() > 0;
            if (success)
            {
                _logger.LogInformation($"Job with id {job.Id} successfully created.");
                var notificationSvc = new UnreliableNotificationService();
                while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
                {
                }

                _logger.LogInformation("New job notification sent");
            }

            return success;
        }

        [HttpPost]
        public bool CreateJobWithFile(IFormFile file, string customer)
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
        public string UpdateJobStatus(int jobId, string newStatus = "")
        {
            _logger.LogInformation($"Job status update request received: {newStatus} for job {jobId}");

            if (string.IsNullOrEmpty(newStatus) || !ControllerHelper.IsJobStatusValid(newStatus))
            {
                throw new ArgumentException("invalid status");
            }

            var job = _dbContext.TranslationJobs.Single(job => job.Id == jobId);

            if (job == null)
                throw new KeyNotFoundException($"Job with id {jobId} not existing");

            bool isStatusChangeValid = (job.Status == JobStatus.New.ToString() && 
                                        newStatus == JobStatus.Completed.ToString()) ||
                                        job.Status == JobStatus.Completed.ToString() || 
                                        newStatus == JobStatus.New.ToString();

            if (isStatusChangeValid)
            {
                throw new InvalidOperationException("invalid status change");
            }

            job.Status = newStatus;

            bool success = _dbContext.SaveChanges() > 0;

            return success ? ProcessFlow.Updated.GetEnumDescription() : ProcessFlow.UpdatingFailed.GetEnumDescription();
        }
    }
}