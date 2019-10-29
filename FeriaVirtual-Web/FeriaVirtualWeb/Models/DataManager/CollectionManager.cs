﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeriaVirtualWeb.Models.DataContext;
using FeriaVirtualWeb.Models.ViewModels;
using FeriaVirtualWeb.Models.DataManager;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class CollectionManager
    {
        public IEnumerable<PRODUCTO> GetProductosList()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRECIO == null && p.CANTIDAD == null).ToList();
            }
            
        }

        public IEnumerable<PRODUCTO> GetMyProductosList(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var listaRechazados = GetProductsRejected(usuario);
                var producto = new List<PRODUCTO>();
                var alocal = new ProductorManager();
                if (GetProductProcesoLocalEqualsToRejected(listaRechazados).Count() == 0)
                {
                    alocal.InsertProductosWhenHasBeedRejectedToLocal(listaRechazados);
                    ChangeRechazadosToMovidos(listaRechazados);
                }
                else
                {
                    alocal.UpdateProductosWhenHasBeedRejectedToLocal(listaRechazados);
                    ChangeRechazadosToMovidos(listaRechazados);
                }

                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.IDPROCESOVENTA == null && p.TIPOVENTA == "Externo").OrderBy(p => p.IDPRODUCTO).ToList();
            }

        }

        public PRODUCTO GetProductByIdProducto(decimal id)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPRODUCTO == id).FirstOrDefault();
            }
        }

        public List<ProcesoVentaViewModel> GetProcesoVentaLocalList()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pd in db.PRODUCTOR join pr in db.PRODUCTO
                            on pd.RUTPRODUCTOR equals pr.PRODUCTOR_RUTPRODUCTOR
                            join pv in db.PROCESOVENTA on pr.IDPROCESOVENTA equals
                            pv.IDPROCESOVENTA where pv.TIPOPROCESO == "Local"
                            select new ProcesoVentaViewModel
                            {
                                PROCESO = pv.IDPROCESOVENTA,
                                NOMBREPRODUCTOR = pd.NOMBRE,
                                CLIENTEINICIAL = pr.PRODUCTOR_RUTPRODUCTOR,
                                FECHA = pv.FECHA

                            }).Distinct().ToList();

                return query;
            }
        }

        public List<PRODUCTO> GetProcesoLocalProductsListOfProductor(decimal proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPROCESOVENTA == proceso && p.CANTIDAD == null && p.CLIENTEINTERNO == null).ToList();
            }
        }

        public List<PRODUCTO> GetProcesoLocalProductsListFilterByCantidad(List<PRODUCTO> productos)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return  productos.Where(p => p.CANTIDAD != null).ToList(); 
            }
        }

        public IEnumerable<PRODUCTO> GetMyProductListProcesoLocal(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.IDPROCESOVENTA == null && p.TIPOVENTA == "Local").OrderBy(p => p.IDPRODUCTO).ToList();
            }
        }

        private List<PRODUCTO> GetProductsRejected(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.ESTADOPROCESO == "Rechazado").ToList();
            }
        }

        private List<PRODUCTO> GetProductProcesoLocalEqualsToRejected(List<PRODUCTO> productos)
        {
            var lista = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in productos)
                {
                    lista = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION && p.TIPOVENTA == "Local" &&
                    p.PRODUCTOR_RUTPRODUCTOR == item.PRODUCTOR_RUTPRODUCTOR && p.IDPROCESOVENTA == null).ToList();
                }
                return lista;
            }
        }

        private void ChangeRechazadosToMovidos(List<PRODUCTO> rechazados)
        {
            try
            {
                using (FeriaVirtualEntities db = new FeriaVirtualEntities())
                {
                    foreach (var item in rechazados)
                    {
                        PRODUCTO producto = db.PRODUCTO.Where(p => p.PRODUCTOR_RUTPRODUCTOR == item.PRODUCTOR_RUTPRODUCTOR && p.ESTADOPROCESO == item.ESTADOPROCESO).FirstOrDefault();
                        producto.ESTADOPROCESO = "Movido";
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<PRODUCTO> GetProductsSelected(List<PRODUCTO> products)
        {
            return products.Where(p => p.CANTIDAD != null).ToList();
        }

        public List<PRODUCTO> GetProductosListSelected(List<PRODUCTO> products)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return products.Where(p => p.PRECIO != null && p.STOCK != null).ToList();
            }
        }

        public List<ORDEN> GetMyOrderList(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.ORDEN.Where(p => p.CLIENTE_RUTCLIENTE == usuario.RUTUSUARIO).OrderBy(p => p.IDORDEN).ToList();
            }
        }

        public ORDEN GetEstadoOrden(decimal orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.ORDEN.Where(or => or.IDORDEN == orden).FirstOrDefault();
            }
        }

        public List<PRODUCTO> GetMyProductsByOrders(decimal orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == orden && p.CANTIDAD != null && p.STOCK == null).ToList();
            }
        }

        public IEnumerable<ProcesoVentaViewModel> GetClientListProcesoVenta()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pv in db.PROCESOVENTA
                             join or in db.ORDEN
                             on pv.ORDENID equals or.IDORDEN
                             join cl in db.CLIENTE
                             on or.CLIENTE_RUTCLIENTE equals cl.RUTCLIENTE
                             select new ProcesoVentaViewModel()
                             {
                                 PROCESO = pv.IDPROCESOVENTA,
                                 ORDEN = pv.ORDENID,
                                 NOMBRECLIENTE = cl.NOMBRE,
                                 FECHA = pv.FECHA
                                 
                             }).OrderBy(p => p.ORDEN).ToList();

                return query as IEnumerable<ProcesoVentaViewModel>;
            }
        }

        public List<PRODUCTO> GetProductClientByOrderAndProductorNull(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == orden && p.PRODUCTOR_RUTPRODUCTOR == null).ToList();
            }
        }

        public List<PRODUCTO> GetProductsListAccordingToPostulacion(decimal? orden, USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.ORDEN_IDORDEN == orden && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO).ToList();
            }
        }

        public List<PRODUCTO> GetProductsProductorAccordingToProcesoVenta(List<PRODUCTO> productos,USUARIO usuario)
        {
            var productosP = new PRODUCTO();
            var newList = new List<PRODUCTO>();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                foreach (var item in productos)
                {
                    productosP = db.PRODUCTO.Where(p => p.DESCRIPCION == item.DESCRIPCION && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.TIPOVENTA == "Externo").FirstOrDefault();
                    if(productosP != null && productosP.STOCK >= 1)
                    {
                        newList.Add(new PRODUCTO
                        {
                            IDPRODUCTO = productosP.IDPRODUCTO,
                            DESCRIPCION = productosP.DESCRIPCION,
                            PRECIO = productosP.PRECIO,
                            STOCK = productosP.STOCK,
                            PRODUCTOR_RUTPRODUCTOR = productosP.PRODUCTOR_RUTPRODUCTOR
                        });
                    }
                    
                }
                return newList;
            }
        }

        public PROCESOVENTA GetProcesoByOrden(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(p => p.ORDENID == orden).FirstOrDefault();
            }
        }

        public decimal? GetProcesoDecimalByOrden(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PROCESOVENTA.Where(p => p.ORDENID == orden).FirstOrDefault().IDPROCESOVENTA;
            }
        }

        public PRODUCTO GetProductToEdit(decimal id)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.PRODUCTO.Where(p => p.IDPRODUCTO == id).FirstOrDefault();
            }
        }

        public bool GetProductosListIfExternoExist(PRODUCTO producto, USUARIO usuario)
        {
            var newProducto = new PRODUCTO();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                newProducto = db.PRODUCTO.Where(p => p.DESCRIPCION == producto.DESCRIPCION && p.TIPOVENTA == producto.TIPOVENTA
                && p.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO && p.IDPROCESOVENTA == null).FirstOrDefault();
                if(newProducto != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public List<TRANSPORTISTA> GetTransporte(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.RUTTRANSPORTISTA == usuario.RUTUSUARIO && t.SUBASTAID == null).ToList();
            }
        }

        public TRANSPORTISTA GetMyTransporteById(decimal idtrans)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.IDTRANSPORTISTA == idtrans).FirstOrDefault();
            }
        }

        public List<SUBASTA> GetSubasta()
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.SUBASTA.ToList();
            }
        }

        public IEnumerable<ProcesoVentaViewModel> GetMySubastasList(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from t in db.TRANSPORTISTA join s in db.SUBASTA
                             on t.SUBASTAID equals s.IDSUBASTA
                             where t.RUTTRANSPORTISTA == usuario.RUTUSUARIO
                             && t.ESTADOSUBASTA != null
                             select new ProcesoVentaViewModel
                             {
                                 IDSUBASTA = s.IDSUBASTA,
                                 ESTADOSUBASTA = t.ESTADOSUBASTA,
                                 FECHASUBASTA = s.FECHA

                             }).ToList();

                return query;
            }
        }

        public TRANSPORTISTA GetTransportistaDetailsBySubasta(decimal subasta, USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.TRANSPORTISTA.Where(t => t.SUBASTAID == subasta && t.RUTTRANSPORTISTA == usuario.RUTUSUARIO).FirstOrDefault();
            }
        }

        public IEnumerable<ProcesoVentaViewModel> GetMyPostulaciones(USUARIO usuario)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from pv in db.PROCESOVENTA
                             join pr in db.PRODUCTO on pv.IDPROCESOVENTA
                             equals pr.IDPROCESOVENTA
                             where pr.PRODUCTOR_RUTPRODUCTOR == usuario.RUTUSUARIO
                             select new ProcesoVentaViewModel
                             {
                                 PROCESO = pr.IDPROCESOVENTA,
                                 ESTADO = pr.ESTADOPROCESO,
                                 FECHA = pv.FECHA,
                                 ORDEN = pv.ORDENID,
                                 TIPOPROCESO = pv.TIPOPROCESO

                             }).Distinct().ToList();

                return query as IEnumerable<ProcesoVentaViewModel>;
            }
        }


        public ProcesoVentaViewModel GetMyPostulacionDetails(decimal? orden)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var lista = new List<PRODUCTO>();
                var query = (from pr in db.PRODUCTO
                             join pv in db.PROCESOVENTA on pr.IDPROCESOVENTA
                             equals pv.IDPROCESOVENTA join or in db.ORDEN
                             on pv.ORDENID equals or.IDORDEN join cl in db.CLIENTE
                             on or.CLIENTE_RUTCLIENTE equals cl.RUTCLIENTE
                             where pr.ORDEN_IDORDEN == orden
                             select new ProcesoVentaViewModel()
                             {
                                 PROCESO = pr.IDPROCESOVENTA,
                                 ESTADO = pr.ESTADOPROCESO,
                                 ORDEN = pr.ORDEN_IDORDEN,
                                 FECHA = pv.FECHA,
                                 NOMBRECLIENTE = cl.NOMBRE,
                                 PAISCLIENTE = cl.PAIS,
                                 TIPOPROCESO = pv.TIPOPROCESO
                                
                             }).FirstOrDefault();

                return query as ProcesoVentaViewModel;
            }
        }

        public decimal? GetProcesoIdBySubastaId(decimal subasta)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                return db.SUBASTA.Where(s => s.IDSUBASTA == subasta).FirstOrDefault().PROCESOVENTAID;
            }
        }

        public ProcesoVentaViewModel GetDatosClientByProcesoVenta(decimal? proceso)
        {
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {
                var query = (from sb in db.SUBASTA join pv in db.PROCESOVENTA
                             on sb.PROCESOVENTAID equals pv.IDPROCESOVENTA
                             join or in db.ORDEN on pv.ORDENID equals or.IDORDEN
                             join cl in db.CLIENTE on or.CLIENTE_RUTCLIENTE equals
                             cl.RUTCLIENTE where pv.IDPROCESOVENTA == proceso
                             select new ProcesoVentaViewModel()
                             {
                                 IDSUBASTA = sb.IDSUBASTA,
                                 NOMBRECLIENTE = cl.NOMBRE,
                                 PAISCLIENTE = cl.PAIS,
                                 CLIENTEFINAL = cl.DIRECCION,
                                 FECHA = pv.FECHA

                             }).FirstOrDefault();

                return query as ProcesoVentaViewModel;
            }
        }
    }
}