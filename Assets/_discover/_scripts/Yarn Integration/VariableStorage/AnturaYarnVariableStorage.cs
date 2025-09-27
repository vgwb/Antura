using Yarn.Unity;

namespace Antura.Discover
{
    public partial class AnturaYarnVariables : InMemoryVariableStorage, IGeneratedVariableStorage
    {
        public bool IS_DESKTOP
        {
            get => this.GetValueOrDefault<bool>("$IS_DESKTOP");
            set => this.SetValue<bool>("$IS_DESKTOP", value);
        }

        public bool EASY_MODE
        {
            get => this.GetValueOrDefault<bool>("$EASY_MODE");
            set => this.SetValue<bool>("$EASY_MODE", value);
        }

        public int TOTAL_COINS
        {
            get => this.GetValueOrDefault<int>("$TOTAL_COINS");
            set => this.SetValue<int>("$TOTAL_COINS", value);
        }

        public int COLLECTED_ITEMS
        {
            get => this.GetValueOrDefault<int>("$COLLECTED_ITEMS");
            set => this.SetValue<int>("$COLLECTED_ITEMS", value);
        }

        public float MAX_PROGRESS
        {
            get => this.GetValueOrDefault<float>("$MAX_PROGRESS");
            set => this.SetValue<float>("$MAX_PROGRESS", value);
        }

        public string CURRENT_ITEM
        {
            get => this.GetValueOrDefault<string>("$CURRENT_ITEM");
            set => this.SetValue<string>("$CURRENT_ITEM", value);
        }
    }
}
