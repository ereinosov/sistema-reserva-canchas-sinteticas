using System;
using System.Collections.Generic;

namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Resultado de sp_ConsultarIngresos (RF12 / RN09): total y detalle de pagos pagados.
    /// </summary>
    public class ConsultaIngresos
    {
        public ConsultaIngresos()
        {
            Detalle = new List<Ingreso>();
        }

        public decimal TotalIngresos { get; set; }

        public IList<Ingreso> Detalle { get; set; }
    }
}
