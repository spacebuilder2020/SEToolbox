namespace SEToolbox.ViewModels
{    
    using SEToolbox.Models;

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

        #endregion
    }
}
