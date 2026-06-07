using concessionnaireVoituesGrA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace concessionnaireVoituesGrA.Controllers
{
    public class HomeController : Controller
    {
        private readonly concessionnaireVoituesGrA.Services.InterfaceVoitures _serviceVoitures;
        private readonly concessionnaireVoituesGrA.Services.InterfaceLocations _serviceLocations;

        public HomeController(concessionnaireVoituesGrA.Services.InterfaceVoitures serviceVoitures, concessionnaireVoituesGrA.Services.InterfaceLocations serviceLocations)
        {
            _serviceVoitures = serviceVoitures;
            _serviceLocations = serviceLocations;
        }

        public IActionResult Index(string categorie, DateTime? dateDepart, DateTime? dateRetour)
        {
            var voitures = _serviceVoitures.GetAllVoitures();

            // Filtrage par catégorie
            if (!string.IsNullOrEmpty(categorie))
            {
                voitures = voitures.Where(v => v.Categorie != null && v.Categorie.Equals(categorie, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtrage par disponibilité (dates)
            if (dateDepart.HasValue && dateRetour.HasValue && dateDepart <= dateRetour)
            {
                var demandes = _serviceLocations.GetAllDemandes();
                var voituresIndisponibles = demandes
                    .Where(d => d.Statut == "Validee" || d.Statut == "Validée" || d.Statut == "En attente")
                    .Where(d => d.DateDebut <= dateRetour.Value && d.DateFin >= dateDepart.Value)
                    .Select(d => d.Voiture.Matricule)
                    .Distinct()
                    .ToList();

                voitures = voitures.Where(v => !voituresIndisponibles.Contains(v.Matricule)).ToList();
            }

            // Conserver les valeurs de recherche dans la vue
            ViewBag.Categorie = categorie;
            ViewBag.DateDepart = dateDepart?.ToString("yyyy-MM-dd");
            ViewBag.DateRetour = dateRetour?.ToString("yyyy-MM-dd");

            return View(voitures);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
