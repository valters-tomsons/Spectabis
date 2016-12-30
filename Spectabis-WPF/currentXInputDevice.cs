using SharpDX.XInput;

namespace Spectabis_WPF
{
    class currentXInputDevice
    {
        private Controller[] xDevices = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

        //Returns first connected xInput controller
        public Controller getActiveController()
        {
            foreach (Controller device in xDevices)
            {
                if (device.IsConnected == true)
                {
                    //Return first connected controller
                    return device;
                }
            }
            return null;
        }
    }
}
