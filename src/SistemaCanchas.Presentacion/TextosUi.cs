using System;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    internal static class TextosUi
    {
        internal const string TituloAplicacion = "Sistema de Reserva de Canchas Sintéticas";

        internal static void ConfigurarGrilla(DataGridView grilla)
        {
            if (grilla == null)
            {
                throw new ArgumentNullException(nameof(grilla));
            }

            grilla.AllowUserToResizeColumns = false;
            grilla.AllowUserToResizeRows = false;
            grilla.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            grilla.RowTemplate.Resizable = DataGridViewTriState.False;
            for (int i = 0; i < grilla.Columns.Count; i++)
            {
                grilla.Columns[i].Resizable = DataGridViewTriState.False;
            }
        }

        internal static void QuitarSeleccionGrilla(DataGridView grilla)
        {
            Action quitar = () =>
            {
                grilla.ClearSelection();
                grilla.CurrentCell = null;
            };

            if (grilla.IsHandleCreated)
            {
                grilla.BeginInvoke(quitar);
                return;
            }

            quitar();
        }
    }
}
