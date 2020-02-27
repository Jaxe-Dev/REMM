using REMM.Common;
using REMM.Manager;
using REMM.RW;

namespace REMM
{
    public class G : ViewModel
    {
        public Settings Settings { get; }
        private RimWorld _rimWorld;
        public RimWorld RimWorld { get => _rimWorld; set => SetField(ref _rimWorld, value); }

        public G()
        {
            Settings = new Settings(App.Directory.GetFile(App.Id + ".xml"));
            RimWorld = Settings.GameDirectory == null ? RimWorld.Init() : RimWorld.Init(Settings.GameDirectory);
        }
    }
}
