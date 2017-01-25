using System;

namespace Autofac.Extras.IocManager
{
    public class RegistrationCompletedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegistrationCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public RegistrationCompletedEventArgs(IResolver resolver)
        {
            Resolver = resolver;
        }

        /// <summary>
        ///     Gets or sets the resolver.
        /// </summary>
        /// <value>
        ///     The resolver.
        /// </value>
        public IResolver Resolver { get; set; }
    }
}
