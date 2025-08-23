namespace Antura.Discover {

    using Yarn.Unity;

    [System.CodeDom.Compiler.GeneratedCode("YarnSpinner", "3.0.3.0")]
    public partial class YarnVariables : Yarn.Unity.InMemoryVariableStorage, Yarn.Unity.IGeneratedVariableStorage {
        // Accessor for Bool $met_teacher
        public bool MetTeacher {
            get => this.GetValueOrDefault<bool>("$met_teacher");
            set => this.SetValue<bool>("$met_teacher", value);
        }

        // Accessor for Bool $got_hint
        public bool GotHint {
            get => this.GetValueOrDefault<bool>("$got_hint");
            set => this.SetValue<bool>("$got_hint", value);
        }

        // Accessor for Bool $doorUnlocked
        public bool DoorUnlocked {
            get => this.GetValueOrDefault<bool>("$doorUnlocked");
            set => this.SetValue<bool>("$doorUnlocked", value);
        }

        // Accessor for Number $coins
        public float Coins {
            get => this.GetValueOrDefault<float>("$coins");
            set => this.SetValue<float>("$coins", value);
        }

        // Accessor for Bool $EASY_MODE
        public bool EASYMODE {
            get => this.GetValueOrDefault<bool>("$EASY_MODE");
            set => this.SetValue<bool>("$EASY_MODE", value);
        }

        // Accessor for Bool $IS_DESKTOP
        public bool ISDESKTOP {
            get => this.GetValueOrDefault<bool>("$IS_DESKTOP");
            set => this.SetValue<bool>("$IS_DESKTOP", value);
        }

        // Accessor for Number $TOTAL_COINS
        public float TOTALCOINS {
            get => this.GetValueOrDefault<float>("$TOTAL_COINS");
            set => this.SetValue<float>("$TOTAL_COINS", value);
        }

        // Accessor for Number $COLLECTED_ITEMS
        public float COLLECTEDITEMS {
            get => this.GetValueOrDefault<float>("$COLLECTED_ITEMS");
            set => this.SetValue<float>("$COLLECTED_ITEMS", value);
        }

        // Accessor for Number $MAX_PROGRESS
        public float MAXPROGRESS {
            get => this.GetValueOrDefault<float>("$MAX_PROGRESS");
            set => this.SetValue<float>("$MAX_PROGRESS", value);
        }

        // Accessor for Number $CURRENT_PROGRESS
        public float CURRENTPROGRESS {
            get => this.GetValueOrDefault<float>("$CURRENT_PROGRESS");
            set => this.SetValue<float>("$CURRENT_PROGRESS", value);
        }

        // Accessor for String $CURRENT_ITEM
        public string CURRENTITEM {
            get => this.GetValueOrDefault<string>("$CURRENT_ITEM");
            set => this.SetValue<string>("$CURRENT_ITEM", value);
        }

    }
}
