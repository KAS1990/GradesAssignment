using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GradesAssignment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            MessageBox.Show($"Непредвиденная ошибка:\n\r{ex.Message}\n\rПрограмма будет закрыта", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            Environment.Exit(0);
        }
    }
}
