namespace SEToolbox.ViewModels
{
    using Sandbox.Common.ObjectBuilders;
    using SEToolbox.Models;
    using System;
    using System.Collections.Generic;
    using VRageMath;

    public class StructureSafeZoneViewModel : StructureBaseViewModel<StructureSafeZoneModel>
    {
        #region ctor

        public StructureSafeZoneViewModel(BaseViewModel parentViewModel, StructureSafeZoneModel dataModel)
            : base(parentViewModel, dataModel)
        {
            // Will bubble property change events from the Model to the ViewModel.
            DataModel.PropertyChanged += (sender, e) => OnPropertyChanged(e.PropertyName);
        }

        #endregion

        #region Properties

        protected new StructureSafeZoneModel DataModel
        {
            get { return base.DataModel as StructureSafeZoneModel; }
        }

        public string Name
        {
            get { return DataModel.Name; }
            set { DataModel.Name = value; }
        }

        public float Radius
        {
            get { return DataModel.Radius; }
            set { DataModel.Radius = value; }
        }

        public float SizeX
        {
            get { return DataModel.Size.X; }
            set { DataModel.Size = new Vector3(value, DataModel.Size.Y, DataModel.Size.Z); }
        }

        public float SizeY
        {
            get { return DataModel.Size.Y; }
            set { DataModel.Size = new Vector3(DataModel.Size.X, value, DataModel.Size.Z); }
        }

        public float SizeZ
        {
            get { return DataModel.Size.Z; }
            set { DataModel.Size = new Vector3(DataModel.Size.X, DataModel.Size.Y, value); }
        }

        public bool Enabled
        {
            get { return DataModel.Enabled; }
            set { DataModel.Enabled = value; }
        }

        public bool IsVisible
        {
            get { return DataModel.IsVisible; }
            set { DataModel.IsVisible = value; }
        }

        public List<string> MySafeZoneShapes
        {
            get
            {
                return new List<string>(Enum.GetNames(typeof(MySafeZoneShape)));
            }
        }
        public string Shape
        {
            get { return Enum.GetName(typeof(MySafeZoneShape),DataModel.Shape); }
            set
            {
                if (value == "Box")
                {
                    DataModel.Shape = MySafeZoneShape.Box;
                } else if (value == "Sphere")
                {
                    DataModel.Shape = MySafeZoneShape.Sphere;
                }
            }
        }

        public long SafeZoneBlockId
        {
            get { return DataModel.SafeZoneBlockId; }
            set { DataModel.SafeZoneBlockId = value; }
        }
            #endregion
        }
}
