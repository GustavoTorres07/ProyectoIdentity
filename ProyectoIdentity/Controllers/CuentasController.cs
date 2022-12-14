using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdentity.Models;

namespace ProyectoIdentity.Controllers
{
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {

            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {

            RegistroViewModel regisitroVM = new RegistroViewModel();
            return View(regisitroVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel rgViewModel)
        {


            if (ModelState.IsValid)
            {

                var usuario = new AppUsuario
                {
                    UserName = rgViewModel.Email,
                    Email = rgViewModel.Email,
                    Nombre = rgViewModel.Nombre,
                    Url = rgViewModel.Url,
                    CodigoPais = rgViewModel.CodigoPais,
                    Telefono = rgViewModel.Telefono,
                    Pais = rgViewModel.Pais,
                    Ciudad = rgViewModel.Ciudad,
                    Direccion = rgViewModel.Direccion,
                    FechaNacimiento = rgViewModel.FechaNacimiento,
                    Estado = rgViewModel.Estado
                };

                var resultado = await _userManager.CreateAsync(usuario, rgViewModel.Password);

                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(usuario, isPersistent: false);

                    return RedirectToAction("Index", "Home");


                }
                ValidarErrores(resultado);
            }

            return View(rgViewModel);  

        }

        //Manejador de errores
        private void ValidarErrores(IdentityResult resultado)
        {

            foreach (var error in resultado.Errors)
            {

                ModelState.AddModelError(string.Empty, error.Description);


            }

        }

        //Metodo mostrar formulario de acceso
        [HttpGet]
        public IActionResult Acceso()
        {
            return View();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoViewModel accViewModel)
        {


            if (ModelState.IsValid)
            {

                var resultado = await _signInManager.PasswordSignInAsync(accViewModel.Email,
                    accViewModel.Password, accViewModel.RememberMe, lockoutOnFailure: false);

                if (resultado.Succeeded)
                {
                    
                    return RedirectToAction("Index", "Home");


                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Acceso Invalido");
                    return View(accViewModel);
                }
            }

            return View(accViewModel);

        }
        //Salir o cerrar sesion de la aplicacion(logout)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplicacion()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index),"Home");

        }


    }
}
