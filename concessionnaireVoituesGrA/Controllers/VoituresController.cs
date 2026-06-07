using concessionnaireVoituesGrA.Models;
using concessionnaireVoituesGrA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace concessionnaireVoituesGrA.Controllers
{
    public class VoituresController : Controller
    {
        InterfaceVoitures service;
        IWebHostEnvironment webHostEnvironment;
        public VoituresController(InterfaceVoitures service, IWebHostEnvironment webHostEnvironment)
        {
            this.service = service;
            this.webHostEnvironment = webHostEnvironment;
        }
        // GET: VoituresController
        public ActionResult Index()
        {
            return View(service.GetAllVoitures());
        }
        [Authorize(Roles = "Admin,Client")]
        // GET: VoituresController/Details/5
        public ActionResult Details(string id) //id=matricule
        {
            VoitureDto v=service.GetVoiture(id);
            return View(v);
        }
        [Authorize(Roles = "Admin")]
        // GET: VoituresController/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: VoituresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, VoitureDto voitureDto)
        {
            try
            {
                if (voitureDto.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "voitures");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + voitureDto.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        voitureDto.ImageFile.CopyTo(fileStream);
                    }
                    voitureDto.ImageUrl = "/images/voitures/" + uniqueFileName;
                }
                else
                {
                    voitureDto.ImageUrl = "/images/voitures/default.png";
                }

                // Une nouvelle voiture est toujours disponible par défaut
                voitureDto.EtatDisponibilite = "Disponible";
                bool res=service.AjouterVoiture(voitureDto);
                if (res)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ViewBag.ErrorMessage = "Matricule existe déjà";
                    return View(voitureDto);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erreur lors de l'ajout : " + ex.Message;
                return View(voitureDto);
            }
        }
        [Authorize(Roles = "Admin")]
        // GET: VoituresController/Edit/5
        public ActionResult Edit(string id)
        {
            VoitureDto v = service.GetVoiture(id);
            return View(v);
        }
        [Authorize(Roles = "Admin")]
        // POST: VoituresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection, VoitureDto voitureDto)
        {
            try
            {
                if (voitureDto.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "voitures");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + voitureDto.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        voitureDto.ImageFile.CopyTo(fileStream);
                    }
                    voitureDto.ImageUrl = "/images/voitures/" + uniqueFileName;
                }
                else
                {
                    // Conserver l'ancienne image si aucune nouvelle n'est téléchargée
                    var oldVoiture = service.GetVoiture(id);
                    voitureDto.ImageUrl = oldVoiture.ImageUrl;
                }

                service.ModifierVoiture(id, voitureDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erreur lors de la modification : " + ex.Message;
                return View(voitureDto);
            }
        }
        [Authorize(Roles = "Admin")]
        // GET: VoituresController/Delete/5
        public ActionResult Delete(string id)
        {

            VoitureDto v = service.GetVoiture(id);
            return View(v);
        }
        [Authorize(Roles = "Admin")]
        // POST: VoituresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                service.SupprimerVoiture(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erreur lors de la suppression : " + ex.Message;
                return View(service.GetVoiture(id));
            }
        }
    }
}
