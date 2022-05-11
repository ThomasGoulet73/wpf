using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MS.Internal.Text.TextInterface
{
    internal unsafe class FontFileStream : IDWriteFontFileStreamMirror
    {
        private const int S_OK = unchecked((int)0L);
        private const int E_INVALIDARG = unchecked((int)0x80070057L);
        private const int E_FAIL = unchecked((int)0x80004005L);

        private Stream _fontSourceStream;
        private long _lastWriteTime;
        private object _fontSourceStreamLock;

        public FontFileStream(IFontSource fontSource)
        {
            // Previously we were using fontSource->GetStream() which caused crashes in the XPS scenarios
            // as the stream was getting closed by some other object. In XPS scenarios GetStream() would return
            // MS::Internal::IO:Packaging::SynchronizingStream which is owned by the XPS docs and
            // is known to have issues regarding the lifetime management where by if the current XPS page is 
            // flipped then the stream will get disposed. Thus, we cannot rely on the stream directly and hence we now use
            // fontSource->GetUnmanagedStream() which returns a copy of the content of the stream. Special casing XPS will not
            // guarantee that this problem will be fixed so we will use the GetUnmanagedStream(). Note: This path will only 
            // be taken for embedded fonts among which XPS is a main scenario. For local fonts we use DWrite's APIs.
            _fontSourceStream = fontSource.GetUnmanagedStream();
            try
            {
                _lastWriteTime = fontSource.GetLastWriteTimeUtc().ToFileTimeUtc();
            }    
            catch(ArgumentOutOfRangeException) //The resulting file time would represent a date and time before 12:00 midnight January 1, 1601 C.E. UTC. 
            {
                _lastWriteTime = -1;
            }        

            // Create lock to control access to font source stream.
            _fontSourceStreamLock = new object();
        }

        ~FontFileStream()
        {
            _fontSourceStream.Close();
        }

        [ComVisible(true)]
        public int ReadFileFragment(
            void** fragmentStart,
            ulong fileOffset,
            ulong fragmentSize,
            void** fragmentContext
            )
        {
            int hr = S_OK;
            try
            {
                if(
                    fragmentContext == null || fragmentStart == null
                    ||
                    fileOffset   > long.MaxValue                    // Cannot safely cast to long
                    ||            
                    fragmentSize > int.MaxValue                     // fragment size cannot be casted to int
                    || 
                    fileOffset > ulong.MaxValue - fragmentSize           // make sure next sum doesn't overflow
                    || 
                    fileOffset + fragmentSize  > (ulong)_fontSourceStream.Length // reading past the end of the Stream
                  ) 
                {
                    return E_INVALIDARG;
                }

                int fragmentSizeInt = (int)fragmentSize;
                byte[] buffer = new byte[fragmentSizeInt];
            
                // DWrite may call this method from multiple threads. We need to ensure thread safety by making Seek and Read atomic.
                Monitor.Enter(_fontSourceStreamLock);
                try
                {
                    _fontSourceStream.Seek((long)fileOffset, //long
                                            SeekOrigin.Begin);

                    _fontSourceStream.Read(buffer,         //byte[]
                                            0,              //int
                                            fragmentSizeInt //int
                                            );
                }
                finally 
                {
                    Monitor.Exit(_fontSourceStreamLock);
                }

                GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                *fragmentStart = (byte*)(gcHandle.AddrOfPinnedObject().ToPointer());
            
                *fragmentContext = GCHandle.ToIntPtr(gcHandle).ToPointer();
            }
            catch(Exception exception)
            {
                hr = Marshal.GetHRForException(exception);
            }

            return hr;
        }

        [ComVisible(true)]
        public void ReleaseFileFragment(
            void* fragmentContext
            )
        {
            if (fragmentContext != null)
            {
                GCHandle gcHandle = GCHandle.FromIntPtr((IntPtr)fragmentContext);
                gcHandle.Free();
            }
        }

        [ComVisible(true)]
        public int GetFileSize(
            ulong* fileSize
            )
        {
            if (fileSize == null)
            {
                return E_INVALIDARG;
            }

            int hr = S_OK;
            try
            {
                *fileSize = (ulong)_fontSourceStream.Length;
            }
            catch(Exception exception)
            {
                hr = Marshal.GetHRForException(exception);
            }

            return hr;
         }

        public int GetLastWriteTime(
            ulong* lastWriteTime
            )
        {
            if (_lastWriteTime < 0) //The resulting file time would represent a date and time before 12:00 midnight January 1, 1601 C.E. UTC.
            {
                return E_FAIL;
            }
            if (lastWriteTime == null)
            {
                return E_INVALIDARG;
            }        
            *lastWriteTime = (ulong)_lastWriteTime;
            return S_OK;
        }
    }
}
