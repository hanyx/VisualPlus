namespace VisualPlus.Managers
{
    #region Namespace

    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    #endregion

    [Description("The monitor manager.")]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    public class MonitorManager
    {
        #region Variables

        public Rectangle Monitor;
        public Rectangle WorkingArea;

        #endregion

        #region Variables

        private int _byteSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        private char[] _device;

        private int _dwordFlags;

        #endregion

        #region Constructors

        public MonitorManager()
        {
            _byteSize = Marshal.SizeOf(typeof(MonitorManager));
            _dwordFlags = 0;
            Monitor = new Rectangle();
            WorkingArea = new Rectangle();
            _device = new char[32];
        }

        #endregion

        #region Properties

        public int ByteSize
        {
            get
            {
                return _byteSize;
            }

            set
            {
                _byteSize = value;
            }
        }

        public char[] Device
        {
            get
            {
                return _device;
            }

            set
            {
                _device = value;
            }
        }

        public int DWordFlags
        {
            get
            {
                return _dwordFlags;
            }

            set
            {
                _dwordFlags = value;
            }
        }

        #endregion
    }
}