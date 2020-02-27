namespace REMM {
    public static class Static
    {
        public static G G { get; private set; }

        public static bool Init()
        {
            G = new G();
            G.RimWorld.Refresh();
            return G.RimWorld != null;
        }
    }
}
