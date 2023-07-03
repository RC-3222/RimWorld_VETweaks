using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace VEFTweaks.PatchOperations
{
    public class PatchOperationSetting : PatchOperation
    {
        public string setting;

        public PatchOperation enabled;

        public PatchOperation disabled;

        private bool IsSettingEnabled => VEFTweaks_Mod.Settings.GetEnabledSettings.Contains(setting);

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (IsSettingEnabled && enabled is { })
            {
                return enabled.Apply(xml);
            } else if (disabled is { }) {
                return disabled.Apply(xml);
            }

            return true;
        }
    }
}
