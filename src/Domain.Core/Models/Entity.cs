using FluentValidation;
using FluentValidation.Results;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Domain.Core.Models
{

    public abstract class Entity<T> :  AbstractValidator<T> where T : Entity<T>
    {

        #region Variables

        private Guid _id;
        private ValidationResult _validationResult;

        #endregion

        #region Properties

        public Guid Id { get => _id; protected set => _id = value; }
        public ValidationResult ValidationResult { get => _validationResult; protected set => _validationResult = value; }        

        #endregion

        #region Constructors

        protected Entity()
        {
            _validationResult = new ValidationResult();
        }

        #endregion

        #region Methods

        public abstract bool EhValido();

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + "[Id = " + Id + "]";
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        #endregion
    }
}
