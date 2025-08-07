namespace Linker.TestCore
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Equivalency;
    using Xunit;

    /// <summary>
    /// The base steps class that provides common steps for testing.
    /// </summary>
    /// <typeparam name="TSteps">The generic step class.</typeparam>
    public abstract class BaseSteps<TSteps>
    {
        /// <summary>
        /// Gets or sets the actual result of the function under test.
        /// </summary>
        public object Result { get; protected set; }

        /// <summary>
        /// Gets or sets the actual exception thrown during the execution.
        /// </summary>
        public Exception Exception { get; protected set; }

        /// <summary>
        /// Records the execution of a piece of code for an exception.
        /// </summary>
        /// <param name="action">The piece of code to execute.</param>
        /// <returns>The steps class.</returns>
        public TSteps RecordException(Action action)
        {
            this.Exception = Record.Exception(action);
            return this.GetSteps();
        }

        /// <summary>
        /// Records the execution of a piece of code for an exception and assign the result.
        /// </summary>
        /// <param name="action">The piece of code to execute.</param>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>The steps class.</returns>
        public TSteps RecordException<TResult>(Func<TResult> action)
        {
            this.Exception = Record.Exception(() => this.Result = action());
            return this.GetSteps();
        }

        /// <summary>
        /// Records the execution of a piece of code for an exception asynchronously.
        /// </summary>
        /// <param name="action">The piece of asynchronous code to execute.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task RecordExceptionAsync(Func<Task> action)
        {
            this.Exception = await Record.ExceptionAsync(action).ConfigureAwait(false);
        }

        /// <summary>
        /// Records the execution of a piece of code for an exception asynchronously
        /// and assigning the resulting value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The piece of asynchronous code to execute.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task RecordExceptionAsync<TResult>(Func<Task<TResult>> action)
        {
            this.Exception = await Record
                .ExceptionAsync(async () => this.Result = await action().ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Expects the result to be the same as the parameter provided.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="obj">The item to compare.</param>
        /// <returns>The steps class.</returns>
        public TSteps ThenIExpectResultToBe<T>(T obj)
        {
            this.Result.Should().NotBeNull();
            this.Result.Should().BeEquivalentTo(obj);
            return this.GetSteps();
        }

        /// <summary>
        /// Expects the result to be the same as the parameter provided along with equivalency options.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="obj">The item to compare.</param>
        /// <param name="opt">The options for comparison.</param>
        /// <returns>The steps class.</returns>
        public TSteps ThenIExpectResultToBe<T>(
            T obj,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> opt)
        {
            this.Result.Should().NotBeNull();
            this.Result.Should().BeEquivalentTo(obj, opt);
            return this.GetSteps();
        }

        /// <summary>
        /// Expects the result to be null.
        /// </summary>
        /// <returns>The step class.</returns>
        public TSteps ThenIExpectResultToBeNull()
        {
            this.Result.Should().BeNull();
            return this.GetSteps();
        }

        /// <summary>
        /// Expect the result to be the same type.
        /// </summary>
        /// <typeparam name="TType">The generic type.</typeparam>
        /// <returns>The step class.</returns>
        public TSteps ThenIExpectResultToBeOfType<TType>()
        {
            this.Result.Should().BeOfType<TType>();
            return this.GetSteps();
        }

        /// <summary>
        /// Expect the result to be the same type.
        /// </summary>
        /// <param name="type">The raw type.</param>
        /// <returns>The step class.</returns>
        public TSteps ThenIExpectResultToBeOfType(Type type)
        {
            this.Result.Should().BeOfType(type);
            return this.GetSteps();
        }

        /// <summary>
        /// Expects no exception is thrown.
        /// </summary>
        /// <returns>The step class.</returns>
        public TSteps ThenIExpectNoExceptionIsThrown()
        {
            this.Exception.Should().BeNull();
            return this.GetSteps();
        }

        /// <summary>
        /// Expects exception of <see cref="TException"/> type to be thrown.
        /// </summary>
        /// <typeparam name="TException">The type of exception.</typeparam>
        /// <returns>The step class.</returns>
        public TSteps ThenIExpectExceptionIsThrown<TException>()
            where TException : Exception
        {
            this.Exception.Should().BeOfType<TException>();
            return this.GetSteps();
        }

        /// <summary>
        /// Expects exception to be thrown.
        /// </summary>
        /// <param name="exceptionType">The raw type of the exception.</param>
        /// <returns>The step class.</returns>
        public TSteps ThenIExpectExceptionIsThrown(Type exceptionType)
        {
            exceptionType.Should().BeAssignableTo<Exception>();
            this.Exception.Should().BeOfType(exceptionType);
            return this.GetSteps();
        }

        /// <summary>
        /// Reference to the step class itself.
        /// </summary>
        /// <returns>The <see cref="TSteps"/> class.</returns>
        public abstract TSteps GetSteps();
    }
}
