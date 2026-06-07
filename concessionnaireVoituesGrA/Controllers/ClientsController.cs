using concessionnaireVoituesGrA.Models;
using concessionnaireVoituesGrA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace concessionnaireVoituesGrA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {
        InterfaceClients service;
        public ClientsController(InterfaceClients service)
        {
            this.service = service;
        }   
        // GET: ClientsController
        public ActionResult Index()
        {
            return View(service.GetAllClients());
        }

        // GET: ClientsController/Details/5
        public ActionResult Details(string id)
        {
                ClientDto client = service.GetClient(id);
                if (client != null)
                    return View(client);
            else
                return View();
        }

        // GET: ClientsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, ClientDto    model)
        {
            try
            {
                service.Ajouter(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erreur de création: " + ex.Message;
                return View(model);
            }
        }

        // GET: ClientsController/Edit/5
        public ActionResult Edit(string id)
        {
                ClientDto client = service.GetClient(id);
                    if (client != null)
                        return View(client);
            else
                return NotFound();
        }

        // POST: ClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection, ClientDto model)
        {
            try
            {
                if(service.Modifier(id, model))
                    return RedirectToAction(nameof(Index));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erreur de modification: " + ex.Message;
                return View(model);
            }
        }

        // GET: ClientsController/Delete/5
        public ActionResult Delete(string id)
        {
                ClientDto client = service.GetClient(id);
                    if (client != null)
                        return View(client);
            else
                return NotFound();
        }

        // POST: ClientsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                if(service.Supprimer(id))
                    return RedirectToAction(nameof(Index));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erreur de suppression: " + ex.Message;
                return View(service.GetClient(id));
            }
        }
    }
}
