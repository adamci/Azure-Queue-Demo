using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace QueueDemo
{
    // This is my wrapper class around the Azure Storage SDK to improve readability of the
    // CloudQueue class. It uses an Azure Storage account.
    // Don't forget to replace ACCOUNTNAME and ACCOUNTKEY with your corresponding account name
    // and key
    class AzureQueuesInTableService
    {
        private CloudTable table;

        public AzureQueuesInTableService(string TableName)
        {
            // Retrieve the storage account from the connection string
            CloudStorageAccount storageAccount =
                   CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ACCOUNTNAME;AccountKey=ACCOUNTKEY");

            // Create the table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist
            table = tableClient.GetTableReference(TableName);
            table.CreateIfNotExists();
        }

        public void Insert(string QueueId, string RowKey, string Data)
        {
            // Create new table entity
            QueueNodeEntity entity = new QueueNodeEntity(QueueId, RowKey);
            entity.Data = Data;

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(entity);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public string Remove(string QueueId, string RowKey)
        {
            // Create a retrieve operation that takes a customer entity
            TableOperation retrieveOperation = TableOperation.Retrieve<QueueNodeEntity>(QueueId, RowKey);

            // Execute the retrieve operation
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a QueueNodeEntity
            QueueNodeEntity deleteEntity = (QueueNodeEntity)retrievedResult.Result;

            // Create the Delete TableOperation
            if (deleteEntity != null)
            {
                string Data = deleteEntity.Data;
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                // Execute the command
                table.Execute(deleteOperation);

                return Data;
            }
            else
            {
                return null;
            }
        }

        public void DeleteTable()
        {
            table.DeleteIfExists();
        }
    }


}


// This class expands the standard TableEntry so that I have an additional column for the data
// in this queue, which takes the form of a string.
class QueueNodeEntity : TableEntity
{
    public string Data { get; set; }

    public QueueNodeEntity(string QueueID, string NodeIndex)
    {
        PartitionKey = QueueID;
        RowKey = NodeIndex;
    }

    public QueueNodeEntity() { }
}
