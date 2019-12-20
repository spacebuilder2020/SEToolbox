namespace SEToolbox.Models
{
    using System;
    using System.Runtime.Serialization;

    using SEToolbox.Interop;
    using VRage.ObjectBuilders;

    using Sandbox.Common.ObjectBuilders;
    using System.Xml.Serialization;
    using VRageMath;

    [Serializable]
    public class StructureSafeZoneModel : StructureBaseModel
    {
        #region ctor

        public StructureSafeZoneModel(MyObjectBuilder_EntityBase entityBase)
            : base(entityBase)
        {
        }

        #endregion

        #region methods

        [XmlIgnore]
        public MyObjectBuilder_SafeZone SafeZone
        {
            get { return EntityBase as MyObjectBuilder_SafeZone; }
        }
        [XmlIgnore]
        public string Name
        {
            get { return SafeZone.DisplayName; }

            set
            {
                if (value != SafeZone.DisplayName)
                {
                    SafeZone.DisplayName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [XmlIgnore]
        public float Radius
        {
            get { return SafeZone.Radius; }
            set
            {
                if (value != SafeZone.Radius)
                {
                    SafeZone.Radius = value;
                    OnPropertyChanged(nameof(Radius));
                }
            }
        }

        [XmlIgnore]
        public Vector3 Size
        {
            get { return SafeZone.Size; }
            set
            {
                if (value != SafeZone.Size)
                {
                    SafeZone.Size = value;
                    OnPropertyChanged(nameof(Size));
                }
            }
        }

        [XmlIgnore]
        public MySafeZoneShape Shape
        {
            get { return SafeZone.Shape; }
            set
            {
                if (value != SafeZone.Shape)
                {
                    SafeZone.Shape = value;
                    OnPropertyChanged(nameof(Shape));
                }
            }
        }

        [XmlIgnore]
        public bool Enabled
        {
            get { return SafeZone.Enabled; }
            set
            {
                if (value != SafeZone.Enabled)
                {
                    SafeZone.Enabled = value;
                    OnPropertyChanged(nameof(Enabled));
                }
            }
        }

        [XmlIgnore]
        public bool IsVisible
        {
            get { return SafeZone.IsVisible; }
            set
            {
                if (value != SafeZone.IsVisible)
                {
                    SafeZone.IsVisible = value;
                    OnPropertyChanged(nameof(IsVisible));
                }
            }
        }

        [XmlIgnore]
        public long SafeZoneBlockId
        {
            get { return SafeZone.SafeZoneBlockId; }
            set
            {
                if (value != SafeZone.SafeZoneBlockId)
                {
                    SafeZone.SafeZoneBlockId = value;
                    OnPropertyChanged(nameof(SafeZoneBlockId));
                }
            }
        }
        [OnSerializing]
        private void OnSerializingMethod(StreamingContext context)
        {
            SerializedEntity = SpaceEngineersApi.Serialize<MyObjectBuilder_EntityBase>(EntityBase);
        }

        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            EntityBase = SpaceEngineersApi.Deserialize<MyObjectBuilder_EntityBase>(SerializedEntity);
        }

        public override void UpdateGeneralFromEntityBase()
        {
            ClassType = ClassType.SafeZone;
            DisplayName = EntityBase.TypeId.ToString();
        }

        #endregion
    }
}
