# NHibernateLockTest
I created this project to track down an error I am receiving when using NHibernate with MS SQL


Just run the code as-is and it should error out on line 45 of Program.cs, which has this:

    using (var transaction = session.BeginTransaction())
    
First restore any nuget packages of course.

The error is the following

    An unhandled exception of type 'NHibernate.TransactionException' occurred in NHibernate.dll
    Begin failed with SQL exception occurred
    
    New transaction is not allowed because there are other threads running in the session.

with stack trace:

       at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action'1 wrapCloseInAction)
       at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action'1 wrapCloseInAction)
       at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
       at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
       at System.Data.SqlClient.TdsParser.Run(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj)
       at System.Data.SqlClient.TdsParser.TdsExecuteTransactionManagerRequest(Byte[] buffer, TransactionManagerRequestType request, String transactionName, TransactionManagerIsolationLevel isoLevel, Int32 timeout, SqlInternalTransaction transaction, TdsParserStateObject stateObj, Boolean isDelegateControlRequest)
       at System.Data.SqlClient.SqlInternalConnectionTds.ExecuteTransactionYukon(TransactionRequest transactionRequest, String transactionName, IsolationLevel iso, SqlInternalTransaction internalTransaction, Boolean isDelegateControlRequest)
       at System.Data.SqlClient.SqlInternalConnectionTds.ExecuteTransaction(TransactionRequest transactionRequest, String name, IsolationLevel iso, SqlInternalTransaction internalTransaction, Boolean isDelegateControlRequest)
       at System.Data.SqlClient.SqlInternalConnection.BeginSqlTransaction(IsolationLevel iso, String transactionName, Boolean shouldReconnect)
       at System.Data.SqlClient.SqlConnection.BeginTransaction(IsolationLevel iso, String transactionName)
       at System.Data.SqlClient.SqlConnection.BeginDbTransaction(IsolationLevel isolationLevel)
       at System.Data.Common.DbConnection.BeginTransaction(IsolationLevel isolationLevel)
       at NHibernate.Transaction.AdoTransaction.Begin(IsolationLevel isolationLevel)
