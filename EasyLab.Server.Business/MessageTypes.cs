
namespace EasyLab.Server.Business
{
    public enum MessageTypes
    {
        #region Server Message
        
        #endregion

        #region Device Message

        //Init and Sync user list
        InitUsersDataFromServer = 1001,

        //Post auditlog to server
        PostbackAuditLogs = 1002,

        //Update labs and instruments list
        UpdateLabInstrumentList = 1003,

        //Update and add reserver queue. 
        AddAndUpdateReserverQueue = 1004,

        //Send reserve queue info
        SendReserveQueueInfo = 1005,

        //Admin manager cancel reserve record
        SendClientCancelReserve = 1006

        #endregion
    }
}
