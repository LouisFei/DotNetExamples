using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace ThreadingTaskExample
{
    [RunInstaller(true)]
    public partial class EventLogInstaller : System.Configuration.Install.Installer
    {
        static void Main()
        {
            new EventLogInstaller();
            Console.WriteLine("hello EventLogInstaller");
        }

        public EventLogInstaller()
        {
            InitializeComponent();

            System.Diagnostics.EventLogInstaller eventLogInstaller = new System.Diagnostics.EventLogInstaller();
            eventLogInstaller.Source = "MemberTest";
            eventLogInstaller.Log = "MemberTest";

            this.Installers.Add(eventLogInstaller);
            //this.Installers.Remove(eventLogInstaller);
        }
    }
}
