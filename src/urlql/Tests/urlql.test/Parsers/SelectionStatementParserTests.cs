using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.Expressions;


namespace urlql.test.Parsers
{
    public class SelectionStatementParserTests : Specification
    {
        public string expression { get; set; }
        public IList<ISelectionStatement> statements { get; set; }
    }

    public class SelectStatement : SelectionStatementParserTests
    {
        private urlql.Parsers.SelectionExpressionParser parser { get; set; }

        protected override void Arrange()
        {
            base.Arrange();
            expression = "Id, Dollars as money";
            parser = new urlql.Parsers.SelectionExpressionParser();
        }

        protected override void Act()
        {
            statements = parser.ParseExpression(expression);
        }

        [Test]
        public void HasSelectionStatement()
        {
            Assert.AreEqual(statements.Count(), 2);
            var statement = statements.ElementAt(0) as Selection;
            Assert.NotNull(statement);
            Assert.AreEqual(statement.PropertyName, "Id");
            Assert.AreEqual(statement.Alias, null);

            statement = statements.ElementAt(1) as Selection;
            Assert.NotNull(statement);
            Assert.AreEqual(statement.PropertyName, "Dollars");
            Assert.AreEqual(statement.Alias.NewName, "money");
        }
    }
}
