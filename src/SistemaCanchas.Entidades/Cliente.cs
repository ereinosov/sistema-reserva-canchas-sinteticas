namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Fila de dbo.CLIENTES (A7 / A11).
    /// </summary>
    public class Cliente
    {
        public int IdCliente { get; set; }

        public string NombreCliente { get; set; }

        public string TipoDocumentoCliente { get; set; }

        public string NumeroDocumentoCliente { get; set; }

        public string TelefonoCliente { get; set; }

        public string CorreoCliente { get; set; }
    }
}
