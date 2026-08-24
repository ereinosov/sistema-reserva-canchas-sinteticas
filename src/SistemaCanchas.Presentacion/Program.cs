using System;
using System.Threading;
using System.Windows.Forms;

namespace SistemaCanchas.Presentacion
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new FrmLogin());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MostrarErrorNoFatal(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception excepcion = e.ExceptionObject as Exception;
            if (excepcion != null)
            {
                MostrarErrorNoFatal(excepcion);
            }
        }

        private static void MostrarErrorNoFatal(Exception excepcion)
        {
            MessageBox.Show(
                "Ha ocurrido un error inesperado. La aplicación seguirá en ejecución." +
                Environment.NewLine + Environment.NewLine +
                "Detalle técnico: " + excepcion.GetType().Name,
                TextosUi.TituloAplicacion,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
