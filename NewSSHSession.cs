namespace SSH
{
    using System.Management.Automation;

    /// <summary>
    /// 
    /// </summary>
    [Cmdlet(VerbsCommon.New, "SSHSession", DefaultParameterSetName = "NoKey")]
    public class NewSshSession : NewSessionBase
    {
        internal override string Protocol
        {
            get
            {
                return "SSH";
            }
        }
    }
    //end of the class for the New-SSHSession
    //###################################################
}
