using System;

namespace CSP
{
    public class Constraint<T>
    {
        public Variable<T> variable { get; private set; }
        public object restriction { get; private set; }
        // object: Variable or T -> restriction, solution, T: guess, bool: answer
        public Func<object, Solution<T>, T, bool> satisfy { get; private set; }

        public Constraint(Variable<T> variable, object restriction, Func<object, Solution<T>, T, bool> satisfy)
        {
            this.variable = variable;
            this.restriction = restriction;
            this.satisfy = satisfy;
        }
    }
}
