using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// ------------------------------------
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
// ------------------------------------

namespace apiRESTCheckUsuarioTADS.Models
{
    public class clsUsuario
    {
        // Definición de atributos
        public string cve { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string ruta { get; set; }
        public string tipo { get; set; }

        // Definición de cadena de Conexión
        private string cadConn = ConfigurationManager.
                    ConnectionStrings["bdControlAcceso"].
                    ConnectionString;

        // Definición de Constructores del Modelo
        public clsUsuario()
        {
            // Código de inicialización posterior ...        
        }
        public clsUsuario(string usuario,
                          string contrasena)
        {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }
        public clsUsuario(string nombre,
                          string apellidoPaterno,
                          string apellidoMaterno,
                          string usuario,
                          string contrasena,
                          string ruta,
                          string tipo)
        {
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.ruta = ruta;
            this.tipo = tipo;
        }

        // Definición de Métodos de Proceso

        // ✅ YA EXISTÍA - spinsusuario
        public DataSet spInsUsuario()
        {
            // Creación del comando SQL
            string cadSql = "CALL spInsUsuario('" + this.nombre + "', '"
                                                  + this.apellidoPaterno + "','"
                                                  + this.apellidoMaterno + "', '"
                                                  + this.usuario + "', '"
                                                  + this.contrasena + "', '"
                                                  + this.ruta + "', "
                                                  + this.tipo + ");";
            // Configuración de los objetosd de conexión a MySQL
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();
            // Ejecución del Adaptadora de Datos
            da.Fill(ds, "spInsUsuario");
            return ds;
        }

        // ✅ YA EXISTÍA - spvalidaracceso
        public DataSet spValidarAcceso()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "call spValidarAcceso('" + this.usuario + "','"
                                              + this.contrasena + "');";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "spValidarAcceso");
            return ds;
        }

        // Proceso de Reporte de usuarios con filtro
        public DataSet vwRptUsuarioFiltro(string filtro)
        {
            string cadSQL;

            if (string.IsNullOrEmpty(filtro))
            {
                // Si no hay filtro, trae todo
                cadSQL = "SELECT * FROM vwRptUsuario;";
            }
            else
            {
                // Busca coincidencias parciales en Nombre, Usuario o Rol
                cadSQL = "SELECT * FROM vwRptUsuario " +
                         "WHERE Nombre LIKE '%" + filtro + "%' " +
                         "OR Usuario LIKE '%" + filtro + "%' " +
                         "OR Rol LIKE '%" + filtro + "%';";
            }

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "vwRptUsuarioFiltro");
            return ds;
        }

        // 🆕 NUEVO - vwtipousuario
        public DataSet vwTipoUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "SELECT * FROM vwTipoUsuario";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwTipoUsuario");
            return ds;
        }

        // 🆕 NUEVO - spupdusuario 
        public DataSet spUpdUsuario()
        {
            // Creación del comando SQL - NOTA: 'clave' es INT en el SP
            string cadSql = "CALL spUpdUsuario(" + this.cve + ", '"  // clave como INT
                                                  + this.nombre + "', '"
                                                  + this.apellidoPaterno + "','"
                                                  + this.apellidoMaterno + "', '"
                                                  + this.usuario + "', '"
                                                  + this.contrasena + "', '"
                                                  + this.ruta + "', "
                                                  + this.tipo + ");";  // tipo como INT

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "spUpdUsuario");
            return ds;
        }

        // 🆕 NUEVO - spdelusuario (INT)
        public DataSet spDelUsuario(string cveUsuario)
        {
            // Creación del comando SQL - 'clave' es INT en el SP
            string cadSql = "CALL spDelUsuario(" + cveUsuario + ");";  // Sin comillas porque es INT

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "spDelUsuario");
            return ds;
        }
    }
}