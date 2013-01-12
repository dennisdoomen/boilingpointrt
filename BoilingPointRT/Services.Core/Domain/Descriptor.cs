using System;
using System.Runtime.Serialization;

using BoilingPointRT.Services.Common;

namespace BoilingPointRT.Services.Domain
{
    [Serializable]
    public class Descriptor : ISerializable
    {
        public Descriptor()
        {
        }

        public Descriptor(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException("code", "Descriptor code cannot be empty.");
            }

            if (code.StartsWith(" ") || code.EndsWith(" "))
            {
                string message = string.Format("Descriptor code '{0}' should not contain white space at the start or end.",
                    code);
                throw new ArgumentException(message, "code");
            }

            Code = code;
        }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Role" /> class.
        /// </summary>
        public Descriptor(SerializationInfo info, StreamingContext context)
        {
            Code = info.GetString("Code");
        }

        /// <summary>
        /// Gets the underlying code represented by this descriptor.
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj" /> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            var other = obj as Descriptor;
            return (other != null) && Code.Equals(other.Code);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetType().Name + "_" + Code;
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Code", Code);
        }

        public static bool operator ==(Descriptor a, Descriptor b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Code.Equals(b.Code);
        }

        public static bool operator !=(Descriptor a, Descriptor b)
        {
            return !(a == b);
        }
    }
}