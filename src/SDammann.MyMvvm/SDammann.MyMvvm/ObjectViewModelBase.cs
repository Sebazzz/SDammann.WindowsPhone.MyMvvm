namespace SDammann.MyMvvm {
    using System.Diagnostics;
    using Utils.Collections.ObjectModel;


    /// <summary>
    ///   Represents a view model for an object that displays some of the objects properties
    /// </summary>
    /// <typeparam name="TModel"> </typeparam>
    public abstract class ObjectViewModelBase<TModel> : ViewModelBase, IViewModelFor<TModel> where TModel : class {
        private readonly TModel _model;

        /// <summary>
        ///   Gets the underlying model for this class. Derived classes may optionally make this property publicly visible.
        /// </summary>
        protected TModel Model {
            [DebuggerStepThrough]
            get { return this._model; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ObjectViewModelBase&lt;TModel&gt;" /> class.
        /// </summary>
        /// <param name="model"> The model. </param>
        protected ObjectViewModelBase (TModel model) {
            this._model = model;
        }


        #region IViewModelFor<TModel> Members

        TModel IViewModelFor<TModel>.Model {
            get { return this._model; }
        }

        #endregion
    }
}