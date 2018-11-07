namespace SSH
{
    using Renci.SshNet;
    using System;

    // Object for SSH Sessions
    public class SshSession
    {
        public Int32 SessionId;
        public string Host;
        public SshClient Session;
        public bool Connected
        {
            get { return Session.IsConnected; }
        }

        // Method for Connecing
        public void Connect()
        {
            Session.Connect();
        }

        // Method for disconecting session
        public void Disconnect()
        {
            Session.Disconnect();
        }
    }
}