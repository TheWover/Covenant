﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

using Covenant.Core;
using Covenant.Models;
using Covenant.Models.Listeners;

namespace Covenant.Controllers.ViewControllers
{
    [Authorize]
    public class HostedFileController : Controller
    {
        private readonly CovenantContext _context;

        public HostedFileController(CovenantContext context)
        {
            _context = context;
        }

        // GET: /hostedfile/create
        public IActionResult Create(int id)
        {
            try
            {
                return View(new HostedFile { ListenerId = id });
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                return RedirectToAction("Index", "Listener");
            }
        }

        // POST: /hostedfile/create
        [HttpPost]
        public async Task<IActionResult> Create(HostedFile file)
        {
            try
            {
                HostedFile createdFile = await _context.CreateHostedFile(file.ListenerId, file);
                return RedirectToAction("Interact", "Listener", new { Id = createdFile.Id });
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(new HostedFile { ListenerId = file.ListenerId });
            }
        }

        // GET: /hostedfile/download
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                HostedFile file = await _context.GetHostedFile(id);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(file.Path, out string contentType))
                {
                    contentType = "application/octet-stream";
                }
                return new FileContentResult(Convert.FromBase64String(file.Content), contentType);
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                return RedirectToAction("Index", "Listener");
            }
        }
    }
}
