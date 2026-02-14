using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CrimePortal.Data;
using CrimePortal.Models;
using CrimePortal.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CrimePortal.Helpers;

namespace CrimePortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var data = (from u in _context.Users
                        join o in _context.Offices
                        on u.UserId equals o.UserId into officeJoin
                        from office in officeJoin.DefaultIfEmpty()
                        select new UserOfficerListVM
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            IsActive = u.IsActive, // ✅ Add this
                            UserType = u.UserType,
                            Post = office.Post,
                            OfficeName = office.OfficeName,
                            OfficeAddress = office.OfficeAddress,
                            District = office.District,
                            Division = office.Division
                        }).ToList();

            return View(data);
        }

        // ================= CREATE (GET) =================
        [HttpGet]
        public IActionResult Create()
        {
            var model = new AdminUserCreateViewModel();
            model.Offices.Add(new OfficeViewModel());
            return View(model);
        }


        // ================= CREATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdminUserCreateViewModel model)
        {
            // 🔥 REMOVE OFFICERS VALIDATION
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith("Officers")).ToList())
            {
                ModelState.Remove(key);
            }

            // 🔥 REMOVE JAWANS VALIDATION
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith("Jawans")).ToList())
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            // 1️⃣ Save User (PASSWORD HASHED)
            var user = new User
            {
                UserName = model.UserName,
                PasswordHash = PasswordHelper.Hash(model.Password), // ✅ HASH
                UserType = model.UserType,
                IsActive = true                                    // ✅ Account Active
            };

            _context.Users.Add(user);
            _context.SaveChanges();


            // 2️⃣ Save Office
            var office = new OfficeInfo
            {
                UserId = user.UserId,
                Post = model.Post,
                OfficeName = model.OfficeName,
                OfficeAddress = model.OfficeAddress,
                District = model.District,
                Division = model.Division
            };

            _context.Offices.Add(office);
            _context.SaveChanges();   // ✅ OFFICE SAVE


            // 3️⃣ Save Officers
            foreach (var o in model.Officers.Where(x => !string.IsNullOrWhiteSpace(x.OfficerName)))
            {
                _context.Officers.Add(new Officer
                {
                    UserId = user.UserId,
                    OfficerName = o.OfficerName,
                    OfficerRank = o.OfficerRank,
                    OfficeName = o.OfficeName,
                    OfficeAddress = o.OfficeAddress,
                    District = o.District,
                    Division = o.Division
                });
            }

            // 4️⃣ Save Jawans
            foreach (var j in model.Jawans.Where(x => !string.IsNullOrWhiteSpace(x.JawanName)))
            {
                _context.Jawans.Add(new Jawan
                {
                    UserId = user.UserId,
                    JawanName = j.JawanName,
                    OfficeName = j.OfficeName,
                    OfficeAddress = j.OfficeAddress,
                    District = j.District,
                    Division = j.Division
                });
            }

            _context.SaveChanges();   // ✅ SAVE ALL

            TempData["Success"] = "डेटा यशस्वीरित्या सेव झाला";
            return RedirectToAction("Index");
        }




        // ================= EDIT (GET) =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound();

            var office = _context.Offices.FirstOrDefault(o => o.UserId == id);

            var model = new AdminUserCreateViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserType = user.UserType,

                Post = office?.Post,
                OfficeName = office?.OfficeName,
                OfficeAddress = office?.OfficeAddress,
                District = office?.District,
                Division = office?.Division,

                Officers = _context.Officers
                    .Where(x => x.UserId == id)
                    .Select(x => new OfficerViewModel
                    {
                        OfficerId = x.OfficerId,
                        UserId = x.UserId,
                        OfficerName = x.OfficerName,
                        OfficerRank = x.OfficerRank,
                        OfficeName = x.OfficeName,
                        OfficeAddress = x.OfficeAddress,
                        District = x.District,
                        Division = x.Division
                    }).ToList(),

                Jawans = _context.Jawans
                    .Where(x => x.UserId == id)
                    .Select(x => new JawanViewModel
                    {
                        JawanId = x.JawanId,
                        UserId = x.UserId,
                        JawanName = x.JawanName,
                        OfficeName = x.OfficeName,
                        OfficeAddress = x.OfficeAddress,
                        District = x.District,
                        Division = x.Division
                    }).ToList()
            };

            // 🔥 IMPORTANT: at least 1 row
            if (!model.Officers.Any())
                model.Officers.Add(new OfficerViewModel());

            if (!model.Jawans.Any())
                model.Jawans.Add(new JawanViewModel());

            return View("Create", model);
        }







        // ================= EDIT (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdminUserCreateViewModel model)
        {
            // 🔥 REMOVE PASSWORD
            ModelState.Remove("Password");

            // 🔥 REMOVE OFFICERS VALIDATION
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith("Officers")).ToList())
            {
                ModelState.Remove(key);
            }

            // 🔥 REMOVE JAWANS VALIDATION
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith("Jawans")).ToList())
            {
                ModelState.Remove(key);
            }


            if (!ModelState.IsValid)
            {
                // DEBUG: temp check
                // return BadRequest(ModelState);
                return View("Create", model);
            }

            
               

            // 🔵 USER UPDATE
            var user = _context.Users.FirstOrDefault(x => x.UserId == model.UserId);
            if (user != null)
            {
                user.UserName = model.UserName;
                user.UserType = model.UserType;
            }

            // 🔵 OFFICE UPDATE
            var office = _context.Offices.FirstOrDefault(x => x.UserId == model.UserId);
            if (office != null)
            {
                office.Post = model.Post;
                office.OfficeName = model.OfficeName;
                office.OfficeAddress = model.OfficeAddress;
                office.District = model.District;
                office.Division = model.Division;
            }
            else
            {
                office = new OfficeInfo
                {
                    UserId = user.UserId,
                    Post = model.Post,
                    OfficeName = model.OfficeName,
                    OfficeAddress = model.OfficeAddress,
                    District = model.District,
                    Division = model.Division
                };

                _context.Offices.Add(office);
            }


            // 🔵 OFFICERS UPDATE

            foreach (var o in model.Officers.Where(x => !string.IsNullOrWhiteSpace(x.OfficerName)))
            {
                if (o.OfficerId == 0)
                {
                    _context.Officers.Add(new Officer
                    {
                        UserId = model.UserId,
                        OfficerName = o.OfficerName,
                        OfficerRank = o.OfficerRank,
                        OfficeName = o.OfficeName,
                        OfficeAddress = o.OfficeAddress,
                        District = o.District,
                        Division = o.Division
                    });
                }
                else
                {
                    var officer = _context.Officers.First(x => x.OfficerId == o.OfficerId);
                    officer.OfficerName = o.OfficerName;
                    officer.OfficerRank = o.OfficerRank;
                    officer.OfficeName = o.OfficeName;
                    officer.OfficeAddress = o.OfficeAddress;
                    officer.District = o.District;
                    officer.Division = o.Division;
                }
            }



            // 🔵 JAWANS UPDATE

            foreach (var j in model.Jawans.Where(x => !string.IsNullOrWhiteSpace(x.JawanName)))
            {
                if (j.JawanId == 0)
                {
                    _context.Jawans.Add(new Jawan
                    {
                        UserId = model.UserId,
                        JawanName = j.JawanName,
                        OfficeName = j.OfficeName,
                        OfficeAddress = j.OfficeAddress,
                        District = j.District,
                        Division = j.Division
                    });
                }
                else
                {
                    var jawan = _context.Jawans.First(x => x.JawanId == j.JawanId);
                    jawan.JawanName = j.JawanName;
                    jawan.OfficeName = j.OfficeName;
                    jawan.OfficeAddress = j.OfficeAddress;
                    jawan.District = j.District;
                    jawan.Division = j.Division;
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "डेटा यशस्वीरित्या अपडेट झाला";
            return RedirectToAction("Index");
        }






        // ================= RESET PASSWORD =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(int userId, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) return NotFound();

            user.PasswordHash = PasswordHelper.Hash(newPassword);
            _context.SaveChanges();

            TempData["Success"] = "पासवर्ड reset झाला";
            return RedirectToAction("Index");
        }



        // ================= DELETE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _context.Offices.RemoveRange(_context.Offices.Where(x => x.UserId == id));
            _context.Officers.RemoveRange(_context.Officers.Where(x => x.UserId == id));
            _context.Jawans.RemoveRange(_context.Jawans.Where(x => x.UserId == id));
            _context.Users.Remove(_context.Users.First(x => x.UserId == id));
            _context.SaveChanges();
            TempData["Success"] = "रेकॉर्ड यशस्वीरित्या हटवला!";
            return RedirectToAction("Index");
        }





        // ================= TOGGLE USER STATUS =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleUserStatus([FromBody] UserStatusVM model)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == model.UserId);
            if (user == null) return NotFound();

            user.IsActive = model.Status;
            _context.SaveChanges();

            return Ok();
        }



        [HttpGet]
        public IActionResult HashAllPasswords()
        {
            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    // Check if password is plain text
                    bool isHashed = true;
                    try
                    {
                        Convert.FromBase64String(user.PasswordHash);
                    }
                    catch
                    {
                        isHashed = false; // Plain text password
                    }

                    if (!isHashed)
                    {
                        user.PasswordHash = PasswordHelper.Hash(user.PasswordHash);
                    }
                }

                // Admin account active ठेवा
                if (user.UserName.ToLower() == "admin")
                    user.IsActive = true;
            }

            _context.SaveChanges();

            return Content("✅ All passwords hashed successfully, admin active!");
        }


        [HttpGet]
        public IActionResult ForceAdminReset()
        {
            var admin = _context.Users.FirstOrDefault(x => x.UserName == "admin");
            if (admin == null)
                return Content("Admin not found");

            admin.PasswordHash = PasswordHelper.Hash("admin123");
            admin.IsActive = true;

            _context.SaveChanges();

            return Content("✅ Admin password reset to admin123");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ResetUserPassword(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (user == null)
                return NotFound();

            var model = new AdminResetPasswordViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName
            };

            return View(model);
        }




        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetUserPassword(AdminResetPasswordViewModel model)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == model.UserId);
            if (user == null)
                return NotFound();

            user.PasswordHash = PasswordHelper.Hash(model.NewPassword);
            _context.SaveChanges();

            TempData["Success"] = $"✅ {user.UserName} चा पासवर्ड reset झाला";

            // ✅ Admin User List ला परत जा
            return RedirectToAction("Index");
        }






    }
}
    

