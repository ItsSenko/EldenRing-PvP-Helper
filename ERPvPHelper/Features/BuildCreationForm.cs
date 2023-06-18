using System.Collections;
using System.Diagnostics;
using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using Erd_Tools.Models.Items;
using PropertyHook;
using SoulsFormats;
using static Erd_Tools.Models.Param;
using System.Runtime.InteropServices;
using System.Text;

namespace ERPvPHelper
{
    public partial class BuildCreationForm : Form
    {
        private ErdHook hook { get; set; }
        private Player player { get; set; }
        public BuildCreationForm(ErdHook hook, Player player)
        {
            InitializeComponent();

            this.hook = hook;
            this.player = player;
        }

        
    }
}
