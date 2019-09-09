using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.Expressions;

namespace urlql.test.Parsers
{
    public class FilteringStatementParserTests : Specification
    {
        public string expression { get; set; }
        public IList<IFilteringStatement> statements { get; set; }
    }

    public class FilterStatement : FilteringStatementParserTests
    {
        private urlql.Parsers.FilteringExpressionParser parser { get; set; }

        protected override void Arrange()
        {
            base.Arrange();
            expression = "Id eq 0";
            parser = new urlql.Parsers.FilteringExpressionParser();
        }

        protected override void Act()
        {
            statements = parser.ParseExpression(expression);
        }

        [Test]
        public void HasEqualStatement()
        {
            Assert.AreEqual(statements.Count(), 1);
            var statement = statements.First() as Comparison;
            Assert.NotNull(statement);
            Assert.AreEqual(statement.ComparisonOperation, ComparisonOperation.eq);
            Assert.AreEqual(statement.LeftOperand, "Id");
            Assert.AreEqual(statement.RightOperand, "0");
        }
    }

    public class FilterAnd : FilteringStatementParserTests
    {
        private urlql.Parsers.FilteringExpressionParser parser { get; set; }

        protected override void Arrange()
        {
            base.Arrange();
            expression = "Id eq 0 and name ieq \"Bobby Tables\"";
            parser = new urlql.Parsers.FilteringExpressionParser();
        }

        protected override void Act()
        {
            statements = parser.ParseExpression(expression);
        }

        [Test]
        public void HasEqualAndStatement()
        {
            Assert.AreEqual(statements.Count(), 3);

            var comp = statements.ElementAtOrDefault(0) as Comparison;
            Assert.NotNull(comp);
            Assert.AreEqual(comp.ComparisonOperation, ComparisonOperation.eq);
            Assert.AreEqual(comp.LeftOperand, "Id");
            Assert.AreEqual(comp.RightOperand, "0");

            comp = statements.ElementAtOrDefault(2) as Comparison;
            Assert.NotNull(comp);
            Assert.AreEqual(comp.ComparisonOperation, ComparisonOperation.ieq);
            Assert.AreEqual(comp.LeftOperand, "name");
            Assert.AreEqual(comp.RightOperand, "\"Bobby Tables\"");

            var logical = statements.ElementAtOrDefault(1) as LogicalConnective;
            Assert.NotNull(logical);
            Assert.AreEqual(logical.LogicalOperation, LogicalOperation.and);
        }
    }

    public class FilterOr : FilteringStatementParserTests
    {
        private urlql.Parsers.FilteringExpressionParser parser { get; set; }

        protected override void Arrange()
        {
            base.Arrange();
            expression = "Id eq 0 or name ieq \"Bobby Tables\"";
            parser = new urlql.Parsers.FilteringExpressionParser();
        }

        protected override void Act()
        {
            statements = parser.ParseExpression(expression);
        }

        [Test]
        public void HasEqualOrStatement()
        {
            Assert.AreEqual(statements.Count(), 3);

            var comp = statements.ElementAtOrDefault(0) as Comparison;
            Assert.NotNull(comp);
            Assert.AreEqual(comp.ComparisonOperation, ComparisonOperation.eq);
            Assert.AreEqual(comp.LeftOperand, "Id");
            Assert.AreEqual(comp.RightOperand, "0");

            comp = statements.ElementAtOrDefault(2) as Comparison;
            Assert.NotNull(comp);
            Assert.AreEqual(comp.ComparisonOperation, ComparisonOperation.ieq);
            Assert.AreEqual(comp.LeftOperand, "name");
            Assert.AreEqual(comp.RightOperand, "\"Bobby Tables\"");

            var logical = statements.ElementAtOrDefault(1) as LogicalConnective;
            Assert.NotNull(logical);
            Assert.AreEqual(logical.LogicalOperation, LogicalOperation.or);
        }
    }
}
