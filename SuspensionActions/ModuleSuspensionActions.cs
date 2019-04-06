using KSP.Localization;
using ModuleWheels;

namespace SupsensionActions
{
    public class ModuleSuspensionActions : PartModule
    {
        protected ModuleWheelSuspension _suspensionModule;
        protected bool _ready = false;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            _suspensionModule = part.FindModuleImplementing<ModuleWheelSuspension>();
            if (_suspensionModule == null)
            {
                part.RemoveModule(this);
                Destroy(this);
            }
            else
            {
                _ready = true;
            }
        }

        [KSPAction("#SSC_SuspensionActions_000001",
            advancedTweakable = true, requireFullControl = false)]
        public void ToggleSpringDamper(KSPActionParam param)
        {
            if (!_ready) return;

            _suspensionModule.autoSpringDamper = !_suspensionModule.autoSpringDamper;
            UpdateSuspensionModuleUI(_suspensionModule);
        }

        protected void UpdateSuspensionModuleUI(ModuleWheelSuspension module)
        {
            BaseEvent toggleEvent = module.Events["EvtAutoSpringDamperToggle"];
            BaseField springField = module.Fields["springTweakable"];
            BaseField damperField = module.Fields["damperTweakable"];

            toggleEvent.guiActive = true;
            toggleEvent.guiActiveEditor = true;
            toggleEvent.guiName = Localizer.Format("#autoLOC_8002214", new object[]
            {
                module.autoSpringDamper ? 1 : 0
            });

            bool active = !module.autoSpringDamper || !GameSettings.WHEEL_AUTO_SPRINGDAMPER;

            springField.guiActive = active;
            springField.guiActiveEditor = active;

            damperField.guiActive = active;
            damperField.guiActiveEditor = active;
        }
    }
}
