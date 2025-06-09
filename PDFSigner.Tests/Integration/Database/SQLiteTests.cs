using NUnit.Framework;
using PDFSign.Database.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper.Contrib.Extensions;

namespace PDFSigner.Tests.Integration.Database
{
    /// <summary>
    /// Summary description for SQLiteTests
    /// </summary>
    [TestFixture(Category = "Integration")]
    public class SQLiteTests
    {    
        [Test]
        public void DBConnectionFactory_should_open_a_database_instance_given_right_connectionstring()
        {
            // Arrange
            var sqlconnectionfactory = new DbConnectionFactory("Data Source=PDFSign.db");
            // Act
            var sqlconnection = sqlconnectionfactory.GetOpenConnection();
            // Assert
            Assert.IsNotNull(sqlconnection);
            Assert.IsNotNull(sqlconnection.State == ConnectionState.Open);
        }

        [Test]
        public void SQLConnection_should_be_closed_when_factory_is_disposed()
        {
            // Arrange
            var sqlconnectionfactory = new DbConnectionFactory("Data Source=PDFSign.db");
            // Act
            var sqlconnection = sqlconnectionfactory.GetOpenConnection();
            sqlconnectionfactory.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => { var state = sqlconnection.State; } );
        }
    }
}
