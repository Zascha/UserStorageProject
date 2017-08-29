namespace UserStorage.Entities
{
    #region Usings

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    [DataContract(Name = "User")]
    public class User : IEquatable<User>
    {
        #region Properties

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }

        public bool IsValid {
            get
            {
                if (FirstName.Trim().Length == 0 || LastName.Trim().Length == 0 || Age < 0)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region Constructors

        public User() { }

        public User(string firstName, string lastName, int age)
        {
            if (firstName == null || lastName == null)
            {
                throw new ArgumentNullException("Passed first or last name is null.");
            }

            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        #endregion

        #region Public methods

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return base.Equals(obj);
            }

            if (!(obj is User))
            {
                throw new InvalidCastException("Passed object is not an object of a 'User' class");
            }
            else
            {
                return Equals(obj as User);
            }
        }

        public bool Equals(User other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
        }

        public override string ToString()
        {
            return String.Format("{0}) {1} {2} - {3} years", Id, FirstName, LastName, Age);
        }

        #endregion
    }
}
