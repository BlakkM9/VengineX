using System.Runtime.InteropServices;

namespace VengineX.Utils
{
    public unsafe struct UnmanagedArray<T> where T : unmanaged
    {
        /// <summary>
        /// Pointer to the first element of the array (IntPtr)
        /// </summary>
        public IntPtr Pointer { get; }

        /// <summary>
        /// Pointer to the first element of the array (T*)
        /// </summary>
        public T* NativePointer { get; }


        /// <summary>
        /// Elements in the unmanaged array
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Access to the elements. Does not check for bounds.
        /// </summary>
        /// <returns>Element at given index i.</returns>
        public T this[int i]
        {
            get { return *(NativePointer + i); }
            set { *(NativePointer + i) = value; }
        }

        /// <summary>
        /// Access to the elements. Does not check for bounds.
        /// </summary>
        /// <returns>Element at given index i.</returns>
        public T this[uint i]
        {
            get { return *(NativePointer + i); }
            set { *(NativePointer + i) = value; }
        }

        /// <summary>
        /// Creates a new unmanaged array
        /// </summary>
        /// <param name="length">Length (items) of the array</param>
        public UnmanagedArray(int length)
        {
            Pointer = Marshal.AllocHGlobal(sizeof(T) * length);
            NativePointer = (T*)Pointer;
            Length = length;
        }


        public UnmanagedArray(T* dataPtr, int length)
        {
            Pointer = (IntPtr)dataPtr;
            NativePointer = dataPtr;
            Length = length;
        }


        /// <summary>
        /// Free's the memory of this array.
        /// Needs to be called to prevent memory leaks
        /// </summary>
        public void Free()
        {
            Marshal.FreeHGlobal(Pointer);
        }
    }
}
