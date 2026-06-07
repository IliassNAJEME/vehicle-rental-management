using concessionnaireVoituesGrA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace concessionnaireVoituesGrA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly InterfaceVoitures _serviceVoitures;
        private readonly InterfaceLocations _serviceLocations;

        public AdminController(InterfaceVoitures serviceVoitures, InterfaceLocations serviceLocations)
        {
            _serviceVoitures = serviceVoitures;
            _serviceLocations = serviceLocations;
        }

        public IActionResult Index()
        {
            var demandes = _serviceLocations.GetAllDemandes();
            var voitures = _serviceVoitures.GetAllVoitures();

            // Calcul des KPIs pour le dashboard
            ViewBag.Demandes = demandes;
            ViewBag.TotalDemandes = demandes.Count;
            ViewBag.DemandesEnAttente = demandes.Count(d => d.Statut == "En attente");
            ViewBag.DemandesValidees = demandes.Count(d => d.Statut == "Validée");
            ViewBag.RevenusTotal = demandes.Where(d => d.Statut == "Validée").Sum(d => d.MontantTotal);
            ViewBag.VoitureDispo = voitures.Count(v => v.EtatDisponibilite == "Disponible");
            ViewBag.VoitureLouee = voitures.Count(v => v.EtatDisponibilite == "Louée");

            return View(voitures);
        }

        // SECURITY FIX: [HttpPost] + [ValidateAntiForgeryToken] — évite le CSRF
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValiderDemande(int id)
        {
            try
            {
                _serviceLocations.ValiderDemande(id);
                TempData["ToastSuccess"] = "Demande validée. La voiture est maintenant marquée comme louée.";
            }
            catch (Exception ex)
            {
                TempData["ToastError"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RefuserDemande(int id)
        {
            try
            {
                _serviceLocations.RefuserDemande(id);
                TempData["ToastSuccess"] = "Demande refusée. La voiture est redevenue disponible.";
            }
            catch (Exception ex)
            {
                TempData["ToastError"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
