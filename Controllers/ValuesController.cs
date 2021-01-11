using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using pfeAPI.Models;

namespace pfeAPI.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {

        // POST api/values -- obtener informacion para validacion de login 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getLogin")]
        public object getLogin([FromBody] pfeUSUARIOS buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _IDUSUARIO = buscar.IDUSUARIO;
                    string _PASSW = buscar.PASSW;

                    var oUSUARIOS = db.pfeUSUARIOS
                        .Where(x => x.IDUSUARIO == _IDUSUARIO)
                        .Select(x => new { x.IDUSUARIO, x.NOMUSUARIO, x.ROL, x.IDEMPRESA, x.ESTATUS, x.PASSW })
                        .FirstOrDefault();

                    if (oUSUARIOS == null)
                        throw new Exception("El usuario no existe.");

                    if (oUSUARIOS.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (_PASSW != oUSUARIOS.PASSW)
                        throw new Exception("La constraseña falló.");

                    return oUSUARIOS;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #region USUARIOS

        // POST api/values -- obtener registros de usuarios. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getUsuariosList")]
        public IEnumerable<object> getUsuariosList([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    int _idempresa = buscar.idempresa;
                    string _valor = buscar.valor != "" ? buscar.valor : "0";

                    var oUSUARIOS = db.pfeUSUARIOS
                        .Where(x => (x.IDEMPRESA == _idempresa || _idempresa == 0) && (x.IDUSUARIO.Contains(_valor) || x.NOMUSUARIO.Contains(_valor) || _valor == "0"))
                        .Select(x => new { x.IDUSUARIO, x.NOMUSUARIO, x.ROL, x.IDEMPRESA, x.ESTATUS, x.PASSW })
                        .ToList();

                    return oUSUARIOS;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- obtener registros de usuarios. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getUsuariosbyId")]
        public object getUsuariosbyId([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    int _idempresa = buscar.idempresa;
                    string _valor = buscar.valor != "" ? buscar.valor : "0";

                    var oUSUARIOS = db.pfeUSUARIOS
                        .Where(x => x.IDEMPRESA == _idempresa && x.IDUSUARIO == _valor)
                        .Select(x => new { x.IDUSUARIO, x.NOMUSUARIO, x.ROL, x.IDEMPRESA, x.ESTATUS, x.PASSW })
                        .SingleOrDefault();

                    return oUSUARIOS;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- ingresar registro de usuario. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insUsuario")]
        public string insUsuario([FromBody] pfeUSUARIOS oUsuario)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Usuario = db.pfeUSUARIOS.Where(x => x.IDEMPRESA == oUsuario.IDEMPRESA && x.IDUSUARIO == oUsuario.IDUSUARIO).SingleOrDefault();

                    if (_Usuario != null)
                        throw new Exception("Usuario ya existe!");

                    db.pfeUSUARIOS.Add(oUsuario);

                    db.SaveChanges();

                    return "Registro ingresado OK.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ACTUALIZAR registros de usuarios. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updUsuario")]
        public string updUsuario([FromBody] pfeUSUARIOS oUsuario)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Usuario = db.pfeUSUARIOS.Where(x => x.IDEMPRESA == oUsuario.IDEMPRESA && x.IDUSUARIO == oUsuario.IDUSUARIO).SingleOrDefault();

                    if (_Usuario == null)
                        throw new Exception("Usuario NO existe!");

                    _Usuario.NOMUSUARIO = oUsuario.NOMUSUARIO;
                    _Usuario.PASSW = oUsuario.PASSW;
                    _Usuario.ROL = oUsuario.ROL;
                    _Usuario.ESTATUS = oUsuario.ESTATUS;

                    db.SaveChanges();

                    return "Registro actualizado OK.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINAR registros de usuarios. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delUsuario")]
        public string delUsuario([FromBody] pfeUSUARIOS oUsuario)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Usuario = db.pfeUSUARIOS.Where(x => x.IDEMPRESA == oUsuario.IDEMPRESA && x.IDUSUARIO == oUsuario.IDUSUARIO).SingleOrDefault();

                    if (_Usuario == null)
                        throw new Exception("Usuario NO existe!");

                    db.pfeUSUARIOS.Remove(_Usuario);

                    db.SaveChanges();

                    return "Registro eliminado OK.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region EMPRESAS

        // POST api/values
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getEmpresasList")]
        public IEnumerable<object> getEmpresasList([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";

                    var oEmpresas = db.pfeEMPRESA
                        .Where(x => x.IDEMPRESA.ToString() == _valor ||
                           x.RAZONSOCIAL.Contains(_valor) ||
                           x.NOMCONTACTO.Contains(_valor) ||
                           x.ESTATUS == _valor ||
                           _valor == "0")
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.RAZONSOCIAL,
                            x.RFC,
                            x.NOMCONTACTO,
                            x.EMAILCONTACTO,
                            x.TELEFONO,
                            x.ESTATUS
                        }).ToList();

                    return oEmpresas;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getEmpresaByID")]
        public object getEmpresaByID([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";

                    var oEmpresaByID = db.pfeEMPRESA
                        .Where(x => x.IDEMPRESA.ToString() == _valor)
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.RAZONSOCIAL,
                            x.RFC,
                            x.NOMCONTACTO,
                            x.EMAILCONTACTO,
                            x.TELEFONO,
                            x.ESTATUS
                        }).FirstOrDefault();

                    return oEmpresaByID;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- agregar empresas
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insEmpersa")]
        public string InsEmpresa([FromBody] pfeEMPRESA oEmpresa)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Empresa = db.pfeEMPRESA.Where(x => x.IDEMPRESA == oEmpresa.IDEMPRESA).SingleOrDefault();

                    if (_Empresa != null)
                        throw new Exception("Empresa ya existe!");

                    db.pfeEMPRESA.Add(oEmpresa);

                    db.SaveChanges();

                    return "Registro ingresado OK.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ACTUALIZAR registros de empresa. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updEmpresa")]
        public string updEmpresa([FromBody] pfeEMPRESA oEmpresa)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Empresa = db.pfeEMPRESA.Where(x => x.IDEMPRESA == oEmpresa.IDEMPRESA)
                        .SingleOrDefault();

                    if (_Empresa == null)
                        throw new Exception("Empresa NO existe!");

                    _Empresa.RAZONSOCIAL = oEmpresa.RAZONSOCIAL;
                    _Empresa.RFC = oEmpresa.RFC;
                    _Empresa.NOMCONTACTO = oEmpresa.NOMCONTACTO;
                    _Empresa.EMAILCONTACTO = oEmpresa.EMAILCONTACTO;
                    _Empresa.TELEFONO = oEmpresa.TELEFONO;
                    _Empresa.ESTATUS = oEmpresa.ESTATUS;

                    db.SaveChanges();

                    return "Registro actualizado OK.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINAR registros de usuarios. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delEmpresa")]
        public string delEmpresa([FromBody] pfeEMPRESA oEmpresa)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Empresa = db.pfeEMPRESA.Where(x => x.IDEMPRESA == oEmpresa.IDEMPRESA)
                        .SingleOrDefault();

                    if (_Empresa == null)
                        throw new Exception("Empresa NO existe!");

                    if (_Empresa.pfeUSUARIOS.Count > 0)
                        throw new Exception("Empresa con registros : USUARIOs !");

                    db.pfeEMPRESA.Remove(_Empresa);

                    db.SaveChanges();

                    return "Registro eliminado OK.";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region FILIALES

        // POST api/values -- CONSULTA DE FILIALES
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getFilialesList")]
        public IEnumerable<object> getFilialesList([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;

                    var oEmpresas = db.pfeFILIALES
                        .Where(x => (x.IDEMPRESA == _idEmpresa || _idEmpresa == 0) &&
                        (x.IDFILIAL.ToString() == _valor ||
                           x.RAZONSOCIAL.Contains(_valor) ||
                           x.RFC.Contains(_valor) ||
                           _valor == "0"))
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.RAZONSOCIAL,
                            x.IDFILIAL,
                            x.TELEFONO,
                            x.RFC,
                            x.EMAILCONTACTO,
                            x.EMAILPAGOS,
                            x.EMAILREVISION,
                            x.IDUSUARIO
                        }).ToList();

                    return oEmpresas;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        // POST api/values -- CONSULTA DE FILIALES
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getFilialById")]
        public object getFilialById([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;

                    var oEmpresas = db.pfeFILIALES
                        .Where(x => x.IDEMPRESA == _idEmpresa && x.IDFILIAL.ToString() == _valor)
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.RAZONSOCIAL,
                            x.IDFILIAL,
                            x.TELEFONO,
                            x.RFC,
                            x.EMAILCONTACTO,
                            x.EMAILPAGOS,
                            x.EMAILREVISION,
                            x.IDUSUARIO
                        }).SingleOrDefault();

                    return oEmpresas;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- INSERTAR FILIAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insFilial")]
        public string insFilial([FromBody] pfeFILIALES oFilial)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Filial = db.pfeFILIALES.Where(x => x.IDEMPRESA == oFilial.IDEMPRESA && x.IDFILIAL == oFilial.IDFILIAL).SingleOrDefault();

                    if (_Filial != null)
                        throw new Exception("Filial ya existe !");

                    db.pfeFILIALES.Add(oFilial);
                    db.SaveChanges();

                    return "registro ingresado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ACTUALIZAR FILIAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updFilial")]
        public string updFilial([FromBody] pfeFILIALES oFilial)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Filial = db.pfeFILIALES.Where(x => x.IDEMPRESA == oFilial.IDEMPRESA && x.IDFILIAL == oFilial.IDFILIAL).SingleOrDefault();

                    if (_Filial == null)
                        throw new Exception("Filial NO existe !");

                    _Filial.RAZONSOCIAL = oFilial.RAZONSOCIAL;
                    _Filial.RFC = oFilial.RFC;
                    _Filial.TELEFONO = oFilial.TELEFONO;
                    _Filial.EMAILCONTACTO = oFilial.EMAILCONTACTO;
                    _Filial.EMAILPAGOS = oFilial.EMAILPAGOS;
                    _Filial.EMAILREVISION = oFilial.EMAILREVISION;
                    _Filial.IDUSUARIO = oFilial.IDUSUARIO;

                    db.SaveChanges();

                    return "registro actualizado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINAR FILIAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delFilial")]
        public string delFilial([FromBody] pfeFILIALES oFilial)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Filial = db.pfeFILIALES.Where(x => x.IDEMPRESA == oFilial.IDEMPRESA && x.IDFILIAL == oFilial.IDFILIAL).SingleOrDefault();

                    if (_Filial == null)
                        throw new Exception("Filial NO existe !");

                    db.pfeFILIALES.Remove(oFilial);
                    db.SaveChanges();

                    return "registro eliminado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region PROVEEDORES

        // POST api/values -- CONSULTA DE PROVEEDORES
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getProveedoresList")]
        public IEnumerable<object> getProveedoresList([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;

                    var oProveedores = db.pfePROVEEDORES
                        .Where(x => x.IDEMPRESA == _idEmpresa &&
                        (x.IDPROVEEDOR == _valor ||
                        x.RAZONSOCIAL.Contains(_valor) ||
                        x.CURP.Contains(_valor) ||
                        x.RFC.Contains(_valor) ||
                           _valor == "0"))
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.IDPROVEEDOR,
                            x.RAZONSOCIAL,
                            x.TIPOPROV,
                            x.CURP,
                            x.RFC,
                            x.TIPOPAGO,
                            x.DIASCREDITO,
                            x.TIPOFACTURA,
                            x.TIPOVENCE,
                            x.CLAVEPROV,
                            x.CLAVEPROD,
                            x.NOMCONTACTO,
                            x.APPCONTACTO,
                            x.APMCONTACTO,
                            x.EMAILPROV,
                            x.TELEFONO,
                            x.ESTATUS,
                            x.IDUSUARIO,
                            x.IDMODIFICA
                        }).ToList();

                    return oProveedores;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- CONSULTA DE PROVEEDOR POR ID
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getProveedorById")]
        public object getProveedorById([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;

                    var oProveedores = db.pfePROVEEDORES
                        .Where(x => x.IDEMPRESA == _idEmpresa && x.IDPROVEEDOR == _valor)
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.IDPROVEEDOR,
                            x.RAZONSOCIAL,
                            x.TIPOPROV,
                            x.CURP,
                            x.RFC,
                            x.TIPOPAGO,
                            x.DIASCREDITO,
                            x.TIPOFACTURA,
                            x.TIPOVENCE,
                            x.CLAVEPROV,
                            x.CLAVEPROD,
                            x.NOMCONTACTO,
                            x.APPCONTACTO,
                            x.APMCONTACTO,
                            x.EMAILPROV,
                            x.TELEFONO,
                            x.ESTATUS,
                            x.IDUSUARIO,
                            x.IDMODIFICA
                        }).SingleOrDefault();

                    return oProveedores;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- CONSULTA DE PROVEEDOR POR USUARIO. 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getProveedorByUser")]
        public object getProveedorByUser([FromBody] pfeUSUARIOS buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _idUsuario = buscar.IDUSUARIO;
                    int _idEmpresa = Int32.Parse(buscar.IDEMPRESA.ToString());

                    var oProveedores = db.pfePROVEEDORES
                        .Where(x => x.IDEMPRESA == _idEmpresa && x.IDUSUARIO == _idUsuario)
                        .Select(x => new
                        {
                            x.IDPROVEEDOR,
                            x.ESTATUS
                        }).SingleOrDefault();

                    return oProveedores;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- INSERTAR PROVEEDOR.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insProveedor")]
        public string insProveedor([FromBody] pfePROVEEDORES oProveedor)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Filial = db.pfePROVEEDORES.Where(x => x.IDEMPRESA == oProveedor.IDEMPRESA && x.IDPROVEEDOR == oProveedor.IDPROVEEDOR).SingleOrDefault();

                    if (_Filial != null)
                        throw new Exception("Proveedor ya existe !");

                    db.pfePROVEEDORES.Add(oProveedor);
                    db.SaveChanges();

                    return "registro ingresado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ACTUALIZAR FILIAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updProveedor")]
        public string updProveedor([FromBody] pfePROVEEDORES oProveedor)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Proveedor = db.pfePROVEEDORES.Where(x => x.IDEMPRESA == oProveedor.IDEMPRESA && x.IDPROVEEDOR == oProveedor.IDPROVEEDOR).SingleOrDefault();

                    if (_Proveedor == null)
                        throw new Exception("Proveedor NO existe !");

                    _Proveedor.IDEMPRESA = oProveedor.IDEMPRESA;
                    _Proveedor.IDPROVEEDOR = oProveedor.IDPROVEEDOR;
                    _Proveedor.RAZONSOCIAL = oProveedor.RAZONSOCIAL;
                    _Proveedor.TIPOPROV = oProveedor.TIPOPROV;
                    _Proveedor.CURP = oProveedor.CURP;
                    _Proveedor.RFC = oProveedor.RFC;
                    _Proveedor.TIPOPAGO = oProveedor.TIPOPAGO;
                    _Proveedor.DIASCREDITO = oProveedor.DIASCREDITO;
                    _Proveedor.TIPOFACTURA = oProveedor.TIPOFACTURA;
                    _Proveedor.TIPOVENCE = oProveedor.TIPOVENCE;
                    _Proveedor.CLAVEPROV = oProveedor.CLAVEPROV;
                    _Proveedor.CLAVEPROD = oProveedor.CLAVEPROD;
                    _Proveedor.NOMCONTACTO = oProveedor.NOMCONTACTO;
                    _Proveedor.APPCONTACTO = oProveedor.APPCONTACTO;
                    _Proveedor.APMCONTACTO = oProveedor.APMCONTACTO;
                    _Proveedor.EMAILPROV = oProveedor.EMAILPROV;
                    _Proveedor.TELEFONO = oProveedor.TELEFONO;
                    _Proveedor.ESTATUS = oProveedor.ESTATUS;
                    _Proveedor.IDUSUARIO = oProveedor.IDUSUARIO;
                    _Proveedor.IDMODIFICA = oProveedor.IDMODIFICA;

                    db.SaveChanges();

                    return "registro actualizado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINAR FILIAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delProveedor")]
        public string delProveedor([FromBody] pfePROVEEDORES oProveedor)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Proveedor = db.pfePROVEEDORES.Where(x => x.IDEMPRESA == oProveedor.IDEMPRESA && x.IDPROVEEDOR == oProveedor.IDPROVEEDOR).SingleOrDefault();

                    if (_Proveedor == null)
                        throw new Exception("Proveedor NO existe !");

                    db.pfePROVEEDORES.Remove(_Proveedor);
                    db.SaveChanges();

                    return "registro eliminado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region ORDENES DE COMPRA

        // POST api/values -- CONSULTA DE ORDENES DE COMPRA
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getOrdenesCompraList")]
        public IEnumerable<object> getOrdenesCompraList([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;

                    var oOrdenes = db.pfeORDENCOMPRA
                        .Where(x => x.IDEMPRESA == _idEmpresa &&
                        (x.pfePROVEEDORES.RAZONSOCIAL.Contains(_valor) ||
                        x.pfeFILIALES.RAZONSOCIAL.Contains(_valor) ||
                        x.IDFOLIOOC.Contains(_valor) ||
                        x.OBRA.Contains(_valor) ||
                           _valor == "0"))
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.IDPROVEEDOR,
                            x.pfePROVEEDORES.RAZONSOCIAL,
                            x.IDFILIAL,
                            FILIALNOMBRE = x.pfeFILIALES.RAZONSOCIAL,
                            x.IDFOLIOOC,
                            x.FECHAOC,
                            x.OBRA,
                            x.COSTO,
                            x.MONEDA,
                            x.ESTATUS,
                            x.COMENTARIO,
                            x.IDUSUARIO
                        }).ToList();

                    return oOrdenes;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- CONSULTA DE ORDENES DE COMPRA por ID
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getOrdenCompraById")]
        public object getOrdenCompraById([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;
                    int _idFilial = buscar.idfilial;
                    string _idProveedor = buscar.idproveedor;
                    string _folioOc = buscar.foliooc;

                    var oOrdenes = db.pfeORDENCOMPRA
                        .Where(x => x.IDEMPRESA == _idEmpresa &&
                        x.IDFILIAL == _idFilial &&
                        x.IDPROVEEDOR == _idProveedor &&
                        x.IDFOLIOOC == _folioOc)
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.IDPROVEEDOR,
                            x.IDFILIAL,
                            x.IDFOLIOOC,
                            x.FECHAOC,
                            x.OBRA,
                            x.COSTO,
                            x.MONEDA,
                            x.ESTATUS,
                            x.COMENTARIO,
                            x.IDUSUARIO
                        }).SingleOrDefault();

                    if (oOrdenes == null)
                        throw new Exception("Orden Compra no existe.");

                    return oOrdenes;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- CONSULTA DE ORDENES DE COMPRA CON FILTROS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getOCsByFilters")]
        public IEnumerable<object> getOCsByFilters([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;
                    int _idFilial = buscar.idfilial;
                    string _idproveedor = buscar.idproveedor;
                    string _estatus = buscar.estatus;

                    var oOrdenes = db.pfeORDENCOMPRA
                        .Where(x => x.IDEMPRESA == _idEmpresa &&
                        ((x.IDFILIAL == _idFilial || _idFilial == 0) ||
                        (x.IDPROVEEDOR == _idproveedor || _idproveedor == "0") ||
                        (x.ESTATUS == _estatus || _estatus == "0")))
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.IDPROVEEDOR,
                            x.pfePROVEEDORES.RAZONSOCIAL,
                            x.IDFILIAL,
                            FILIALNOMBRE = x.pfeFILIALES.RAZONSOCIAL,
                            x.IDFOLIOOC,
                            x.FECHAOC,
                            x.OBRA,
                            x.COSTO,
                            x.MONEDA,
                            x.ESTATUS,
                            x.COMENTARIO,
                            x.IDUSUARIO
                        }).ToList();

                    return oOrdenes;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- INSERTAR ORDEN DE COMPRA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insOrden")]
        public string insOrden([FromBody] pfeORDENCOMPRA oOrden)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Orden = db.pfeORDENCOMPRA.Where(x => x.IDEMPRESA == oOrden.IDEMPRESA &&
                    x.IDFILIAL == oOrden.IDFILIAL &&
                    x.IDPROVEEDOR == oOrden.IDPROVEEDOR &&
                    x.IDFOLIOOC == oOrden.IDFOLIOOC)
                       .SingleOrDefault();

                    if (_Orden != null)
                        throw new Exception("Orden ya existe !");

                    db.pfeORDENCOMPRA.Add(oOrden);
                    db.SaveChanges();

                    return "registro ingresado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ACTUALIZAR ORDEN COMPRA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updOrden")]
        public string updOrden([FromBody] pfeORDENCOMPRA oOrden)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Orden = db.pfeORDENCOMPRA.Where(x => x.IDEMPRESA == oOrden.IDEMPRESA &&
                    x.IDFILIAL == oOrden.IDFILIAL &&
                    x.IDPROVEEDOR == oOrden.IDPROVEEDOR &&
                    x.IDFOLIOOC == oOrden.IDFOLIOOC)
                       .SingleOrDefault();

                    if (_Orden == null)
                        throw new Exception("Orden NO existe !");

                    _Orden.IDEMPRESA = oOrden.IDEMPRESA;
                    _Orden.IDFILIAL = oOrden.IDFILIAL;
                    _Orden.IDPROVEEDOR = oOrden.IDPROVEEDOR;
                    _Orden.IDFOLIOOC = oOrden.IDFOLIOOC;
                    _Orden.FECHAOC = oOrden.FECHAOC;
                    _Orden.OBRA = oOrden.OBRA;
                    _Orden.COSTO = oOrden.COSTO;
                    _Orden.MONEDA = oOrden.MONEDA;
                    _Orden.ESTATUS = oOrden.ESTATUS;
                    _Orden.COMENTARIO = oOrden.COMENTARIO;
                    _Orden.IDUSUARIO = oOrden.IDUSUARIO;

                    db.SaveChanges();

                    return "registro actualizado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINAR Orden.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delOrden")]
        public string delOrden([FromBody] pfeORDENCOMPRA oOrden)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _Orden = db.pfeORDENCOMPRA.Where(x => x.IDEMPRESA == oOrden.IDEMPRESA &&
                    x.IDFILIAL == oOrden.IDFILIAL &&
                    x.IDPROVEEDOR == oOrden.IDPROVEEDOR &&
                    x.IDFOLIOOC == oOrden.IDFOLIOOC)
                       .SingleOrDefault();

                    if (_Orden == null)
                        throw new Exception("Orden NO existe !");

                    db.pfeORDENCOMPRA.Remove(_Orden);
                    db.SaveChanges();

                    return "registro eliminado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region FACTURAS

        // POST api/values -- CONSULTA FACTURAS RELACIONADAS A ORDEN DE COMPRA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getFacturasByOC")]
        public IEnumerable<object> getFacturasByOC([FromBody] param buscar)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = buscar.valor != "" ? buscar.valor : "0";
                    int _idEmpresa = buscar.idempresa;
                    int _idFilial = buscar.idfilial;
                    string _idproveedor = buscar.idproveedor;
                    string _estatus = buscar.estatus;
                    string _folioOc = buscar.foliooc;

                    var oFacturas = db.pfeFACTURAS.Where(x => x.IDEMPRESA == _idEmpresa &&
                        (x.IDFILIAL == _idFilial || _idFilial == 0) &&
                        (x.IDPROVEEDOR == _idproveedor || _idproveedor == "0") ||
                        (x.IDFOLIOOC == _folioOc || _folioOc == "0"))
                        .Select(x => new
                        {
                            x.IDEMPRESA,
                            x.IDFACTURA,
                            x.IDFILIAL,
                            x.IDPROVEEDOR,
                            x.IDFOLIOOC,
                            x.SERIE,
                            x.FOLIO,
                            x.FECHA,
                            x.TOTAL,
                            x.MONEDA,
                            x.UUID,
                            x.ESTATUS,
                            x.COMENTARIO,
                            x.XMLFILENAME,
                            x.PDFFILENAME,
                            x.IMGFILENAME,
                            x.TIPOCOMPROBANTE,
                            x.FECHAINGRESOXML,
                            x.FECHAINGRESOPDF,
                            x.FECHAINGRESOIMG,
                            filialNom = x.pfeFILIALES.RAZONSOCIAL,
                            x.XMLOBJ,
                            x.XMLTXT,
                            x.PDFOBJ,
                            x.IMGOBJ
                        }).ToList();

                    return oFacturas;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        // POST api/values -- INSERTAR FACTURA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insFactura")]
        public string insFactura([FromBody] pfeFACTURAS oFacturas)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _factura = db.pfeFACTURAS.Where(x => x.IDEMPRESA == oFacturas.IDEMPRESA &&
                    x.UUID == oFacturas.UUID)
                       .SingleOrDefault();

                    if (_factura != null)
                        throw new Exception("Factura ya existe !");

                    db.pfeFACTURAS.Add(oFacturas);
                    db.SaveChanges();

                    return "registro ingresado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ACTUALIZAR FACTURA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updFactura")]
        public string updFactura([FromBody] pfeFACTURAS oFactura)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _factura = db.pfeFACTURAS.Where(x => x.IDEMPRESA == oFactura.IDEMPRESA &&
                    x.UUID == oFactura.UUID)
                       .SingleOrDefault();

                    if (_factura == null)
                        throw new Exception("Factura NO existe !");

                    _factura.IDEMPRESA = oFactura.IDEMPRESA;
                    _factura.IDFACTURA = oFactura.IDFACTURA;
                    _factura.IDFILIAL = oFactura.IDFILIAL;
                    _factura.IDPROVEEDOR = oFactura.IDPROVEEDOR;
                    _factura.IDFOLIOOC = oFactura.IDFOLIOOC;
                    _factura.XMLTXT = oFactura.XMLTXT;
                    _factura.XMLOBJ = oFactura.XMLOBJ;
                    _factura.XMLFILENAME = oFactura.XMLFILENAME;
                    _factura.PDFOBJ = oFactura.PDFOBJ;
                    _factura.PDFFILENAME = oFactura.PDFFILENAME;
                    _factura.SERIE = oFactura.SERIE;
                    _factura.FOLIO = oFactura.FOLIO;
                    _factura.FECHA = oFactura.FECHA;
                    _factura.TOTAL = oFactura.TOTAL;
                    _factura.MONEDA = oFactura.MONEDA;
                    _factura.UUID = oFactura.UUID;
                    _factura.ESTATUS = oFactura.ESTATUS;
                    _factura.COMENTARIO = oFactura.COMENTARIO;
                    _factura.IMGOBJ = oFactura.IMGOBJ;
                    _factura.IMGFILENAME = oFactura.IMGFILENAME;
                    _factura.TIPOCOMPROBANTE = oFactura.TIPOCOMPROBANTE;
                    _factura.FECHAINGRESOXML = oFactura.FECHAINGRESOXML;
                    _factura.FECHAINGRESOPDF = oFactura.FECHAINGRESOPDF;
                    _factura.FECHAINGRESOIMG = oFactura.FECHAINGRESOIMG;
                    _factura.FECHAESTATUS = oFactura.FECHAESTATUS;

                    db.SaveChanges();

                    return "registro actualizado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINAR Orden.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delFactura")]
        public string delFactura([FromBody] pfeFACTURAS oFactura)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    var _factura = db.pfeFACTURAS.Where(x => x.IDEMPRESA == oFactura.IDEMPRESA &&
                    x.UUID == oFactura.UUID)
                       .SingleOrDefault();

                    if (_factura == null)
                        throw new Exception("Factura NO existe !");

                    db.pfeFACTURAS.Remove(_factura);
                    db.SaveChanges();

                    return "registro eliminado OK";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}
