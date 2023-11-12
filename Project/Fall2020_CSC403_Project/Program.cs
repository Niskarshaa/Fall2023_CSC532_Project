using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project {
  static class Program {
    public static FrmHome frmHome;
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    ///
    [STAThread]
    static void Main() {
      
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      frmHome = new FrmHome();
            Application.Run(frmHome);
            //Application.Run(new ProjectRedPill.TheAwakening());
            //Application.Run(new ProjectRedPill.TrainingCenter());
      
    }
  }
}

/* Reference Code Locations
 * 1. (menu->storyWindow>game)c# windows form overlay and with button click: https://foxlearn.com/windows-forms/how-to-create-overlay-modal-popup-in-csharp-514.html
 * 
 * 
 */
