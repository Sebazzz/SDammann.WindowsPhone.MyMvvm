namespace SDammann.MyMvvm.Phone.Ninject {
    using System;
    using Utils.ServiceLocator;
    using global::Ninject;


    /// <summary>
    /// <see cref="IServiceProviderEx"/> implementation via Ninject
    /// </summary>
    public sealed class NinjectServiceResolver : IServiceProviderEx {
        private static IServiceProviderEx ServiceResolverInstance;
        private readonly IKernel kernelToUse;

        /// <summary>
        ///   Gets a service resolver instance that uses the <see cref="Dependency" /> class
        /// </summary>
        public static IServiceProviderEx WithDependency {
            get { return ServiceResolverInstance ?? (ServiceResolverInstance = new NinjectServiceResolver()); }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="NinjectServiceResolver" /> class.
        /// </summary>
        /// <param name="kernelToUse"> The kernel to use. </param>
        public NinjectServiceResolver (IKernel kernelToUse) {
            this.kernelToUse = kernelToUse;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="NinjectServiceResolver"/> class from being created.
        /// </summary>
        private NinjectServiceResolver() {
            this.kernelToUse = Dependency.GetKernelInstance();
        }

        #region IServiceProviderEx Members

        /// <summary>
        ///   Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType"> An object that specifies the type of service object to get. </param>
        /// <returns> A service object of type <paramref name="serviceType" /> .-or- null if there is no service object of type <paramref
        ///    name="serviceType" /> . </returns>
        public object GetService (Type serviceType) {
            try {
                return this.kernelToUse.Get(serviceType);
            } catch (ActivationException) {
                return null;
            }
        }

        /// <summary>
        ///   Injects the existing instance with dependencies
        /// </summary>
        /// <param name="instance"> The instance. </param>
        public void Inject (object instance) {
            this.kernelToUse.Inject(instance);
        }

        #endregion
    }
}