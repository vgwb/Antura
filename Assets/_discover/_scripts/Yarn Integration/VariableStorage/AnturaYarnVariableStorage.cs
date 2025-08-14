using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{
    public partial class AnturaYarnVariables : InMemoryVariableStorage, IGeneratedVariableStorage
    {
        // Accessor for Bool $met_teacher_1
        public bool MetTeacher1
        {
            get => this.GetValueOrDefault<bool>("$met_teacher_1");
            set => this.SetValue<bool>("$met_teacher_1", value);
        }

        // Accessor for Bool $got_hint
        public bool GotHint
        {
            get => this.GetValueOrDefault<bool>("$got_hint");
            set => this.SetValue<bool>("$got_hint", value);
        }

        // Accessor for String $current_item
        public string CurrentItem
        {
            get => this.GetValueOrDefault<string>("$current_item");
            set => this.SetValue<string>("$current_item", value);
        }

        // Accessor for Bool $doorUnlocked
        public bool DoorUnlocked
        {
            get => this.GetValueOrDefault<bool>("$doorUnlocked");
            set => this.SetValue<bool>("$doorUnlocked", value);
        }

        // Accessor for Number $coins
        public float Coins
        {
            get => this.GetValueOrDefault<float>("$coins");
            set => this.SetValue<float>("$coins", value);
        }

        // Accessor for Bool $IS_DESKTOP
        public bool ISDESKTOP
        {
            get => this.GetValueOrDefault<bool>("$IS_DESKTOP");
            set => this.SetValue<bool>("$IS_DESKTOP", value);
        }

        // Accessor for Number $TOTAL_COINS
        public float TOTALCOINS
        {
            get => this.GetValueOrDefault<float>("$TOTAL_COINS");
            set => this.SetValue<float>("$TOTAL_COINS", value);
        }

        // Accessor for Number $COLLECTED_ITEMS
        public float COLLECTEDITEMS
        {
            get => this.GetValueOrDefault<float>("$COLLECTED_ITEMS");
            set => this.SetValue<float>("$COLLECTED_ITEMS", value);
        }

        // Accessor for Number $MAX_PROGRESS
        public float MAXPROGRESS
        {
            get => this.GetValueOrDefault<float>("$MAX_PROGRESS");
            set => this.SetValue<float>("$MAX_PROGRESS", value);
        }

        // Accessor for String $CURRENT_ITEM
        public string CURRENTITEM
        {
            get => this.GetValueOrDefault<string>("$CURRENT_ITEM");
            set => this.SetValue<string>("$CURRENT_ITEM", value);
        }
    }
}
