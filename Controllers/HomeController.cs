using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using petpet.Models;

namespace petpet.Controllers {
    public class HomeController : Controller {
        private Context dbContext;
        public Dictionary<string, string> AllPetInfo = new Dictionary<string, string> () { { "babyfox", "/images/babyfox.png" }, { "adultfox", "https://imgur.com/Ejyjg3a" }, { "babysquirrel", "" }, { "adultsquirrel", "" }, { "babydeer", "" }, { "adultdeer", "" }, { "babypenguin", "" }, { "adultpenguin", "" }, { "babysharky", "https://imgur.com/5PbGvak" }, { "sharky", "https://imgur.com/KwuNcxo" }
        };

        public HomeController (Context context) {
            dbContext = context;
        }
        public IActionResult Index () {
            return View ();
        }

        [HttpGet ("register")]
        public IActionResult register () {
            return View ("registration");
        }

        [HttpPost ("user/new")]
        public IActionResult createuser (User newUser) {
            if (ModelState.IsValid) {
                User exists = dbContext.Users.FirstOrDefault (p => p.Email == newUser.Email);
                if (exists != null) {
                    TempData["errors"] = "Email already exists.";
                    ModelState.AddModelError ("Email", "Email already taken.");
                    return RedirectToAction ("register");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User> ();
                    newUser.Password = Hasher.HashPassword (newUser, newUser.Password);
                    dbContext.Add (newUser);
                    dbContext.SaveChanges ();
                    User currentUser = dbContext.Users.FirstOrDefault (p => p.Email == newUser.Email);
                    HttpContext.Session.SetInt32 ("userId", currentUser.UserId);
                    return RedirectToAction ("createpet");
                }
            }
            TempData["errors"] = "There were problems with your registration info.";
            return RedirectToAction ("register");
        }

        [HttpPost ("user/login")]
        public IActionResult login (Login user) {
            if (ModelState.IsValid) {
                User userInDb = dbContext.Users.FirstOrDefault (p => p.Email == user.lemail);
                if (userInDb == null) {
                    TempData["lerror"] = "Incorrect Login Credentials";
                    return RedirectToAction ("Index");
                }
                var hasher = new PasswordHasher<Login> ();
                var result = hasher.VerifyHashedPassword (user, userInDb.Password, user.lpassword);

                if (result == 0) {
                    ModelState.AddModelError ("lpassword", "Invalid Email/Password");
                    TempData["lerror"] = "Incorrect Login Credentials";
                    return RedirectToAction ("Index");
                } else {
                    HttpContext.Session.SetInt32 ("userId", userInDb.UserId);
                    Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
                    if (pet == null) {
                        return RedirectToAction ("createpet");
                    }
                    return RedirectToAction ("success");
                }
            } else {
                TempData["lerror"] = "Incorrect Login Credentials";
                return RedirectToAction ("Index");
            }
        }

        [HttpGet ("pet/create")]
        public IActionResult createpet () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            } else {
                User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
                return View ("newpet", currentUser);
            }
        }

        [HttpPost ("pet/select")]
        public IActionResult petselect (int petValue) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            return RedirectToAction ("petcustomization", new { petvalue = petValue });
        }

        [HttpGet ("pet/info/{petValue}")]
        public IActionResult petcustomization (int petValue) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            string peturl = "";
            switch (petValue) {
                case 1:
                    peturl = "/images/babyfox.png"; // Fox
                    break;
                case 2:
                    peturl = "/images/babysquirrel.png"; // Squirrel
                    break;
                case 3:
                    peturl = "/images/FireDeer.png"; // Deer
                    break;
                case 4:
                    peturl = "/images/FireDeer.png"; // Penguin
                    break;
                default:
                    break;
            }
            ViewBag.PetId = petValue;
            ViewBag.PetUrl = peturl;
            return View ();
        }

        [HttpPost ("pet/creation")]
        public IActionResult addpet (Pet newPet) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            if (ModelState.IsValid) {
                string peturl = "";
                switch (newPet.PetValue) {
                    case 1:
                        peturl = "/images/babyfox.png"; // Fox
                        break;
                    case 2:
                        peturl = "/images/babysquirrel.png"; // Squirrel
                        break;
                    case 3:
                        peturl = "/images/FireDeer.png"; // Deer
                        break;
                    case 4:
                        peturl = "/images/FireDeer.png"; // Penguin
                        break;
                    default:
                        break;
                }
                User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
                newPet.UserId = currentUser.UserId;
                newPet.PetPicture = peturl;
                newPet.PetExperience = 0;
                newPet.PetLevel = 1;
                newPet.PetHunger = 100;
                newPet.PetHappiness = 100;
                dbContext.Pets.Add (newPet);
                dbContext.SaveChanges ();
                currentUser.Pet = newPet;
                return RedirectToAction ("success");
            }
            TempData["peterror"] = "Pet name must be at least 2 characters and a bio is required.";
            return RedirectToAction ("petcustomization", new {
                petValue = newPet.PetValue
            });
        }

        [HttpGet ("dashboard")]
        public IActionResult success () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            dashboardModel dashboard = new dashboardModel ();
            dashboard.userLogged = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            dashboard.pet = dbContext.Pets.Include (p => p.PetComments).FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            return View ("dashboard", dashboard);
        }

        [HttpGet ("pet/show/{petid}")]
        public IActionResult showPet (int PetId) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            Pet petinfo = dbContext.Pets.Include (p => p.UserId).FirstOrDefault (p => p.PetId == PetId);
            return View ("showPet", petinfo);
        }

        // *********************Beginning of Mailbox********************* 

        [HttpGet ("user/mailbox")]
        public IActionResult mailbox () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.Include (p => p.UserMail).OrderByDescending (p => p.CreatedAt).FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            List<Mail> allMail = dbContext.AllMail.ToList ();
            ViewBag.UserId = currentUser.UserId;
            return View (allMail);
        }

        [HttpGet ("mail/new")]
        public IActionResult newmail () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            ViewBag.UserList = dbContext.Users.ToList ();
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            ViewBag.UserLoggedId = currentUser.UserId;
            return View ();
        }

        [HttpPost ("mail/create")]
        public IActionResult createMail (Mail newMail) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            if (ModelState.IsValid) {
                User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
                newMail.UserId = currentUser.UserId;
                User recipient = dbContext.Users.FirstOrDefault (p => p.UserId == newMail.RecipientId);
                newMail.AuthorName = currentUser.Name;
                dbContext.AllMail.Add (newMail);
                dbContext.SaveChanges ();
                return RedirectToAction ("mailbox");
            }
            TempData["EmailError"] = "Must specify a recipient and mail must have content.";
            return RedirectToAction ("newmail");
        }

        [HttpGet ("mail/show/{mailId}")]
        public IActionResult showMail (int mailId) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            Mail mail = dbContext.AllMail.FirstOrDefault (p => p.MailId == mailId);
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            return View (mail);
        }

        [HttpGet ("mail/reply/{mailId}")]
        public IActionResult replyMail (int mailId) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            Mail mail = dbContext.AllMail.FirstOrDefault (p => p.MailId == mailId);
            User replyUser = dbContext.Users.FirstOrDefault (p => p.UserId == mail.UserId);
            ViewBag.UserToReply = replyUser.Name;
            ViewBag.RecipientId = replyUser.UserId;
            return View (mail);
        }

        [HttpPost ("mail/send/{mailId}")]
        public IActionResult sendReply (Mail newMail) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            User recipient = dbContext.Users.FirstOrDefault (p => p.UserId == newMail.RecipientId);
            if (ModelState.IsValid) {
                Mail mail = new Mail ();
                mail.RecipientId = newMail.RecipientId;
                mail.Subject = newMail.Subject;
                mail.Content = newMail.Content;
                mail.AuthorName = currentUser.Name;
                mail.UserId = currentUser.UserId;
                mail.RecipientName = recipient.Name;
                dbContext.AllMail.Add (mail);
                dbContext.SaveChanges ();
                return RedirectToAction ("mailbox");
            }
            TempData["mailError"] = "Sent mail cannot be empty.";
            return RedirectToAction ("mailbox");
        }

        [HttpPost ("mail/delete/{mailId}")]
        public IActionResult deleteMail (int mailId) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            Mail mail = dbContext.AllMail.FirstOrDefault (p => p.MailId == mailId);
            dbContext.Remove (mail);
            dbContext.SaveChanges ();
            return RedirectToAction ("mailbox");
        }

        // *********************Beginning of Pet Interactions*********************
        [HttpGet ("play")]
        public IActionResult play () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet currentPet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            string PetImage = currentPet.PetPicture;
            Random top = new Random ();
            Random left = new Random ();
            ViewBag.Top = (top.NextDouble () * 100);
            ViewBag.Left = (left.NextDouble () * 100);
            return View ("play", currentPet);
        }

        [HttpGet ("play/success")]
        public IActionResult foundsuccess () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            if (pet.PetHunger < 30) {
                TempData["error"] = "Your pet is too hungry to play!";
                return RedirectToAction ("success");
            }
            if (pet.PetHappiness < 95) {
                pet.PetHappiness += 5;
            } else {
                pet.PetHappiness = 100;
            }
            if (pet.PetExperience + 50 >= 100) {
                pet.PetLevel += 1;
                pet.PetExperience = pet.PetExperience - 50;
                TempData["level"] = "Your Pet also leveled up!";
            } else {
                pet.PetExperience += 50;
            }
            if (pet.PetLevel == 10 && pet.isAdult == false) {
                pet.isAdult = true;
                TempData["petEvo"] = "Your pet evolved!";
                // Function to change the pet image to evo'd pet
            }
            pet.PetHunger -= 10;
            dbContext.SaveChanges ();
            return RedirectToAction ("showPlayResults");
        }

        [HttpGet ("play/results")]
        public IActionResult showPlayResults () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            return View ();
        }

        [HttpGet ("show/{PetId}")]
        public IActionResult PetPage (int PetId) {
            Pet pet = dbContext.Pets.Include (p => p.Creator).Include (p => p.PetComments).ThenInclude (p => p.Author).FirstOrDefault (p => p.PetId == PetId);
            if (pet == null) {
                if (HttpContext.Session.GetInt32 ("userId") == null) {
                    TempData["error"] = "No pets found with that ID";
                    return RedirectToAction ("Index");
                } else {
                    TempData["error"] = "No pets found with that ID";
                    return RedirectToAction ("success");
                }
            }
            PetAndComments pac = new PetAndComments ();
            pac.pet = pet;
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            if (currentUser != null) {
                ViewBag.UserId = currentUser.UserId;
            }
            return View (pac);
        }

        [HttpPost ("comment/new/{petId}")]
        public IActionResult newComment (int petId, Comment comment) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            if (ModelState.IsValid) {
                User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
                comment.Author = currentUser;
                comment.PetId = petId;
                dbContext.PetComments.Add (comment);
                dbContext.SaveChanges ();
                return RedirectToAction ("PetPage", new { petId = petId });
            }
            TempData["error"] = "Message cannot be blank.";
            return RedirectToAction ("PetPage", new {
                petId = petId
            });
        }

        [HttpGet ("comment/delete/{commentId}")]
        public IActionResult deleteComment (int commentId) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            Comment comment = dbContext.PetComments.FirstOrDefault (p => p.CommentId == commentId);
            dbContext.PetComments.Remove (comment);
            dbContext.SaveChanges ();
            return RedirectToAction ("success");
        }

        [HttpGet ("user/edit")]
        public IActionResult editPet () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            return View (pet);
        }

        [HttpPost ("user/pet/edit/submit")]
        public IActionResult submitChanges (Pet currenPet) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            pet.PetBio = currenPet.PetBio;
            pet.PetName = currenPet.PetName;
            dbContext.SaveChanges ();
            return RedirectToAction ("success");
        }

        [HttpGet ("pet/feed")]
        public IActionResult feedPet () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            if (currentUser.Balance < 1) {
                TempData["error"] = "You have no meals remaining! Go get more.";
                return RedirectToAction ("success");
            }
            if (pet.PetHunger + 20 >= 100) {
                pet.PetHunger = 100;
            } else {
                pet.PetHunger += 20;
            }
            currentUser.Balance -= 1;
            dbContext.SaveChanges ();
            return View ("feedSuccess");
        }

        [HttpGet ("pet/food/get")]
        public IActionResult getFood () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            ViewBag.TopPos1 = Randomizer ();
            ViewBag.BotPos1 = Randomizer ();
            ViewBag.TopPos2 = Randomizer ();
            ViewBag.BotPos2 = Randomizer ();
            ViewBag.TopPos3 = Randomizer ();
            ViewBag.BotPos3 = Randomizer ();
            ViewBag.TopPos4 = Randomizer ();
            ViewBag.BotPos4 = Randomizer ();
            ViewBag.TopPos5 = Randomizer ();
            ViewBag.BotPos5 = Randomizer ();
            ViewBag.TopPos6 = Randomizer ();
            ViewBag.BotPos6 = Randomizer ();
            ViewBag.TopPos7 = Randomizer ();
            ViewBag.BotPos7 = Randomizer ();
            ViewBag.TopPos8 = Randomizer ();
            ViewBag.BotPos8 = Randomizer ();
            ViewBag.TopPos9 = Randomizer ();
            ViewBag.BotPos9 = Randomizer ();
            ViewBag.TopPos10 = Randomizer ();
            ViewBag.BotPos10 = Randomizer ();
            return View ();
        }

        [HttpGet ("food/success")]
        public IActionResult foodSuccess () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            currentUser.Balance += 2;
            return RedirectToAction ("success");
        }

        [HttpGet ("release/pet/{petId}")]
        public IActionResult releasePet () {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            User currentUser = dbContext.Users.FirstOrDefault (p => p.UserId == HttpContext.Session.GetInt32 ("userId"));
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.UserId == currentUser.UserId);
            return View (pet);
        }

        [HttpPost ("pet/delete/{petId}")]
        public IActionResult deletePet (int PetId) {
            if (HttpContext.Session.GetInt32 ("userId") == null) {
                return RedirectToAction ("Index");
            }
            Pet pet = dbContext.Pets.FirstOrDefault (p => p.PetId == PetId);
            dbContext.Pets.Remove (pet);
            dbContext.SaveChanges ();
            return RedirectToAction ("createpet");
        }

        [HttpGet ("logout")]
        public IActionResult logout () {
            HttpContext.Session.Clear ();
            return RedirectToAction ("Index");
        }
        public IActionResult Privacy () {
            return View ();
        }

        public double Randomizer () {
            Random top = new Random ();
            double rand = new double ();
            rand = top.NextDouble () * 90;
            return rand;
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}