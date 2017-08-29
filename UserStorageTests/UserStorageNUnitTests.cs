using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using UserStorage.Interfaces;
using UserStorage.Entities;
using UserStorage.Storages;

namespace UserStorageTests
{
    public class UserStorageNUnitTests
    {
        public IEnumerable<TestCaseData> AddTestData
        {
            get
            {
                yield return new TestCaseData(null, null).Throws(typeof(ArgumentNullException));
                yield return new TestCaseData(new User(String.Empty, String.Empty, -5), new User(String.Empty, String.Empty, -5)).Throws(typeof(ArgumentException));
                yield return new TestCaseData(new User() { Id = 1, FirstName = "A", LastName = "A", Age = 20 },
                                              new User() { Id = 1, FirstName = "A", LastName = "A", Age = 20 }).Throws(typeof(InvalidOperationException));
                yield return new TestCaseData(new User() { Id = 1, FirstName = "A", LastName ="A", Age = 20 },
                                              new User() { Id = 2, FirstName = "B", LastName = "B", Age = 20 }).Returns(true);
            }
        }

        [Test, TestCaseSource("AddTestData")]
        public bool AddToStorage_Test(User user1, User user2)
        {  
            UserStorageHashSet storage = new UserStorageHashSet();
            
            storage.Add(user1);
            storage.Add(user2);
        
            return true;
        }

        public IEnumerable<TestCaseData> RemoveGetByIdTestData
        {
            get
            {
                yield return new TestCaseData(-1).Throws(typeof(ArgumentException));
                yield return new TestCaseData(5).Throws(typeof(InvalidOperationException));
                yield return new TestCaseData(0).Returns(true);
            }
        }

        [Test, TestCaseSource("RemoveGetByIdTestData")]
        public bool RemoveFromStorage_Test(int id)
        {
            UserStorageHashSet storage = new UserStorageHashSet();

            storage.Add(new User("A", "A", 10));

            storage.Remove(id);

            return true;
        }

        [Test, TestCaseSource("RemoveGetByIdTestData")]
        public bool GetByIdFromStorage_Test(int id)
        {
            UserStorageHashSet storage = new UserStorageHashSet();

            storage.Add(new User("A", "A", 10));

            storage.GetById(id);

            return true;
        }

    }
}
