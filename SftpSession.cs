namespace SSH
{
    using Renci.SshNet;
    using System;

    // Object for SSH Sessions
    public class SftpSession
    {
        public Int32 SessionId;
        public string Host;
        public SftpClient Session;
        public bool Connected
        {
            get { return Session.IsConnected; }
        }
        public void Disconnect()
        {
            Session.Disconnect();
        }

        // Method for Connecing
        public void Connect()
        {
            Session.Connect();
        }
    }
}
