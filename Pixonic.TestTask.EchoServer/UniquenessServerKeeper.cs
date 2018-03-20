using System.Diagnostics.Contracts;
using System.Threading;

namespace Pixonic.TestTask.EchoServer
{
    public class UniquenessServerKeeper
    {
        #region Private Fields

        private readonly string _moduleKey;
        private Mutex _moduleMutex;

        #endregion Private Fields

        #region Public Constructors

        public UniquenessServerKeeper(string moduleKey)
        {
            Contract.Assert(!string.IsNullOrEmpty(moduleKey), "!string.IsNullOrEmpty(moduleKey)");

            _moduleKey = moduleKey;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool CheckUniquenessModule()
        {
            _moduleMutex = new Mutex(true, _moduleKey, out var moduleExisted);

            return moduleExisted;
        }

        public void ReleaseUniqueModuleKey()
        {
            _moduleMutex?.Close();
        }

        #endregion Public Methods
    }
}