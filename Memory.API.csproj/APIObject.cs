using System;

namespace Memory.API
{
    public class APIObject : IDisposable
    {
        private int id;

        private bool disposedValue = false;

        public APIObject(int id)
        {
            this.id = id;
            MagicAPI.Allocate(id);
        }

        ~APIObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                if (MagicAPI.Contains(id))
                    MagicAPI.Free(id);
                disposedValue = true;
            }
        }
    }
}