using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QueueDemoTest
{
    [TestClass]
    public class LocalQueueUnitTest
    {
        // This method ensures that MyQueue's splitting off into LocalQueue and CloudQueue
        // happens correctly.
        [TestMethod]
        public void Test_LocalQueueInitialization()
        {
            QueueDemo.MyQueue q = new QueueDemo.MyQueue(false, 100);

            string ExpectedQueueType = "Local";
            string ActualQueueType = q.QueueType();
            Assert.AreEqual(ExpectedQueueType, ActualQueueType);
        }

        [TestMethod]
        public void Test_EnqueueDequeueForOneElement()
        {
            QueueDemo.MyQueue q = new QueueDemo.MyQueue(false, 100);

            string ExpectedToBeDequeued = "foo";
            string ActuallyDequeued;

            q.Enqueue(ExpectedToBeDequeued);
            ActuallyDequeued = q.Dequeue();

            Assert.AreEqual(ExpectedToBeDequeued, ActuallyDequeued);
        }

        // This method pushes the LocalQueue to its limit on both the Enqueue and Dequeue
        // side of things to make sure that it's catching errors at this endpoints correctly.
        [TestMethod]
        public void Test_EnqueueWhenFullDequeueWhenEmpty()
        {
            QueueDemo.MyQueue q = new QueueDemo.MyQueue(false, 10);

            // Enqueue when full
            int ExpectedReturnValue;
            int ActualReturnValue;

            for (int i = 0; i < 10; i++)
            {
                ExpectedReturnValue = 0;
                ActualReturnValue = q.Enqueue(i.ToString());

                Assert.AreEqual(ExpectedReturnValue, ActualReturnValue);
            }

            ExpectedReturnValue = -1;
            ActualReturnValue = q.Enqueue("foo");

            Assert.AreEqual(ExpectedReturnValue, ActualReturnValue);

            // Dequeue when empty
            string ExpectedStringReturnValue;
            string ActualStringReturnValue;

            for (int i = 0; i < 10; i++)
            {
                ExpectedStringReturnValue = i.ToString();
                ActualStringReturnValue = q.Dequeue();

                Assert.AreEqual(ExpectedStringReturnValue, ActualStringReturnValue);
            }

            ExpectedStringReturnValue = null;
            ActualStringReturnValue = q.Dequeue();

            Assert.AreEqual(ExpectedStringReturnValue, ActualStringReturnValue);
        }
    }
}