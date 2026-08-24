namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Fila de dbo.USUARIOS (A7 / A11).
    /// </summary>
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public string UsuarioLogin { get; set; }

        public string ClaveAppHash { get; set; }

        public string UsuarioBd { get; set; }

        public string ClaveBdEnc { get; set; }

        public int IdRol { get; set; }

        public string EstadoUsuario { get; set; }

        /// <summary>
        /// Nombre del rol obtenido por JOIN en sp_ObtenerCredencialesLogin.
        /// No es columna de USUARIOS; se usa en la sesión de aplicación (RF16 / RN11).
        /// </summary>
        public string NombreRol { get; set; }
    }
}
