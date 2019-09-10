using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.Expressions;

namespace urlql.test.Parsers
{
    public class OrderingStatementParserTests : Specification
    {
        public string expression { get; set; }
        public IList<IOrderingStatement> statements { get; set; }
        public urlql.Parsers.OrderingExpressionParser parser { get; set; }
    }

    public class OrderingStatement : OrderingStatementParserTests
    {

        protected override void Arrange()
        {
            base.Arrange();
            expression = "Id asc";
            parser = new urlql.Parsers.OrderingExpressionParser();
        }

        protected override void Act()
        {
            statements = parser.ParseExpression(expression);
        }

        [Test]
        public void OrderByIdAsc()
        {
            Assert.AreEqual(statements.Count(), 1);
            var statement = statements.First() as Ordering;
            Assert.NotNull(statement);
            Assert.AreEqual(statement.OrderingOperation, OrderingOperation.asc);
            Assert.AreEqual(statement.PropertyName, "Id");
        }
    }

    public class MultipleOrderingStatements : OrderingStatementParserTests
    {

        protected override void Arrange()
        {
            base.Arrange();
            expression = "Id asc, Name desc";
            parser = new urlql.Parsers.OrderingExpressionParser();
        }

        protected override void Act()
        {
            statements = parser.ParseExpression(expression);
        }

        [Test]
        public void HasTwoStatements()
        {
            Assert.AreEqual(statements.Count(), 2);
            var statement = statements.First() as Ordering;
            Assert.NotNull(statement);
            Assert.AreEqual(statement.OrderingOperation, OrderingOperation.asc);
            Assert.AreEqual(statement.PropertyName, "Id");
            statement = statements.Last() as Ordering;
            Assert.NotNull(statement);
            Assert.AreEqual(statement.OrderingOperation, OrderingOperation.desc);
            Assert.AreEqual(statement.PropertyName, "Name");
        }
    }

}
