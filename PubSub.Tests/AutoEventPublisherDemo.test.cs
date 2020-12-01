using NUnit.Framework;
using PubSubServer;

namespace PubSub.Tests
{
    [TestFixture]
    public class AutoEventPublisherDemoTests
    {       

        [Test]
        public void OpenServerDoesNotThrowExceptions()
        {
            // arrange
            using (var test = new AutoEventPublisherDemo())
            {
                // act
                test.OpenServerAndPublishEvents();

                // assert
                Assert.IsNotNull(test);
            }
        }     
        
        [Test]
        public void UnSubscribeDoesNotThrowExceptions()
        {
            // arrange
            using (var test = new AutoEventPublisherDemo())
            {

                // act
                test.Unsubscribe();
                test.OpenServerAndPublishEvents();
                test.Unsubscribe();

                // assert
                Assert.IsNotNull(test);
            }
        }

    }    
}