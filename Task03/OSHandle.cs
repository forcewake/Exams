using System;

namespace Task03
{
    /// <summary>
    /// Implementing the IDisposable interface signals users of 
    /// this class that it offers the dispose pattern. 
    /// </summary>
    public sealed class OSHandle : IDisposable
    {

        // This field holds the Win32 handle of the unmanaged resource. 
        private IntPtr _handle;

        // This constructor initializes the handle. 
        public OSHandle(IntPtr handle)
        {
            this._handle = handle;
        }

        // When garbage collected, this Finalize method, which 
        // will close the unmanaged resource's handle, is called. 
        ~OSHandle()
        {
            Dispose(false);
        }

        // This public method can be called to deterministically 
        // close the unmanaged resource's handle. 
        public void Dispose()
        {
            // Because the object is explicitly cleaned up, stop the 
            // garbage collector from calling the Finalize method. 
            GC.SuppressFinalize(this);

            // Call the method that actually does the cleanup. 
            Dispose(true);
        }

        // This public method can be called instead of Dispose. 
        public void Close()
        {
            Dispose();
        }

        // The common method that does the actual cleanup. 
        // Finalize, Dispose, and Close call this method. 
        // Because this class is sealed, this method is private. 
        // If this class weren't sealed, this method wouldn’t be protected. 
        private void Dispose(bool disposing)
        {
            // Synchronize threads calling Dispose/Close simultaneously. 
            lock (this)
            {
                if (disposing)
                {
                    // The object is being explicitly disposed of/closed, not 
                    // finalized. It is therefore safe for code in this if 
                    // statement to access fields that reference other 
                    // objects because the Finalize method of these other objects 
                    // hasn’t yet been called. 
                    // For the OSHandle class, there is nothing to do in here. 
                }

                // The object is being disposed of/closed or finalized. 
                if (IsValid)
                {
                    // If the handle is valid, close the unmanaged resource. 
                    // NOTE: Replace CloseHandle with whatever function is 
                    // necessary to close/free your unmanaged resource. 
                    CloseHandle(_handle);

                    // Set the handle field to some sentinel value. This precaution 
                    // prevents the possibility of calling CloseHandle twice. 
                    _handle = InvalidHandle;
                }
            }
        }

        // Public property to return the value of an invalid handle. 
        // NOTE: Make this property return an invalid value for 
        // whatever unmanaged resource you're using. 
        public IntPtr InvalidHandle
        {
            get { return IntPtr.Zero; }
        }

        // Public method to return the value of the wrapped handle 
        public IntPtr ToHandle()
        {
            if (IsInvalid)
                throw new ObjectDisposedException("The handle was closed.");
            return _handle;
        }

        // Public implicit cast operator returns the value of the wrapped handle 
        public static implicit operator IntPtr(OSHandle osHandle)
        {
            return osHandle.ToHandle();
        }

        // Public properties to return whether the wrapped handle is valid. 
        public bool IsValid
        {
            get { return (_handle != InvalidHandle); }
        }

        public bool IsInvalid
        {
            get { return !IsValid; }
        }

        // Private method called to free the unmanaged resource. 
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static bool CloseHandle(IntPtr handle);
    }
}