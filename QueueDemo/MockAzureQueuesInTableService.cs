using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace QueueDemo
{
    // To create a mock version of my cloud-based queue, I mocked the AzureQueuesInTableService class
    // by having the whole queue exist locally as a List datatype. To simplify the implementation, I have
    // the Insert() and Remove() functions essentially queue and dequeue strings onto this list.
    class MockAzureQueuesInTableService
    {
        private List<string> MockTable;

        public MockAzureQueuesInTableService(string TableName)
        {
            // Initialize the list
            MockTable = new List<string>();
        }

        public void Insert(string QueueId, string RowKey, string Data)
        {
            // Insert the value into the list
            MockTable.Add(Data);
        }

        public string Remove(string QueueId, string RowKey)
        {
            string ReturnValue = MockTable[0];

            // Always dequeue from the top
            MockTable.RemoveAt(0);

            return ReturnValue;
        }

        public void DeleteTable()
        {
            ;
        }
    }
}
