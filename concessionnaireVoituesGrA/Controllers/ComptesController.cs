using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Models;
using concessionnaireVoituesGrA.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace concessionnaireVoituesGrA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ComptesController : Controller
    {
        private readonly InterfaceComptes _service;

        public ComptesController(InterfaceComptes service)
        {
            _service = service;
        }

        // GET: ComptesController
        public ActionResult Index()
        {
            return View(_service.GetComptes());
        }

        public ActionResult Details(string id)
        {
            var compte = _service.Rechercher(id);
            if (compte == null) return NotFound();
            return View(compte);
        }

        // ─── AUTHENTIFICATION (Public) ───────────────────────────────────────
        [AllowAnonymous]
        public ActionResult Authentifier()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Authentifier(CompteDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MessageErreur = "Veuillez remplir tous les champs.";
                return View(model);
            }

            try
            {
                var authenticatedUser = _service.Authentifier(model);
                if (authenticatedUser != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, authenticatedUser.Username),
                        new Claim(ClaimTypes.Role, authenticatedUser.Role ?? "Client")
                    };
                    var identity = new ClaimsIdentity(claims, "Cookies");
                    // FIX: await obligatoire — sans lui, l'auth échouait silencieusement
                    await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                    TempData["ToastSuccess"] = $"Bienvenue, {authenticatedUser.Username} !";

                    return authenticatedUser.Role == "Admin"
                        ? RedirectToAction("Index", "Admin")
                        : RedirectToAction("Index", "Home");
                }

                ViewBag.MessageErreur = "Identifiants incorrects. Vérifiez votre nom d'utilisateur et mot de passe.";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.MessageErreur = "Une erreur technique s'est produite. Réessayez.";
                // Log en production : _logger.LogError(ex, "Auth error");
                return View(model);
            }
        }

        // ─── DÉCONNEXION (Public) ────────────────────────────────────────────
        [AllowAnonymous]
        public async Task<ActionResult> Deconnexion()
        {
            await HttpContext.SignOutAsync();
            TempData["ToastInfo"] = "Vous avez été déconnecté avec succès.";
            return RedirectToAction("Index", "Home");
        }

        // ─── INSCRIPTION (Public) ─────────────────────────────────────────────
        [AllowAnonymous]
        public ActionResult Creer()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creer(CompteDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MessageErreur = "Formulaire invalide.";
                return View(model);
            }

            try
            {
                // Validation insensible à la casse — évite admin/Admin/ADMIN
                if (model.Username.Trim().ToLower() == "admin")
                {
                    ViewBag.MessageErreur = "Ce nom d'utilisateur est réservé.";
                    return View(model);
                }

                // Vérifier si le username existe déjà
                var existant = _service.Rechercher(model.Username.Trim());
                if (existant != null)
                {
                    ViewBag.MessageErreur = "Ce nom d'utilisateur est déjà pris.";
                    return View(model);
                }

                model.Username = model.Username.Trim();
                _service.Creer(model);
                TempData["ToastSuccess"] = "Compte créé avec succès ! Connectez-vous maintenant.";
                return RedirectToAction(nameof(Authentifier));
            }
            catch (Exception)
            {
                ViewBag.MessageErreur = "Une erreur s'est produite lors de la création du compte.";
                return View(model);
            }
        }

        // ─── GESTION ADMIN ────────────────────────────────────────────────────
        public ActionResult Edit(string id)
        {
            var compte = _service.Rechercher(id);
            if (compte == null) return NotFound();
            return View(compte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, CompteDto model)
        {
            try
            {
                _service.Modifier(id, model);
                TempData["ToastSuccess"] = "Compte modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Erreur lors de la modification.";
                return View(model);
            }
        }

        public ActionResult Delete(string id)
        {
            var compte = _service.Rechercher(id);
            if (compte == null) return NotFound();
            return View(compte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                _service.Supprimer(id);
                TempData["ToastSuccess"] = "Compte supprimé.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Erreur lors de la suppression.";
                return View(_service.Rechercher(id));
            }
        }
    }
}
