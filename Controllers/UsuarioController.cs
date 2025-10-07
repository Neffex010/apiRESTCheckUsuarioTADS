using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apiRESTCheckUsuarioTADS.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace apiRESTCheckUsuarioTADS.Controllers
{
    public class UsuarioController : ApiController
    {
        // ✅ ENDPOINT 1: spinsusuario
        [HttpPost]
        [Route("tads/usuario/spinsusuario")]
        public clsApiStatus spInsUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario(
                    modelo.nombre,
                    modelo.apellidoPaterno,
                    modelo.apellidoMaterno,
                    modelo.usuario,
                    modelo.contrasena,
                    modelo.ruta,
                    modelo.tipo
                );
                DataSet ds = new DataSet();

                ds = objUsuario.spInsUsuario();
                objRespuesta.statusExec = true;
                objRespuesta.msg = "Ejecución exitosa";
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                jsonResp.Add("msgData", "Ejecución exitosa del proceso spinsusuario");
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Ejecución fallida";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
            }
            objRespuesta.datos = jsonResp;
            return objRespuesta;
        }

        // ✅ ENDPOINT 2: spvalidaracceso 
        [HttpPost]
        [Route("tads/usuario/spvalidaracceso")]
        public clsApiStatus spValidarAcceso([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario(
                    modelo.usuario,
                    modelo.contrasena
                );

                DataSet ds = new DataSet();
                ds = objUsuario.spValidarAcceso();

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario validado exitosamente";
                    jsonResp.Add("usu_nombre_completo", ds.Tables[0].Rows[0][1].ToString());
                    jsonResp.Add("usu_ruta", ds.Tables[0].Rows[0][2].ToString());
                    jsonResp.Add("usu_usuario", ds.Tables[0].Rows[0][3].ToString());
                    jsonResp.Add("tip_descripcion", ds.Tables[0].Rows[0][4].ToString());
                }
                else
                {
                    objRespuesta.msg = "Acceso no autorizado";
                    jsonResp.Add("msgData", "Usuario no encontrado");
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Ejecución fallida";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
            }
            objRespuesta.datos = jsonResp;
            return objRespuesta;
        }

        // Endpoint con filtro único (nombre, apPaterno, apMaterno o usuario)
        [HttpGet]
        [Route("tads/usuario/vwrptusuariofiltro")]
        public clsApiStatus vwRptUsuarioFiltro(string filtro = "")
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                DataSet ds = objUsuario.vwRptUsuarioFiltro(filtro);

                objRespuesta.statusExec = true;
                objRespuesta.msg = "Consulta filtrada ejecutada exitosamente";
                objRespuesta.ban = ds.Tables[0].Rows.Count;

                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Error en el proceso vwRptUsuarioFiltro";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
            }

            objRespuesta.datos = jsonResp;
            return objRespuesta;
        }

        // 🆕 ENDPOINT 4: vwtipousuario (NUEVO)
        [HttpGet]
        [Route("tads/usuario/vwtipousuario")]
        public clsApiStatus vwTipoUsuario()
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                DataSet ds = objUsuario.vwTipoUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.msg = "Tipos de usuario consultados exitosamente";
                objRespuesta.ban = ds.Tables[0].Rows.Count;

                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"tiposUsuario\": {jsonString}}}");
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Ejecución fallida";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
            }
            objRespuesta.datos = jsonResp;
            return objRespuesta;
        }

        // 🆕 ENDPOINT 5: spupdusuario (NUEVP)
        [HttpPut]
        [Route("tads/usuario/spupdusuario")]
        public clsApiStatus spUpdUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario(
                    modelo.nombre,
                    modelo.apellidoPaterno,
                    modelo.apellidoMaterno,
                    modelo.usuario,
                    modelo.contrasena,
                    modelo.ruta,
                    modelo.tipo
                );
                objUsuario.cve = modelo.cve;

                DataSet ds = objUsuario.spUpdUsuario();
                int codigoRetorno = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                objRespuesta.statusExec = true;
                objRespuesta.ban = codigoRetorno;

                // Manejo de códigos según el SP
                switch (codigoRetorno)
                {
                    case 0:
                        objRespuesta.msg = "Usuario actualizado exitosamente";
                        jsonResp.Add("msgData", "Actualización completada");
                        break;
                    case 1:
                        objRespuesta.msg = "Usuario no encontrado";
                        jsonResp.Add("msgData", "La clave de usuario no existe");
                        break;
                    case 2:
                        objRespuesta.msg = "Usuario duplicado";
                        jsonResp.Add("msgData", "Ya existe un usuario con ese nombre completo");
                        break;
                    case 3:
                        objRespuesta.msg = "Usuario duplicado";
                        jsonResp.Add("msgData", "Ya existe un usuario con ese nombre de usuario");
                        break;
                    case 4:
                        objRespuesta.msg = "Tipo de usuario inválido";
                        jsonResp.Add("msgData", "El tipo de usuario no existe");
                        break;
                    default:
                        objRespuesta.msg = "Código de retorno desconocido";
                        jsonResp.Add("msgData", $"Código: {codigoRetorno}");
                        break;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Ejecución fallida";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
            }

            objRespuesta.datos = jsonResp;
            return objRespuesta;
        }

        // 🆕 ENDPOINT 6: spdelusuario (NUEVO)
        [HttpDelete]
        [Route("tads/usuario/spdelusuario/{cve}")]
        public clsApiStatus spDelUsuario(string cve)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                DataSet ds = objUsuario.spDelUsuario(cve);
                int codigoRetorno = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                objRespuesta.statusExec = true;
                objRespuesta.ban = codigoRetorno;

                // Manejo de códigos según el SP
                if (codigoRetorno == 0)
                {
                    objRespuesta.msg = "Usuario eliminado exitosamente";
                    jsonResp.Add("msgData", "Eliminación completada");
                }
                else if (codigoRetorno == 1)
                {
                    objRespuesta.msg = "Usuario no encontrado";
                    jsonResp.Add("msgData", "La clave de usuario no existe");
                }
                else
                {
                    objRespuesta.msg = "Código de retorno desconocido";
                    jsonResp.Add("msgData", $"Código: {codigoRetorno}");
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Ejecución fallida";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
            }

            objRespuesta.datos = jsonResp;
            return objRespuesta;
        }
    }
}