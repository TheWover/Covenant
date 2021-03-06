﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using Covenant.Core;
using Covenant.Models;
using Covenant.Models.Covenant;
using Covenant.Models.Listeners;

namespace Covenant.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly CovenantContext _context;
        private readonly UserManager<CovenantUser> _userManager;

        public ProfileController(CovenantContext context, UserManager<CovenantUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /profile/
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.GetProfiles());
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                return RedirectToAction("Index", "Listener");
            }
        }

        // GET: /profile/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return View(await _context.GetHttpProfile(id));
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                return RedirectToAction("Index", "Listener");
            }
        }

        // POST: /profile/
        [HttpPost]
        public async Task<IActionResult> Edit(HttpProfile profile)
        {
            try
            {
                await _context.EditHttpProfile(profile, await _userManager.GetUserAsync(HttpContext.User));
                return RedirectToAction("Index", "Listener");
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                return RedirectToAction("Index", "Listener");
            }
        }

        // GET: /profile/create
        public IActionResult Create()
        {
            try
            {
                return View(new HttpProfile());
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                return RedirectToAction("Index", "Listener");
            }
        }

        // POST: /profile/
        [HttpPost]
        public async Task<IActionResult> Create(HttpProfile profile)
        {
            try
            {
                await _context.CreateHttpProfile(profile, await _userManager.GetUserAsync(HttpContext.User));
                return RedirectToAction("Index", "Listener");
            }
            catch (Exception e) when (e is ControllerNotFoundException || e is ControllerBadRequestException || e is ControllerUnauthorizedException)
            {
                return RedirectToAction("Index", "Listener");
            }
        }
    }
}
