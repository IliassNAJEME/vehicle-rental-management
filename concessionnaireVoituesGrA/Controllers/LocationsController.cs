using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace concessionnaireVoituesGrA.Controllers
{
    [Authorize(Roles = "Client")]
    public class LocationsController : Controller
    {
        InterfaceLocations serviceLocations;
        InterfaceVoitures serviceVoitures;

        public LocationsController(InterfaceLocations serviceLocations, InterfaceVoitures serviceVoitures)
        {
            this.serviceLocations = serviceLocations;
            this.serviceVoitures = serviceVoitures;
        }

        // Mes réservations: uniquement les demandes du client connecté
        public ActionResult MesDemandes()
        {
            var username = User.Identity!.Name!;
            var mesDemandes = serviceLocations.GetDemandesByClient(username);
            return View(mesDemandes);
        }

        // GET: LocationsController/Create?id=matricule
        public ActionResult Create(string id) // id = MatriculeVoiture
        {
            var voiture = serviceVoitures.GetVoiture(id);
            if (voiture == null) return NotFound();
            if (voiture.EtatDisponibilite != "Disponible")
            {
                TempData["Erreur"] = "Cette voiture n'est plus disponible.";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Voiture = voiture;
            return View();
        }

        // POST: LocationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                string matricule = collection["MatriculeVoiture"];
                DateTime dateDebut = DateTime.Parse(collection["DateDebut"]);
                DateTime dateFin = DateTime.Parse(collection["DateFin"]);

                if (dateFin <= dateDebut)
                {
                    ViewBag.Erreur = "La date de fin doit être postérieure à la date de début.";
                    ViewBag.Voiture = serviceVoitures.GetVoiture(matricule);
                    return View();
                }

                var voitureDto = serviceVoitures.GetVoiture(matricule);

                var location = new Location
                {
                    Voiture = new Voiture
                    {
                        Matricule = voitureDto.Matricule,
                        Marque = voitureDto.Marque,
                        Modele = voitureDto.Modele,
                        PrixLocation = voitureDto.PrixLocation,
                        EtatDisponibilite = voitureDto.EtatDisponibilite
                    },
                    Client = new Client { Nom = User.Identity!.Name! },
                    DateDebut = dateDebut,
                    DateFin = dateFin
                };

                serviceLocations.AjouterDemande(location);
                TempData["Succes"] = "Votre demande de location a été soumise avec succès !";
                return RedirectToAction(nameof(MesDemandes));
            }
            catch (Exception ex)
            {
                ViewBag.Erreur = "Une erreur s'est produite : " + ex.Message;
                ViewBag.Voiture = serviceVoitures.GetVoiture(collection["MatriculeVoiture"]);
                return View();
            }
        }
    }
}
