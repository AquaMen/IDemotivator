using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IDemotivator.Models;
using IDemotivator.App_LocalResources;
using IDemotivator.Filters;
using System.Net;
using IDemotivator.Search;

namespace IDemotivator.Controllers
{
    [Authorize]
    [Culture]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        Entities db = new Entities();

        public int GetCommentCount(string id)
        {
            var demotivators = db.Demotivators.ToList();
            int count = 0;
            foreach (var item in demotivators)
            {
                count += item.Comments.Where(d => d.AspNetUserId == id).Count();     
            }
            return (count);
        }

        public int GetLikeCount(string id)
        {
            var demotivators = db.Demotivators.ToList();
            int count = 0;
            foreach (var item in demotivators)
            {
                foreach (var item1 in item.Comments.Where(d=> d.AspNetUserId == id))
                {
                    count += item1.Likes.Count();
                }
            }
            return (count);
        }

        public Single RateNow( ICollection<Demotivator> demotivators)
        {
            int currentVotesCount;
            int totalNumberOfVotes;
            int totalVoteCount;
            Single m_Average = 0;
            int count = 0;
            Single Average = 0;
            foreach (var item in demotivators)
            {
                if (item.rates.Count() == 0)
                { continue; }
                currentVotesCount = 0;
                totalNumberOfVotes = 0;
                totalVoteCount = 0;
                if (item.Rate.Length>0)
                {
                    count++;
                    string[] votes = item.Rate.Split(',');
                    for (int i = 0; i < votes.Length; i++)
                    {
                        currentVotesCount = int.Parse(votes[i]);
                        totalNumberOfVotes = totalNumberOfVotes + currentVotesCount;
                        totalVoteCount = totalVoteCount + (currentVotesCount * (i + 1));
                    }
                    m_Average = totalVoteCount / totalNumberOfVotes;
                }
                Average += m_Average;
            }
            if (Average != 0)
            {
                Average /= count;
            }
            return (Average);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Profile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser user = await db.AspNetUsers.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var demotivators = db.Demotivators.Where(d => d.AspNetUserId == user.Id).ToList();
            ProfileViewModel User = new ProfileViewModel();
            User.Demotivator = demotivators;
            User.User = user;
            User.Rate = RateNow(demotivators);
            User.LikeCount = GetLikeCount(id);
            User.CommentCount = GetCommentCount(id);
            return View(User);
        }
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    if (user.EmailConfirmed == true)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.Resource.NotConfirmedEmail);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resource.IncorrectLoginOrPassword);
                }
            }
            return View(model);
        }
       
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                using (var elastic = new elasticsearchNEST())
                {
                    elastic.Adding(user);
                }
                if (result.Succeeded)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                               protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, Resources.Resource.MailTheme,
                               Resources.Resource.MailMessage + " <a href=\"" + callbackUrl + "\">here</a>");
                    return View("DisplayEmail");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            base.Dispose(disposing);
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}