﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FeriaVirtualWeb.Filter;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.DataManager;
using FeriaVirtualWeb.Models.ViewModels;

namespace FeriaVirtualWeb.Controllers
{
    [UserAuthorization(Rol = 5)]
    public class ProcesoVentaController : Controller
    {
        CollectionManager collection = new CollectionManager();
        public ActionResult ProcesoVentaList()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            return View();
        }

        public ActionResult ProcesosVentaList()
        {
            var lista = collection.GetClientListProcesoVenta();
            return View(lista);
        }

        public ActionResult ProductosListAccordingProcesoVenta(decimal id)
        {
            var productos = new List<PRODUCTO>();
            productos = collection.GetProductClientByOrderAndProductorNull(id);
            return View(productos);
        }
   

        public JsonResult Postular(decimal orden)
        {
            var procesoManager = new ProcesoVentaManager();
            var productos = new List<PRODUCTO>();
            var pPostulacion = new List<PRODUCTO>();
            var newOrden = orden;
            var procesoOr = orden;
            var proceso = new PROCESOVENTA();
   
            productos = collection.GetProductClientByOrderAndProductorNull(newOrden);
            proceso = collection.GetProcesoByOrden(procesoOr);
            var usuario = (USUARIO)Session["usuario"];
            pPostulacion = collection.GetProductsProductorAccordingToProcesoVenta(productos, usuario);
            if (pPostulacion.Count() != 0)
            {
                var productsInserted = procesoManager.InsertProcesoVentaAccordingToUsuario(pPostulacion, proceso.IDPROCESOVENTA, newOrden);
                foreach (var item in productos)
                {
                    if (item.IDPROCESOVENTA == null)
                    {
                        procesoManager.InsertOrderToProceso(productos, proceso.IDPROCESOVENTA);
                    }
                }
                
                procesoManager.UpdateStockProductsAfterPostular(productos, usuario, proceso.IDPROCESOVENTA);
            }
          
            return Json(pPostulacion);
        }

        public ActionResult MyPostulaciones()
        {
            var usuario = (USUARIO)Session["usuario"];
            ViewBag.session = usuario.NOMBREUSUARIO;
            var listaP = collection.GetMyPostulaciones(usuario);
            return View(listaP);
        }

        public ActionResult MyPostulacionesDetails(decimal? id)
        {
            var idp = id;
            var detalle = new ProcesoVentaViewModel();
            var listaP = new List<PRODUCTO>();
            var usuario = (USUARIO)Session["usuario"];
            if (id != 0)
            {
                detalle = collection.GetMyPostulacionDetails(id);
            }

            listaP = collection.GetProductsListAccordingToPostulacion(idp, usuario);
            ViewBag.productos = listaP;
            return View(detalle);
        }
        
    }
}
