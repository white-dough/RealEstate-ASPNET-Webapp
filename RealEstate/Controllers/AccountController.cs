﻿using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
