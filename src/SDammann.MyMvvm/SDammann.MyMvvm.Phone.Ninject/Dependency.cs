namespace SDammann.MyMvvm.Phone.Ninject {
    using System;
    using System.Diagnostics;
    using System.Linq;
    using global::Ninject;
    using global::Ninject.Activation.Caching;
    using global::Ninject.Modules;


    /// <summary>
    ///   Static helper class for obtaining dependencies
    /// </summary>
    public static class Dependency {
        private static Func<IKernel> KernelInitializer = InitializeKernelDefault;
        private static bool IsKernelInitialized;
        private static IKernel Kernel;
        private static INinjectModule[] ModulesToLoad = new INinjectModule[0];

        /// <summary>
        ///   Loads the specified ninject modules in the kernel or puts the on the list if the kernel hasn't been initialized yet
        /// </summary>
        /// <param name="modulesToLoad"> </param>
        public static void SetNinjectModulesToLoad (params INinjectModule[] modulesToLoad) {
            if (!IsKernelInitialized) {
                ModulesToLoad = ModulesToLoad.Union(modulesToLoad).ToArray();
            } else {
                foreach (INinjectModule ninjectModule in modulesToLoad) {
                    Kernel.Load(ninjectModule);
                }
            }
        }

        /// <summary>
        ///   Sets the initializer used for initializing the kernel
        /// </summary>
        /// <param name="initializer"> </param>
        public static void SetKernelInitializer (Func<IKernel> initializer) {
            if (IsKernelInitialized) {
                throw new InvalidOperationException("The dependency resolvement system has already been initialized");
            }

            KernelInitializer = initializer;
        }

        /// <summary>
        ///   Default method for initializing the kernel if another one wasn't provided via dependency injection
        /// </summary>
        /// <returns> </returns>
        private static IKernel InitializeKernelDefault() {
            StandardKernel kernel = new StandardKernel();
            foreach (INinjectModule module in ModulesToLoad) {
                kernel.Load(module);
            }

            return kernel;
        }

        /// <summary>
        ///   Initializes this class if it isn't done yet
        /// </summary>
        private static void InitializeIfNecessary() {
            if (IsKernelInitialized) {
                return;
            }

            // call initializer
            Kernel = KernelInitializer.Invoke();

            // OK
            IsKernelInitialized = true;
        }

        /// <summary>
        /// Gets a <see cref="IKernel"/> instance
        /// </summary>
        /// <returns></returns>
        internal static IKernel GetKernelInstance() {
            InitializeIfNecessary();

            return Kernel;
        }

        /// <summary>
        ///   Returns the specified service from the dependency provider.
        /// </summary>
        /// <typeparam name="TService"> Type of service to return </typeparam>
        /// <returns> </returns>
        [DebuggerStepThrough]
        public static TService Get<TService>() where TService : class {
            InitializeIfNecessary();

            return Kernel.Get<TService>();
        }

        /// <summary>
        ///   Resets all dependency injection by disposing all created instances by the kernel. Note this doesn't reload all the modules, it only clears the cache of activated instances.
        /// </summary>
        public static void ResetDependencies() {
            if (!IsKernelInitialized) {
                return;
            }

            Kernel.Components.Get<ICache>().Clear();
            Kernel.Components.Get<IActivationCache>().Clear();
        }

        /// <summary>
        ///   Resets all dependency injection by disposing all created instances by the kernel. Note this reinitializes this class, but doesn't reset the Kernel initializer.
        /// </summary>
        public static void ResetKernel() {
            if (!IsKernelInitialized) {
                return;
            }

            Kernel.Dispose();
            Kernel = null;

            IsKernelInitialized = false;
        }
    }
}