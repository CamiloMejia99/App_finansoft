using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Login;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Framework.Linq;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Controllers
{
    public class LoginController : Controller
    {
        private AccountingContext db = new AccountingContext();
        //jme
        public LoginController()
        {
        }
        public LoginController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
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
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //jme



        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Error = 0;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginUserViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = 0;
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);

            if (user != null)
            {

                if (!await UserManager.IsEmailConfirmedAsync(user.Id)) return View("ErrorNotConfirmed");

            }



            var parametros = db.EmailParameter.Where(i => i.Parametro_id == 1).FirstOrDefault();
            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:

                    user.LastActivityDate = DateTime.Now;
                    UserManager.Update(user);

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:

                    ModelState.AddModelError("", "Intento de inicio de sesión inválido...");

                    var intentos = (from intento in db.AspNetUsersApp where intento.UserName == model.UserName select new { intento.AccessFailedCount }).FirstOrDefault();
                    if (intentos != null)
                    {
                        ViewBag.Error = intentos.AccessFailedCount;
                    }
                    else if (intentos == null)
                    {
                        ViewBag.Error = null;
                    }



                    return View(model);
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión inválido...");
                    return View(model);
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






        public ActionResult CambiarContrasenia()
        {

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            var usuarios = (from users in db.AspNetUsersApp select users).ToList();
            ViewBag.count = usuarios.Count();

            if (TempData["callbackUrl"] != null)
            {
                ViewBag.Message = "Se han enviado las instrucciones al correo electronico " + TempData["callbackUrl"];
            }
            else if (TempData["succesuser"] != null)
            {
                ViewBag.Message = TempData["succesuser"];
            }
            else
            {

                ViewBag.Message = null;
            }

            return View(usuarios);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasenia([Bind(Include = "id,usuario,password,nombre")] ControlAcceso ControlAcceso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ControlAcceso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Default", new { Area = "Dashboard" });
            }
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            // AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        //REGISTER NUEVO PASSWORD
        //=======================
        [AllowAnonymous]
        public ActionResult Register(int id)
        {
            return View();
        }

        //POST REGISTER NUEVO PASSWORD
        //============================
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null || user.EmailConfirmed == false)
                {
                    // Don't reveal that the user does not exist or is not confirmed

                    if (user == null)
                    {
                        ViewBag.mensaje = "El usuario no existe...";
                    }
                    else
                    {
                        ViewBag.mensaje = "No se ha confirmado la cuenta de correo...";
                    }
                    ViewBag.mensaje = "La dirección de correo " + model.Email + " no se encuentra registrada en la Base de Datos...";
                    return View("ForgotPasswordConfirmationError");
                }


                var Userid = await UserManager.FindByEmailAsync(model.Email);
                var code = await UserManager.GeneratePasswordResetTokenAsync(Userid.Id);
                var callbackUrl = Url.Action("CreatePassword", "Login", new { userId = Userid.Id, code = code }, protocol: Request.Url.Scheme);

                string body = "Pulse en el siguiente enlace para cambiar su clave: <a href='" + callbackUrl + "' target='_blank'> Has Click Aquí <i class='fa fa-link'></i> </a>";

                await UserManager.SendEmailAsync(Userid.Id, "FINANSOFT - Restablecimiento de Clave", body);
                ViewBag.Link = callbackUrl;
                TempData["callbackUrl"] = model.Email;
                return RedirectToAction("Users");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult CreatePasswordConfirmation()
        {
            return View();
        }

        //CREAR CONTRASEÑA
        [AllowAnonymous]
        public ActionResult CreatePassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword UPDATE PASS
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePassword(ResetPasswordViewModelJme model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.mensaje = "El usuario no existe...";
                return RedirectToAction("ResetPasswordConfirmationError", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {

                return RedirectToAction("ResetPasswordConfirmation", "Login");
            }
            AddErrors(result);
            return View();
        }

        //REGISTRO
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterUserViewModel model)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    var datos = (from email in db.Terceros where email.EMAIL == model.Email select email).FirstOrDefault();
                    var fecha = DateTime.Now;
                    var user = new ApplicationUser { UserName = datos.NIT, Email = model.Email, Nombres = datos.NOMBRE + datos.APELLIDO1, Cedula = datos.NIT, LastPasswordChangedDate = fecha };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Login", "Login");
                    }
                    AddErrors(result);

                }
            }
            catch (Exception e)
            {


            }
            return View();
        }
        //FIN REGISTRO

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmationError()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //CREATE
        public async Task<ActionResult> Create()
        {

            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Description");
            return View();
        }
        //CREACION DE ROLES Y USUARIO
        [HttpPost]
        public async Task<ActionResult> Create(RegisterUserViewModel userViewModel, params string[] selectedRoles)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    // Add the Address Info:
                    Address = userViewModel.Address,
                    City = userViewModel.City,
                    State = userViewModel.State,
                    PhoneNumber = userViewModel.PhoneNumber,

                    Nombres = userViewModel.Nombres,
                    LastPasswordChangedDate = DateTime.Now,
                    LastActivityDate = DateTime.Now,
                    Fecha_Registro = DateTime.Now,
                    EmailConfirmed = true,


                };

                // Add the Address Info:
                user.Address = userViewModel.Address;
                user.City = userViewModel.City;
                user.State = userViewModel.State;
                user.PhoneNumber = userViewModel.PhoneNumber;

                // Then create:
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                    TempData["succesuser"] = "Usuario Registrado Con Éxito!!!!";
                    return RedirectToAction("Users");
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());

                    return RedirectToAction("Users");

                }




            }

            return RedirectToAction("Users");
        }

    }
}