namespace FakeItEasy.Expressions.ArgumentConstraints
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FakeItEasy.Core;

    internal class EqualityArgumentConstraint
        : IArgumentConstraint
    {
        public EqualityArgumentConstraint(object expectedValue)
        {
            this.ExpectedValue = expectedValue;
        }

        public object ExpectedValue { get; }

        public string ConstraintDescription => this.ToString();

        public bool IsValid(object argument)
        {
            return object.Equals(this.ExpectedValue, argument);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Any type of exception may be encountered.")]
        public override string ToString()
        {
            try
            {
                var writer = ServiceLocator.Current.Resolve<StringBuilderOutputWriter>();
                writer.WriteArgumentValue(this.ExpectedValue);
                return writer.Builder.ToString();
            }
            catch (Exception)
            {
                FakeManager manager = Fake.TryGetFakeManager(this.ExpectedValue);
                return manager != null
                    ? "Faked " + manager.FakeObjectType
                    : this.ExpectedValue.GetType().ToString();
            }
        }

        public void WriteDescription(IOutputWriter writer)
        {
            writer.Write(this.ConstraintDescription);
        }
    }
}
