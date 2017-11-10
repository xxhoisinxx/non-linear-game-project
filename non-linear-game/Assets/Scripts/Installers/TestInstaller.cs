using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Installers {
    using System.Diagnostics;

    using log4net;

    using Zenject;

    public class TestInstaller : MonoInstaller {
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void InstallBindings() {
        }
    }
}
