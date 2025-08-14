namespace Antura.Discover {

    using Yarn.Unity;

    [System.CodeDom.Compiler.GeneratedCode("YarnSpinner", "3.0.3.0")]
    public partial class YarnVariables : Yarn.Unity.InMemoryVariableStorage, Yarn.Unity.IGeneratedVariableStorage {
        // Accessor for Bool $doorUnlocked
        public bool DoorUnlocked {
            get => this.GetValueOrDefault<bool>("$doorUnlocked");
            set => this.SetValue<bool>("$doorUnlocked", value);
        }

    }
}
