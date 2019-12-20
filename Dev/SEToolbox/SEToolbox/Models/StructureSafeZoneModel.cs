namespace SEToolbox.Models
{
    using System;
    using System.Runtime.Serialization;

    using SEToolbox.Interop;
    using VRage.ObjectBuilders;

    using Sandbox.Common.ObjectBuilders;
    using System.Xml.Serialization;

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
